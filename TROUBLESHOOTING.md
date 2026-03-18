# Hướng Dẫn Xử Lý Sự Cố (Troubleshooting)

## 🆘 Tài Liệu Này Là Gì?

Nếu bạn gặp lỗi khi cài đặt hoặc chạy test, tài liệu này cung cấp **các giải pháp thực tế** cho những sự cố phổ biến nhất.

---

## 🔴 LỖI CÀI ĐẶT

### 1. "Command 'dotnet' is not recognized"

**Triệu Chứng:**
```powershell
dotnet --version
# Output: 'dotnet' is not recognized as an internal or external command
```

**Nguyên Nhân:**
- .NET SDK chưa được cài đặt
- .NET SDK cài nhưng chưa được reload PowerShell
- PATH environment variable chưa được update

**Cách Fix:**

**Bước 1: Kiểm tra cài đặt**
```powershell
# Mở Settings → Apps → Installed apps
# Tìm ".NET Desktop Runtime" hoặc ".NET SDK"

# Hoặc dùng PowerShell
Get-Package | Select-String -Pattern "dotnet|.NET"
```

**Bước 2: Nếu chưa cài, cài ngay**
- Tải từ https://dotnet.microsoft.com/download
- Chọn **.NET SDK 8**
- Chạy installer

**Bước 3: Reload PowerShell**
```powershell
# Đóng PowerShell hiện tại
# Mở PowerShell mới

# Kiểm tra
dotnet --version
# Expected: 8.0.x
```

---

### 2. "WinAppDriver.exe is not found"

**Triệu Chứng:**
```powershell
WinAppDriver.exe 127.0.0.1 4723
# Output: 'WinAppDriver.exe' is not recognized
```

**Nguyên Nhân:**
- WinAppDriver chưa cài đặt
- WinAppDriver cài nhưng chưa được thêm vào PATH

**Cách Fix:**

**Bước 1: Kiểm tra cài đặt**
```powershell
# Tìm trong Program Files
Get-ChildItem "C:\Program Files\WindowsAppDriver" -ErrorAction SilentlyContinue

# Hoặc
Get-ChildItem "C:\Program Files (x86)\Windows App Driver" -ErrorAction SilentlyContinue
```

**Bước 2: Nếu chưa cài, cài từ GitHub**
1. Truy cập: https://github.com/microsoft/WinAppDriver/releases
2. Tải bản mới nhất (.exe installer)
3. Chạy installer

**Bước 3: Thêm vào PATH (nếu cần)**

Nếu WinAppDriver cài nhưng không trong PATH:

```powershell
# Tìm thư mục cài đặt WinAppDriver
$winappdriver_path = "C:\Program Files\WindowsAppDriver"

# Thêm vào PATH (tạm thời cho session hiện tại)
$env:Path += ";$winappdriver_path"

# Kiểm tra
WinAppDriver.exe --help
```

**Bước 4: Thêm vào PATH Vĩnh Viễn (tuỳ chọn)**

Để WinAppDriver.exe chạy từ bất cứ đâu:

1. Mở **System Properties** (right-click "This PC" → Properties)
2. Click **Advanced system settings**
3. Click **Environment Variables**
4. Click **New** (under System variables)
5. Variable name: `Path`
6. Variable value: `C:\Program Files\WindowsAppDriver`
7. Click OK, OK, OK
8. Restart PowerShell

---

### 3. "Git not found"

**Triệu Chứng:**
```powershell
git --version
# 'git' is not recognized
```

**Cách Fix:**
1. Tải từ https://git-scm.com/
2. Chạy installer với default settings
3. Restart PowerShell
4. Kiểm tra: `git --version`

---

## 🔴 LỖI Clone DỰ ÁN

### 4. "Repository not found"

**Triệu Chứng:**
```powershell
git clone https://github.com/YourUsername/QuanLyChungCu_Final.git
# Error: repository not found (HTTP 404)
```

