using Microsoft.Data.Sqlite;

namespace QuanLyChungCu.Tests.UI.Infrastructure;

internal static class TestDataSeeder
{
    public static void SeedUsersDatabase(string appBinDirectory)
    {
        Directory.CreateDirectory(appBinDirectory);

        string dbPath = Path.Combine(appBinDirectory, "users.db");
        ExecuteWithRetry(() =>
        {
            using var connection = new SqliteConnection($"Data Source={dbPath};Cache=Shared;Mode=ReadWriteCreate;");
            connection.Open();

            using (var resetUsersTable = connection.CreateCommand())
            {
                resetUsersTable.CommandText =
                    @"CREATE TABLE IF NOT EXISTS Users (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT NOT NULL UNIQUE,
                        Password TEXT NOT NULL,
                        Role INTEGER NOT NULL
                    );
                    DELETE FROM Users;";
                resetUsersTable.ExecuteNonQuery();
            }
            InsertUser(connection, "admin", "1", 1);
        });
    }

    public static void SeedOwnersForManagementTests(string appBinDirectory)
    {
        Directory.CreateDirectory(appBinDirectory);

        string dbPath = Path.Combine(appBinDirectory, "users.db");
        ExecuteWithRetry(() =>
        {
            using var connection = new SqliteConnection($"Data Source={dbPath};Cache=Shared;Mode=ReadWriteCreate;");
            connection.Open();

            using (var ensureTables = connection.CreateCommand())
            {
                ensureTables.CommandText =
                    @"CREATE TABLE IF NOT EXISTS Owners (
                        OwnerID INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Phone TEXT,
                        RoomNumber INTEGER,
                        QueQuan TEXT,
                        SoNguoiO INTEGER DEFAULT 0,
                        BirthDate TEXT
                    );
                    CREATE TABLE IF NOT EXISTS Residents (
                        ResidentID INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Phone TEXT,
                        BirthDate TEXT,
                        QueQuan TEXT,
                        OwnerID INTEGER,
                        FOREIGN KEY (OwnerID) REFERENCES Owners(OwnerID)
                    );
                    CREATE TABLE IF NOT EXISTS Money (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        OwnerID INTEGER NOT NULL,
                        RoomRent INTEGER DEFAULT 0,
                        PowerReading INTEGER DEFAULT 0,
                        WaterReading INTEGER DEFAULT 0,
                        FOREIGN KEY (OwnerID) REFERENCES Owners(OwnerID) ON DELETE CASCADE
                    );
                    DELETE FROM Residents;
                    DELETE FROM Money;
                    DELETE FROM Owners;";
                ensureTables.ExecuteNonQuery();
            }

            InsertOwner(connection, "Chu ho sua", "0901000001", 101, "Ha Noi", "01/01/1980");
            InsertOwner(connection, "Chu ho xoa", "0901000002", 102, "Hai Phong", "02/02/1982");
            InsertOwner(connection, "Chu ho timkiem", "0901000003", 103, "Da Nang", "03/03/1983");
            InsertOwner(connection, "Chu ho khac", "0901000004", 104, "Hue", "04/04/1984");
        });
    }

    private static void ExecuteWithRetry(Action action)
    {
        const int maxAttempts = 5;

        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                action();
                return;
            }
            catch (SqliteException) when (attempt < maxAttempts)
            {
                Thread.Sleep(200 * attempt);
            }
            catch (IOException) when (attempt < maxAttempts)
            {
                Thread.Sleep(200 * attempt);
            }
        }

        action();
    }

    private static void InsertUser(SqliteConnection connection, string username, string password, int role)
    {
        string hash = ComputeSha256Hash(password);
        using var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO Users (Username, Password, Role) VALUES (@username, @password, @role);";
        command.Parameters.AddWithValue("@username", username);
        command.Parameters.AddWithValue("@password", hash);
        command.Parameters.AddWithValue("@role", role);
        command.ExecuteNonQuery();
    }

    private static void InsertOwner(
        SqliteConnection connection,
        string name,
        string phone,
        int roomNumber,
        string queQuan,
        string birthDate)
    {
        using var command = connection.CreateCommand();
        command.CommandText = @"INSERT INTO Owners (Name, Phone, RoomNumber, QueQuan, BirthDate)
                                VALUES (@name, @phone, @roomNumber, @queQuan, @birthDate);";
        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@phone", phone);
        command.Parameters.AddWithValue("@roomNumber", roomNumber);
        command.Parameters.AddWithValue("@queQuan", queQuan);
        command.Parameters.AddWithValue("@birthDate", birthDate);
        command.ExecuteNonQuery();
    }

    private static string ComputeSha256Hash(string value)
    {
        byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(value);
        byte[] hashBytes = System.Security.Cryptography.SHA256.HashData(inputBytes);
        return Convert.ToHexString(hashBytes).ToLowerInvariant();
    }
}
