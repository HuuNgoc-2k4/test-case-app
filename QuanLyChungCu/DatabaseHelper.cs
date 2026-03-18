using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.Sqlite;

namespace QuanLyChungCu
{
    public static class DatabaseHelper
    {
        private static readonly string dbFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.db");
        private static string connectionString = $"Data Source={dbFile};";

        public static void EnsureCoreTablesCreated()
        {
            CreateDatabaseIfNotExists();
            CreateOwnersTableIfNotExists();
            CreateResidentsTableIfNotExists();
            CreateTIENTableIfNotExists();
            CreateNotificationsTableIfNotExists();
            CreateReportsTableIfNotExists();
            CreateCANHOTableIfNotExists();
        }


        public static Owner GetOwnerByPhone(string phone)
        {
            Owner owner = null;
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT OwnerID, Name, Phone, RoomNumber, QueQuan, SoNguoiO, BirthDate FROM Owners WHERE Phone = @phone";
                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@phone", phone);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            owner = new Owner
                            {
                                OwnerID = Convert.ToInt32(reader["OwnerID"]),
                                Name = reader["Name"]?.ToString(),
                                Phone = reader["Phone"]?.ToString(),
                                RoomNumber = reader["RoomNumber"] != DBNull.Value ? Convert.ToInt32(reader["RoomNumber"]) : 0,
                                QueQuan = reader["QueQuan"]?.ToString(),
                                SoNguoiO = reader["SoNguoiO"] != DBNull.Value ? Convert.ToInt32(reader["SoNguoiO"]) : 0,
                                BirthDate = reader["BirthDate"]?.ToString()
                            };
                        }
                    }
                }
            }
            return owner;
        }


        #region Đăng Nhập
        public static void CreateDatabaseIfNotExists()
        {
            if (!File.Exists(dbFile))
            {
                using (File.Create(dbFile)) { }
            }

            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                string usersTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Users (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT NOT NULL UNIQUE,
                        Password TEXT NOT NULL,
                        Role INTEGER NOT NULL
                    );";
                using (var cmd = new SqliteCommand(usersTableQuery, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // SHA256 dùng cho mật khẩu
        public static string ComputeHash(string rawData)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(rawData);
                byte[] hash = sha256.ComputeHash(bytes);
                StringBuilder builder = new StringBuilder();
                foreach (var b in hash)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }


        // Thêm tài khoản mới cho chủ hộ
        public static void InsertUser(string username, string password, int role)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string hashedPassword = ComputeHash(password);
                string insertQuery = "INSERT OR IGNORE INTO Users (Username, Password, Role) VALUES (@username, @password, @role)";

                using (var cmd = new SqliteCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);
                    cmd.Parameters.AddWithValue("@role", role);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqliteException ex)
                    {
                        Console.WriteLine("Lỗi InsertUser: " + ex.Message);
                    }
                }
            }
        }

        public static bool ValidateUser(string username, string password)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string hashedPassword = ComputeHash(password);
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @username AND Password = @password";

                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }


        // Hàm lấy Role người dùng (0 = admin, 1 = user)
        public static int? GetUserRole(string username, string password)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string hashedPassword = ComputeHash(password);
                string query = "SELECT Role FROM Users WHERE Username = @username AND Password = @password";

                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);

                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }
            return null;
        }

        //Đổi mật khẩu
        public static bool ChangeUserPassword(string username, string oldPassword, string newPassword)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                // Kiểm tra mật khẩu hiện tại có đúng không
                if (!ValidateUser(username, oldPassword))
                {
                    return false; // Nếu mật khẩu cũ không khớp, không cập nhật
                }
                string hashedNewPassword = ComputeHash(newPassword);
                string updateQuery = "UPDATE Users SET Password = @newPassword WHERE Username = @username";
                using (var cmd = new SqliteCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@newPassword", hashedNewPassword);
                    cmd.Parameters.AddWithValue("@username", username);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        #endregion

        #region Tien
        // Tạo bảng Money
        public static void CreateTIENTableIfNotExists()
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string TIENTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Money (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        OwnerID INTEGER NOT NULL,
                        RoomRent INTEGER DEFAULT 0,
                        PowerReading INTEGER DEFAULT 0,
                        WaterReading INTEGER DEFAULT 0,
                        FOREIGN KEY (OwnerID) REFERENCES Owners(OwnerID) ON DELETE CASCADE
                    );";
                using (var cmd = new SqliteCommand(TIENTableQuery, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        // Thêm dữ liệu tiền
        public static bool InsertMoneyRecord(int ownerId)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    INSERT INTO Money (OwnerID, RoomRent, PowerReading, WaterReading)
                    VALUES (@ownerId, 0, 0, 0)";
                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ownerId", ownerId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
        // Cập nhật tiền
        public static bool UpdateMoney(Money money)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    UPDATE Money 
                    SET 
                        PowerReading = @power,
                        WaterReading = @water,
                        RoomRent = @rent
                    WHERE ID = @id";
                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@power", money.PowerReading);
                    cmd.Parameters.AddWithValue("@water", money.WaterReading);
                    cmd.Parameters.AddWithValue("@rent", money.RoomRent);
                    cmd.Parameters.AddWithValue("@id", money.ID);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        // Lấy dữ liệu tiền
        public static List<Money> GetMoneyData()
        {
            List<Money> moneyList = new List<Money>();

            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT m.ID, m.OwnerID, o.Name AS TenChuHo, 
                           m.RoomRent, m.PowerReading, m.WaterReading, 
                           o.SoNguoiO 
                    FROM Money m
                    JOIN Owners o ON m.OwnerID = o.OwnerID";
                using (var cmd = new SqliteCommand(query, conn))
                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        moneyList.Add(new Money
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            OwnerID = Convert.ToInt32(reader["OwnerID"]),
                            TenChuHo = reader["TenChuHo"].ToString(),
                            RoomRent = Convert.ToInt32(reader["RoomRent"]),
                            PowerReading = Convert.ToInt32(reader["PowerReading"]),
                            WaterReading = Convert.ToInt32(reader["WaterReading"]),
                            SoNguoiO = Convert.ToInt32(reader["SoNguoiO"])
                        });
                    }
                }
            }
            return moneyList;
        }
        // Lấy dữ liệu tiền theo ID
        public static Money GetMoneyByOwnerId(int ownerId)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT m.ID, m.OwnerID, m.RoomRent, m.PowerReading, m.WaterReading,
                           o.Name AS TenChuHo, o.SoNguoiO
                    FROM Money m
                    JOIN Owners o ON m.OwnerID = o.OwnerID
                    WHERE m.OwnerID = @ownerId";

                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ownerId", ownerId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Money
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                OwnerID = Convert.ToInt32(reader["OwnerID"]),
                                RoomRent = Convert.ToInt32(reader["RoomRent"]),
                                PowerReading = Convert.ToInt32(reader["PowerReading"]),
                                WaterReading = Convert.ToInt32(reader["WaterReading"]),
                                SoNguoiO = Convert.ToInt32(reader["SoNguoiO"]),
                                TenChuHo = reader["TenChuHo"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }
        #endregion

        #region Căn Hộ
        public static void CreateCANHOTableIfNotExists()
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string TIENTableQuery = @"
                    CREATE TABLE IF NOT EXISTS CanHo (
                        MaCanHo TEXT PRIMARY KEY, 
                        LoaiPhong TEXT NOT NULL,    
                        GiaPhong INTEGER NOT NULL,  
                        DienTich TEXT NOT NULL,     
                        Controng TEXT NOT NULL      
                    );";
                using (var cmd = new SqliteCommand(TIENTableQuery, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static List<CanHo> GetAllCanHo()
        {
            var result = new List<CanHo>();
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT MaCanHo, LoaiPhong, GiaPhong, DienTich, Controng FROM CanHo";
                using (var cmd = new SqliteCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var owner = new CanHo
                            {
                                MaCanHo = reader["MaCanHo"].ToString(),
                                LoaiPhong = reader["LoaiPhong"].ToString(),
                                GiaPhong = Convert.ToInt32(reader["GiaPhong"]),
                                DienTich = reader["DienTich"].ToString(),
                                Controng = reader["Controng"].ToString()
                            };
                            result.Add(owner);
                        }
                    }
                }
            }
            return result;
        }

        #endregion

        #region Thông Báo
        // Tạo bảng Notifications nếu chưa tồn tại
        public static void CreateNotificationsTableIfNotExists()
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string query = @"
            CREATE TABLE IF NOT EXISTS Notifications (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                Content TEXT NOT NULL,
                Date TEXT NOT NULL
            );";
                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Hàm thêm thông báo mới vào bảng Notifications
        public static bool InsertNotification(string title, string content, string date)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Notifications (Title, Content, Date) VALUES (@title, @content, @date)";
                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@content", content);
                    cmd.Parameters.AddWithValue("@date", date);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        // Hàm cập nhật thông báo dựa theo ID
        public static bool UpdateNotification(int id, string title, string content, string date)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Notifications SET Title = @title, Content = @content, Date = @date WHERE ID = @id";
                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@content", content);
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.Parameters.AddWithValue("@id", id);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        // Hàm lấy danh sách thông báo từ bảng Notifications
        public static List<Notification> GetNotifications()
        {
            var notifications = new List<Notification>();
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ID, Title, Content, Date FROM Notifications ORDER BY ID DESC";
                using (var cmd = new SqliteCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            notifications.Add(new Notification
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Title = reader["Title"].ToString(),
                                Content = reader["Content"].ToString(),
                                Date = reader["Date"].ToString()
                            });
                        }
                    }
                }
            }
            return notifications;
        }
        #endregion

        #region BÁO CÁO/KHIẾU NẠI
        public static void CreateReportsTableIfNotExists()
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string query = @"
            CREATE TABLE IF NOT EXISTS Reports (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                Content TEXT NOT NULL,
                Date TEXT NOT NULL,
                OwnerID INTEGER,
                FOREIGN KEY (OwnerID) REFERENCES Owners(OwnerID)
            );";
                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Thêm khiếu nại mới
        public static bool InsertReport(string title, string content, string date, int ownerId)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string query = @"
            INSERT INTO Reports (Title, Content, Date, OwnerID)
            VALUES (@title, @content, @date, @ownerId)";
                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@content", content);
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.Parameters.AddWithValue("@ownerId", ownerId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // Lấy tất cả khiếu nại
        public static List<Report> GetAllReports()
        {
            var reports = new List<Report>();
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ID, Title, Content, Date, OwnerID FROM Reports ORDER BY ID DESC";
                using (var cmd = new SqliteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reports.Add(new Report
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Title = reader["Title"].ToString(),
                            Content = reader["Content"].ToString(),
                            Date = reader["Date"].ToString(),
                            OwnerID = Convert.ToInt32(reader["OwnerID"])
                        });
                    }
                }
            }
            return reports;
        }
        // Lấy tất cả báo cáo kèm thông tin chủ hộ
        public static List<ReportWithOwnerInfo> GetAllReportsWithOwnerInfo()
        {
            var reports = new List<ReportWithOwnerInfo>();
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string query = @"
            SELECT r.ID, r.Title, r.Content, r.Date, r.OwnerID,
                   o.Name AS OwnerName, o.Phone AS OwnerPhone, o.RoomNumber AS OwnerRoom
            FROM Reports r
            LEFT JOIN Owners o ON r.OwnerID = o.OwnerID
            ORDER BY r.ID DESC";

                using (var cmd = new SqliteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reports.Add(new ReportWithOwnerInfo
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Title = reader["Title"].ToString(),
                            Content = reader["Content"].ToString(),
                            Date = reader["Date"].ToString(),
                            OwnerID = Convert.ToInt32(reader["OwnerID"]),
                            OwnerName = reader["OwnerName"].ToString(),
                            OwnerPhone = reader["OwnerPhone"].ToString(),
                            OwnerRoom = reader["OwnerRoom"] != DBNull.Value ? Convert.ToInt32(reader["OwnerRoom"]) : 0
                        });
                    }
                }
            }
            return reports;
        }
        #endregion

        #region CHỦ HỘ
        // Tạo bảng Owners (Chủ hộ) với cột BirthDate được thêm vào
        public static void CreateOwnersTableIfNotExists()
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string ownersTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Owners (
                        OwnerID INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Phone TEXT,
                        RoomNumber INTEGER, 
                        QueQuan TEXT,
                        SoNguoiO INTEGER DEFAULT 0,
                        BirthDate TEXT  
                    );";
                using (var cmd = new SqliteCommand(ownersTableQuery, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Thêm chủ hộ mới
        public static bool InsertOwner(Owner owner)
        {
            EnsureCoreTablesCreated();

            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string insertQuery = "INSERT INTO Owners (Name, Phone, RoomNumber, QueQuan, BirthDate) VALUES (@name, @phone, @roomNumber, @quequan, @birthDate)";
                using (var cmd = new SqliteCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@name", owner.Name);
                    cmd.Parameters.AddWithValue("@phone", owner.Phone);
                    cmd.Parameters.AddWithValue("@roomNumber", owner.RoomNumber);
                    cmd.Parameters.AddWithValue("@quequan", owner.QueQuan);
                    cmd.Parameters.AddWithValue("@birthDate", owner.BirthDate);
                    try
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            cmd.CommandText = "SELECT last_insert_rowid()";
                            int newOwnerId = Convert.ToInt32(cmd.ExecuteScalar());
                            InsertMoneyRecord(newOwnerId);
                            InsertUser(owner.Phone, "1", 0);
                            return true;
                        }
                        return false;
                    }
                    catch (SqliteException ex)
                    {
                        Console.WriteLine("Lỗi InsertOwner: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        // Thêm cư dân mới
        public static bool InsertResident(Resident resident)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string insertQuery = @"
                    INSERT INTO Residents (Name, Phone, BirthDate, QueQuan, OwnerID)
                    VALUES (@name, @phone, @birthDate, @quequan, @ownerID)";
                using (var cmd = new SqliteCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@name", resident.Name);
                    cmd.Parameters.AddWithValue("@phone", resident.Phone);
                    cmd.Parameters.AddWithValue("@birthDate", resident.BirthDate);
                    cmd.Parameters.AddWithValue("@quequan", resident.QueQuan);
                    cmd.Parameters.AddWithValue("@ownerID", resident.OwnerID);
                    try
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            int newCount = GetResidentCount(resident.OwnerID);
                            UpdateOwnerResidentCount(resident.OwnerID, newCount);
                            return true;
                        }
                    }
                    catch (SqliteException ex)
                    {
                        Console.WriteLine("Lỗi InsertResident: " + ex.Message);
                    }
                }
            }
            return false;
        }


        // Xóa chủ hộ
        public static bool DeleteOwner(int ownerId)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string deleteQuery = "DELETE FROM Owners WHERE OwnerID = @ownerId";
                using (var cmd = new SqliteCommand(deleteQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ownerId", ownerId);
                    try
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (SqliteException ex)
                    {
                        Console.WriteLine("Lỗi DeleteOwner: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        // Cập nhật thông tin chủ hộ
        public static bool UpdateOwner(Owner owner)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string updateQuery = @"
                    UPDATE Owners 
                    SET Name = @name, 
                        Phone = @phone, 
                        RoomNumber = @roomNumber, 
                        QueQuan = @quequan, 
                        BirthDate = @birthDate
                    WHERE OwnerID = @ownerId";
                using (var cmd = new SqliteCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@name", owner.Name);
                    cmd.Parameters.AddWithValue("@phone", owner.Phone);
                    cmd.Parameters.AddWithValue("@roomNumber", owner.RoomNumber);
                    cmd.Parameters.AddWithValue("@quequan", owner.QueQuan);
                    cmd.Parameters.AddWithValue("@birthDate", owner.BirthDate);
                    cmd.Parameters.AddWithValue("@ownerId", owner.OwnerID);
                    try
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (SqliteException ex)
                    {
                        Console.WriteLine("Lỗi UpdateOwner: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        //kiểm tra số điện thoại đã tồn tại chưa
        public static bool IsOwnerPhoneExists(string phone)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Owners WHERE Phone = @phone";
                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@phone", phone);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        //kiểm tra số phòng đã tồn tại chưa
        public static bool IsRoomNumberExists(int roomNumber)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Owners WHERE RoomNumber = @roomNumber";
                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@roomNumber", roomNumber);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }


        // Lấy danh sách tất cả chủ hộ
        public static List<Owner> GetAllOwners()
        {
            var result = new List<Owner>();
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT OwnerID, Name, Phone, RoomNumber, QueQuan, SoNguoiO, BirthDate FROM Owners";
                using (var cmd = new SqliteCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var owner = new Owner
                            {
                                OwnerID = Convert.ToInt32(reader["OwnerID"]),
                                Name = reader["Name"]?.ToString(),
                                Phone = reader["Phone"]?.ToString(),
                                RoomNumber = reader["RoomNumber"] != DBNull.Value ? Convert.ToInt32(reader["RoomNumber"]) : 0,
                                QueQuan = reader["QueQuan"]?.ToString(),
                                SoNguoiO = reader["SoNguoiO"] != DBNull.Value ? Convert.ToInt32(reader["SoNguoiO"]) : 0,
                                BirthDate = reader["BirthDate"]?.ToString()
                            };
                            result.Add(owner);
                        }
                    }
                }
            }
            return result;
        }

        // Lấy thông tin chi tiết của một chủ hộ, kèm theo danh sách cư dân và cập nhật SoNguoiO theo số cư dân
        public static Owner GetOwnerDetails(int ownerId)
        {
            Owner owner = null;
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                // Lấy thông tin chủ hộ
                string ownerQuery = "SELECT OwnerID, Name, Phone, RoomNumber, QueQuan, BirthDate FROM Owners WHERE OwnerID = @ownerId";
                using (var cmd = new SqliteCommand(ownerQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ownerId", ownerId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            owner = new Owner
                            {
                                OwnerID = Convert.ToInt32(reader["OwnerID"]),
                                Name = reader["Name"]?.ToString(),
                                Phone = reader["Phone"]?.ToString(),
                                RoomNumber = reader["RoomNumber"] != DBNull.Value ? Convert.ToInt32(reader["RoomNumber"]) : 0,
                                QueQuan = reader["QueQuan"]?.ToString(),
                                BirthDate = reader["BirthDate"]?.ToString()
                            };
                        }
                    }
                }

                if (owner != null)
                {
                    // Lấy danh sách cư dân cho chủ hộ
                    string residentsQuery = "SELECT ResidentID, Name, Phone, BirthDate, QueQuan, OwnerID FROM Residents WHERE OwnerID = @ownerId";
                    using (var cmd = new SqliteCommand(residentsQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@ownerId", ownerId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var resident = new Resident
                                {
                                    ResidentID = Convert.ToInt32(reader["ResidentID"]),
                                    Name = reader["Name"]?.ToString(),
                                    Phone = reader["Phone"]?.ToString(),
                                    BirthDate = reader["BirthDate"]?.ToString(),
                                    QueQuan = reader["QueQuan"]?.ToString(),
                                    OwnerID = Convert.ToInt32(reader["OwnerID"])
                                };
                                owner.Residents.Add(resident);
                            }
                        }
                    }
                    // Cập nhật số cư dân đã load (nếu bạn sử dụng cột SoNguoiO lưu trong DB)
                    owner.SoNguoiO = owner.Residents.Count;
                }
            }
            return owner;
        }


        #endregion

        #region CƯ DÂN
        // Tạo bảng Residents (Cư dân) mà bỏ cột Relation


        public static int GetResidentCount(int ownerId)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Residents WHERE OwnerID = @ownerId";
                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ownerId", ownerId);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public static void CreateResidentsTableIfNotExists()
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string residentsTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Residents (
                        ResidentID INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Phone TEXT,
                        BirthDate TEXT,
                        QueQuan TEXT,
                        OwnerID INTEGER,
                        FOREIGN KEY (OwnerID) REFERENCES Owners(OwnerID) ON DELETE CASCADE
                    );";
                using (var cmd = new SqliteCommand(residentsTableQuery, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        //xóa cư dân
        public static bool DeleteResident(int residentId)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Residents WHERE ResidentID = @residentId";
                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@residentId", residentId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
        //sửa cư dân
        public static bool UpdateResident(Resident resident)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    UPDATE Residents 
                    SET Name = @name, 
                        Phone = @phone, 
                        BirthDate = @birthDate, 
                        QueQuan = @queQuan 
                    WHERE ResidentID = @residentId";
                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", resident.Name);
                    cmd.Parameters.AddWithValue("@phone", resident.Phone);
                    cmd.Parameters.AddWithValue("@birthDate", resident.BirthDate);
                    cmd.Parameters.AddWithValue("@queQuan", resident.QueQuan);
                    cmd.Parameters.AddWithValue("@residentId", resident.ResidentID);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
        //cập nhật lại số người ở cho chủ hộ
        public static void UpdateOwnerResidentCount(int ownerId, int count)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string updateQuery = "UPDATE Owners SET SoNguoiO = @count WHERE OwnerID = @ownerId";
                using (var cmd = new SqliteCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@count", count);
                    cmd.Parameters.AddWithValue("@ownerId", ownerId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        //tính số cư dân
        public static List<Resident> GetResidentsByOwnerID(int ownerId)
        {
            var residents = new List<Resident>();
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ResidentID, Name, Phone, BirthDate, QueQuan, OwnerID FROM Residents WHERE OwnerID = @ownerId";
                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ownerId", ownerId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            residents.Add(new Resident
                            {
                                ResidentID = Convert.ToInt32(reader["ResidentID"]),
                                Name = reader["Name"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                BirthDate = reader["BirthDate"].ToString(),
                                QueQuan = reader["QueQuan"].ToString(),
                                OwnerID = Convert.ToInt32(reader["OwnerID"])
                            });
                        }
                    }
                }
            }
            return residents;
        }
        #endregion

    }

    #region Class
    // Lớp đại diện cho Chủ hộ
    public class Owner
    {
        public int STT { get; set; }
        public int OwnerID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int RoomNumber { get; set; }
        public string QueQuan { get; set; }
        public int SoNguoiO { get; set; }
        public string BirthDate { get; set; }
        public List<Resident> Residents { get; set; } = new List<Resident>();
        public int TotalPeople => (Residents != null ? Residents.Count : 0) + 1;

    }

    // Lớp đại diện cho Cư dân
    public class Resident : INotifyPropertyChanged
    {
        private int _stt;
        public int STT
        {
            get { return _stt; }
            set
            {
                _stt = value;
                OnPropertyChanged(nameof(STT));
            }
        }

        public int ResidentID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string BirthDate { get; set; }
        public string QueQuan { get; set; }
        public int OwnerID { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    // Lớp đại diện cho Tiền
    public class Money
    {
        public int ID { get; set; }
        public int OwnerID { get; set; }
        public string TenChuHo { get; set; }
        public int RoomRent { get; set; }
        public int PowerReading { get; set; }
        public int WaterReading { get; set; }
        public int SoNguoiO { get; set; }
        public int STT { get; set; }

        public int TienDien => PowerReading * 3500;
        public int TienNuoc => WaterReading * 30000;
        public int TienDichVu => (SoNguoiO + 1) * 50000;
        public int TongTien => TienDien + TienNuoc + TienDichVu + RoomRent;
    }

    public class CanHo
    {
        public string MaCanHo { get; set; }
        public string LoaiPhong { get; set; }
        public int GiaPhong { get; set; }
        public string DienTich { get; set; }
        public string Controng { get; set; }
    }

    public class Notification
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Date { get; set; }
    }

    public class Report
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Date { get; set; }
        public int OwnerID { get; set; }
    }

    public class ReportWithOwnerInfo : Report
    {
        public string OwnerName { get; set; }
        public string OwnerPhone { get; set; }
        public int OwnerRoom { get; set; }
    }
    #endregion
}