**Nguyên Nhân:**
- URL repository sai
- Repository chưa được push lên GitHub
- Repository là private và không có quyền truy cập

**Cách Fix:**

**Bước 1: Kiểm tra URL**
```powershell
# Đúng format
git clone https://github.com/YourUsername/QuanLyChungCu_Final.git

# Hoặc SSH
git clone git@github.com:YourUsername/QuanLyChungCu_Final.git
```

**Bước 2: Kiểm tra repository tồn tại**
- Truy cập https://github.com/YourUsername/QuanLyChungCu_Final
- Nếu 404, repository chưa tồn tại
- Tạo repository trên GitHub trước

**Bước 3: Kiểm tra quyền truy cập**
```powershell
# Nếu là private repository
git clone https://YourUsername:YourToken@github.com/YourUsername/QuanLyChungCu_Final.git

# Hoặc setup SSH keys
ssh-keygen -t rsa -b 4096 -f ~/.ssh/id_rsa
# Add public key vào GitHub Settings → SSH Keys
```

---

### 5. "Connection timeout" khi clone

**Triệu Chứng:**
```powershell
Cloning into 'QuanLyChungCu_Final'...
fatal: unable to access 'https://github.com/...' Connection timed out
```

**Nguyên Nhân:**
- Mạng không ổn định
- GitHub server bị lỗi
- Firewall chặn kết nối

**Cách Fix:**

```powershell
# 1. Kiểm tra kết nối mạng
ping github.com

# 2. Nếu có firewall, cho phép kết nối
# (Hỏi Admin IT nếu ở công ty)

# 3. Retry clone
git clone https://github.com/YourUsername/QuanLyChungCu_Final.git

# 4. Nếu vẫn timeout, clone từ zip
# - Vào GitHub
# - Click "Code" → "Download ZIP"
# - Extract zip về máy
```

---

## 🔴 LỖI BUILD

### 6. "Project file not found"

**Triệu Chứng:**
```powershell
dotnet build .\QuanLyChungCu\QuanLyChungCu.csproj
# Error: NETSDK1004: Assets file '.../obj/project.assets.json' not found
```

**Nguyên Nhân:**
- NuGet packages chưa restore
- File .csproj corrupted

**Cách Fix:**

```powershell
# Bước 1: Restore packages
cd D:\Projects\QuanLyChungCu_Final
dotnet restore

# Bước 2: Clean build
dotnet clean
dotnet build .\QuanLyChungCu\QuanLyChungCu.csproj -c Release
```

---

### 7. "Cannot find assembly reference"

**Triệu Chứng:**
```
error CS0246: The type or namespace name 'Appium' could not be found
```

**Nguyên Nhân:**
- NuGet packages chưa restore
- packages.config hoặc .csproj file không đúng

**Cách Fix:**

```powershell
# Restore all packages
dotnet restore .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj

# Rebuild
dotnet clean
dotnet build .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj -c Release
```

---

## 🔴 LỖI CHẠY TEST

### 8. "Unable to connect to http://127.0.0.1:4723/"

**Triệu Chứng:**
```
OpenQA.Selenium.DriverServiceTransportException: 
Unable to connect to http://127.0.0.1:4723/
Connection refused
```

**Nguyên Nhân:**
- WinAppDriver service chưa chạy
- Port 4723 bị chiếm bởi process khác
- WinAppDriver crash

**Cách Fix:**

**Bước 1: Kiểm tra WinAppDriver có chạy**
```powershell
# Xem process
Get-Process | findstr "WinAppDriver"

# Nếu không có, khởi động
WinAppDriver.exe 127.0.0.1 4723
```

**Bước 2: Nếu port bị chiếm**
```powershell
# Tìm process chiếm port 4723
netstat -ano | findstr :4723
# Output: TCP ... 127.0.0.1:4723 ... LISTENING <PID>

# Kill process
taskkill /PID <PID> /F

# Hoặc dùng lệnh đơn giản
Get-Process | Where-Object {$_.Id -eq <PID>} | Stop-Process -Force
```

**Bước 3: Khởi động lại WinAppDriver**
```powershell
# Nếu WinAppDriver vẫn running, stop it
Get-Process WinAppDriver | Stop-Process -Force

# Khởi động lại
WinAppDriver.exe 127.0.0.1 4723
```

**Bước 4: Kiểm tra cấu hình URL**
```powershell
# Nếu WinAppDriver chạy trên port khác, cập nhật biến môi trường
$env:WINAPPDRIVER_URL = "http://127.0.0.1:4724/"  # nếu port khác

# Hoặc check file TestConfig.cs
```

---

### 9. "QuanLyChungCu.exe is locked"

**Triệu Chứng:**
```
System.IO.IOException: The file 'QuanLyChungCu.exe' cannot be opened 
because it is being used by another process.
```

**Nguyên Nhân:**
- App vẫn chạy từ test trước
- Process QuanLyChungCu.exe không tắt hoàn toàn
- WinAppDriver session không close

**Cách Fix:**

```powershell
# Bước 1: Kill toàn bộ process QuanLyChungCu
taskkill /IM QuanLyChungCu.exe /F
taskkill /IM QuanLyChungCu.exe /F  # Chạy lần 2 để chắc

# Bước 2: Kiểm tra process còn không
Get-Process | findstr "QuanLyChungCu"
# Không output = ok

# Bước 3: Nếu vẫn lock, check WinAppDriver session
Get-Process WinAppDriver

# Bước 4: Chạy test lại
dotnet test ...
```

---

### 10. "RUN_WINAPPDRIVER_TESTS not set"

**Triệu Chứng:**
```
Test is skipped because environment variable 'RUN_WINAPPDRIVER_TESTS' is not 'true'
```

**Nguyên Nhân:**
- Quên bật biến môi trường

**Cách Fix:**

```powershell
# Bật biến
$env:RUN_WINAPPDRIVER_TESTS = "true"

# Kiểm tra
$env:RUN_WINAPPDRIVER_TESTS  # Output: true

# Chạy test
dotnet test ...
```

---

## 🔴 LỖI ELEMENT & LOCATOR

### 11. "NoSuchElementException: Could not locate element"

**Triệu Chứng:**
```
OpenQA.Selenium.NoSuchElementException: 
Could not locate element with name: "btnAddOwner" or AutomationId: "btnAddOwner"
```

**Nguyên Nhân:**
- Locator (tên/ID) element sai
- Element không tồn tại trong UI
- UI app thay đổi
- Timeout quá ngắn

**Cách Fix:**

**Bước 1: Xác nhận locator**
```powershell
# Mở ứng dụng QuanLyChungCu
.\QuanLyChungCu\bin\Release\net8.0-windows\QuanLyChungCu.exe

# Xem tên button (right-click → Inspect hoặc dùng Inspect.exe)
# So sánh với locator trong test code
```

**Bước 2: Update locator trong code**
```csharp
// File: Pages/OwnerManagementPage.cs

// OLD (SAI)
var addButton = AppDriver.FindElementByName("btnAddOwner");

// NEW (ĐÚNG) - kiểm tra lại tên trong app
var addButton = AppDriver.FindElementByName("btnAdd");
// Hoặc
var addButton = AppDriver.FindElementByAutomationId("AddButton");
```

**Bước 3: Tăng timeout nếu element load lâu**
```csharp
// Đợi element xuất hiện lâu hơn
var wait = new WebDriverWait(AppDriver, TimeSpan.FromSeconds(15));
var element = wait.Until(EC.ElementToBeClickable(By.Name("btnAdd")));
```

**Bước 4: Dump element tree để debug**
```powershell
# Chạy test với debug output
dotnet test ... -v detailed

# Xem console output tìm dòng:
# [Debug] Element tree snapshot:
#   - Window: "QuanLyChungCu"
#   - Button: "btnEdit"
#   - Button: "btnDelete"
```

---

### 12. "StaleElementReferenceException"

**Triệu Chứng:**
```
OpenQA.Selenium.StaleElementReferenceException: 
Stale element reference
```

**Nguyên Nhân:**
- Element bị reload/refresh sau khi find
- DOM thay đổi giữa find và action

**Cách Fix:**

```csharp
// OLD (SAI)
var button = AppDriver.FindElementByName("btnAdd");
// ... UI refresh ...
button.Click();  // Element đã cũ (stale)

// NEW (ĐÚNG) - Tìm lại element trước khi action
var button = AppDriver.FindElementByName("btnAdd");
AppDriver.FindElementByName("btnAdd").Click();  // Tìm lại

// Hoặc dùng helper method
public void ClickButtonRetry(string buttonName, int retries = 3)
{
    for (int i = 0; i < retries; i++)
    {
        try
        {
            AppDriver.FindElementByName(buttonName).Click();
            return;
        }
        catch (StaleElementReferenceException)
        {
            Thread.Sleep(100);
        }
    }
}
```

---

### 13. "TimeoutException: Timed out"

**Triệu Chứng:**
```
OpenQA.Selenium.WebDriverTimeoutException: 
Timeout after 10000ms waiting for element
```

**Nguyên Nhân:**
- Element load quá lâu
- Timeout setting quá ngắn
- App hang/crash
- Network chậm

**Cách Fix:**

**Bước 1: Tăng timeout**
```powershell
# Tạm thời (session hiện tại)
$env:WINAPPDRIVER_TIMEOUT_MS = "20000"  # 20 giây

# Hoặc trong code (TestConfig.cs)
public const int DefaultWaitTimeoutMs = 20000;
```

**Bước 2: Kiểm tra app performance**
```powershell
# Xem CPU/RAM usage
Get-Process QuanLyChungCu | Select Handles, CPU, Memory

# Nếu CPU/RAM cao, app lag
# Optimize app hoặc close other apps
```

**Bước 3: Kiểm tra network (nếu app dùng database server)**
```powershell
# Test kết nối database
ping <database-server>

# Hoặc check internet speed
```

**Bước 4: Retry element find**
```csharp
// Với retry logic
public IWebElement FindElementWithRetry(By locator, int maxRetries = 3)
{
    for (int i = 0; i < maxRetries; i++)
    {
        try
        {
            var wait = new WebDriverWait(AppDriver, TimeSpan.FromSeconds(10));
            return wait.Until(EC.PresenceOfElementLocated(locator));
        }
        catch (TimeoutException) when (i < maxRetries - 1)
        {
            Thread.Sleep(500);
        }
    }
    throw new TimeoutException($"Element not found after {maxRetries} retries");
}
```

---

## 🔴 LỖI TEST LOGIC

### 14. "AssertionFailedError: Expected vs Actual"

**Triệu Chứng:**
```
AssertionFailedError: Expected: true, Actual: false
```

**Nguyên Nhân:**
- Assertion logic sai
- Dữ liệu test không đúng
- App behavior khác kỳ vọng

**Cách Fix:**

```csharp
// OLD (SAI)
Assert.IsTrue(actualText == expectedText);

// NEW (ĐÚNG) - Check dữ liệu trước
Console.WriteLine($"Expected: {expectedText}");
Console.WriteLine($"Actual: {actualText}");
Assert.AreEqual(expectedText, actualText, 
                $"Text mismatch: expected '{expectedText}' but got '{actualText}'");
```

---

### 15. "Test passes locally but fails in CI/CD"

**Nguyên Nhân:**
- Environment khác
- Database state khác
- App version khác
- Timing issue (test flaky)

**Cách Fix:**

```powershell
# Bước 1: Chạy lặp nhiều lần để kiểm tra flaky
for ($i=1; $i -le 10; $i++) {
    dotnet test ...
}

# Bước 2: Thêm stabilization wait
Thread.Sleep(500);  // Chờ UI settle

# Bước 3: Seed dữ liệu rõ ràng
# Bước 4: Xóa dữ liệu cũ trước test
# Bước 5: Kiểm tra environment variables
```

---

## 🟡 LỖI PERFORMANCE

### 16. Test chạy rất chậm

**Nguyên Nhân:**
- Timeout setting quá cao
- App hang
- Machine low resources
- WinAppDriver overhead

**Cách Fix:**

```powershell
# Bước 1: Giảm timeout nếu đã tăng
$env:WINAPPDRIVER_TIMEOUT_MS = "10000"  # Trở về mặc định

# Bước 2: Kiểm tra machine resources
Get-Process | Sort-Object Memory | Select-Object -Last 10 ProcessName, @{n='Memory(MB)'; e={[int]($_.Memory/1MB)}}

# Bước 3: Close unnecessary apps
# Bước 4: Restart WinAppDriver
taskkill /IM WinAppDriver.exe /F
WinAppDriver.exe 127.0.0.1 4723
```

---

## 🟡 LỖI DATABASE

### 17. "Database locked" hoặc "File already in use"

**Triệu Chứng:**
```
SQLite.SQLiteException: database is locked
```

**Nguyên Nhân:**
- App còn lock database
- Test không close connection
- File .sqlite được open bởi tool khác

**Cách Fix:**

```powershell
# Bước 1: Đóng app
taskkill /IM QuanLyChungCu.exe /F

# Bước 2: Đóng SQL tool (SQLite Browser, DBeaver, v.v.)
# Bước 3: Chạy test lại
```

---

## 🟠 LỖI WINAPPDDRIVER SESSION

### 18. "Session not created"

**Triệu Chứng:**
```
OpenQA.Selenium.WebDriverException: 
A new session could not be created. Details: 
Session ID is null. Driver info: ...
```

**Nguyên Nhân:**
- App crash khi start
- App path sai
- WinAppDriver service not ready

**Cách Fix:**

```powershell
# Bước 1: Xác nhận app path
Test-Path .\QuanLyChungCu\bin\Release\net8.0-windows\QuanLyChungCu.exe

# Bước 2: Thử start app thủ công
.\QuanLyChungCu\bin\Release\net8.0-windows\QuanLyChungCu.exe

# Bước 3: Nếu crash, xem Event Viewer
# Start → Event Viewer → Windows Logs → Application

# Bước 4: Restart WinAppDriver
taskkill /IM WinAppDriver.exe /F
WinAppDriver.exe 127.0.0.1 4723
```

---

## ✅ CHECKLIST XỬ LỸ SỰ CỐ

Khi gặp lỗi, hãy check theo thứ tự:

```
□ 1. Đọc thông báo lỗi từ console carefully
□ 2. Copy error message vào Google/StackOverflow
□ 3. Kiểm tra biến môi trường ($env:...)
□ 4. Kiểm tra WinAppDriver service (Get-Process, port 4723)
□ 5. Kiểm tra QuanLyChungCu.exe không lock
□ 6. Kiểm tra locator element (Name, AutomationId)
□ 7. Tăng timeout nếu timeout
□ 8. Kill process, restart, chạy lại
□ 9. Nếu vẫn fail, enable verbose logging
□ 10. Ghi lại error detail, tạo Issue trên GitHub
```

---

## 📞 CẦN THÊM GIÚP?

Nếu sau khi thử tất cả còn fail:

1. **Ghi lại thông tin:**
   - Error message (đầy đủ)
   - Console output (screenshot)
   - File báo cáo TRX
   - Bước để reproduce lỗi

2. **Tạo Issue trên GitHub:**
   - Title: `[BUG] Error message tóm tắt`
   - Description: Chi tiết issue + cách reproduce
   - Labels: `bug`, `help-needed`

3. **Liên hệ Team:**
   - Email: [your-email@example.com]
   - Slack: [your-channel]

---

**Good luck! Bạn sẽ khắc phục được! 💪**

