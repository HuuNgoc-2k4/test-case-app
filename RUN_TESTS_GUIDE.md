# Hướng Dẫn Chi Tiết Chạy Test - Từng Bước

## Tóm Tắt Nhanh

| Bước | Hành Động | Lệnh |
|------|----------|------|
| 1 | Build app | `dotnet build .\QuanLyChungCu\QuanLyChungCu.csproj` |
| 2 | Start WinAppDriver | `WinAppDriver.exe 127.0.0.1 4723` (terminal riêng) |
| 3 | Enable UI Tests | `$env:RUN_WINAPPDRIVER_TESTS = "true"` |
| 4 | Chạy test | `.\run-ui-tests.ps1` |
| 5 | Xem báo cáo | `TestResults/ui-tests.trx` |

---

## Chi Tiết Từng Bước

### Bước 1: Build Ứng Dụng

**Mục tiêu**: Tạo file `QuanLyChungCu.exe` cần thiết để test

```powershell
# Mở PowerShell hoặc Command Prompt
cd d:\Code\KiemThu\QuanLyChungCu_Final

# Build với cấu hình Debug
dotnet build .\QuanLyChungCu\QuanLyChungCu.csproj -c Debug

# Output mong đợi:
# Build succeeded. (1.2s)
```

**Nếu lỗi**:
- Kiểm tra .NET SDK: `dotnet --version` (phải ≥ 8.0)
- Xóa folder: `rm -r .\bin .\obj`
- Build lại

**File được tạo**:
```
QuanLyChungCu/bin/Debug/net8.0-windows/QuanLyChungCu.exe  ← File này sẽ được test
```

---

### Bước 2: Khởi Động WinAppDriver

**Mục tiêu**: Bắt đầu server WinAppDriver để điều khiển ứng dụng

**Quan trọng**: Mở **terminal PowerShell/Command Prompt RIÊNG BIỆT**, không cùng terminal với test

```powershell
# Terminal 2 (riêng biệt từ test)
WinAppDriver.exe 127.0.0.1 4723
```

**Kết quả mong đợi**:
```
Windows Application Driver listening on http://127.0.0.1:4723/
```

**Kiểm tra port bằng lệnh khác**:
```powershell
# Kiểm tra port 4723 đang lắng nghe
netstat -ano | findstr :4723
# Output: TCP    127.0.0.1:4723    0.0.0.0:0    LISTENING    12345
```

**Nếu port đã được sử dụng**:
```powershell
# Tìm process sử dụng port 4723
Get-NetTCPConnection -LocalPort 4723 | Select-Object OwnerProcess

# Kill process (nếu không cần)
Stop-Process -Id <PID> -Force
```

---

### Bước 3: Enable UI Tests (Set Biến Môi Trường)

**Mục tiêu**: Cho phép các test chạy thay vì bị skip

**Terminal 1 (chạy test)** - một terminal khác với WinAppDriver:

```powershell
cd d:\Code\KiemThu\QuanLyChungCu_Final

# Set biến môi trường
$env:RUN_WINAPPDRIVER_TESTS = "true"

# Kiểm tra (optional)
Write-Host $env:RUN_WINAPPDRIVER_TESTS  # Output: true
```

**Lưu ý**: Biến môi trường chỉ có hiệu lực trong session hiện tại. 
- Nếu mở terminal mới, phải set lại
- Hoặc sử dụng script `run-ui-tests.ps1` (tự động set)

---

### Bước 4: Chạy Test

#### Cách 1: Chạy Tất Cả Test (Khuyến Nghị)

```powershell
# Cách này tự động set biến môi trường
.\run-ui-tests.ps1
```

**Kết quả mong đợi**:
```
Building...
  QuanLyChungCu.Tests.UI test net8.0-windows7.0 succeeded (0.7s)

Running 19 test cases...

Test summary: total: 19, failed: 0, succeeded: 19, skipped: 0, duration: 2m 45s
Build succeeded in 3.1s
```

#### Cách 2: Chạy Test Cụ Thể

```powershell
# Chạy một test cụ thể (vd: TC_ADDOWNER_001)
.\run-ui-tests.ps1 -TestCategory TC_ADDOWNER_001

# Hoặc chạy tất cả test "Thêm" (TC_ADDOWNER_*)
.\run-ui-tests.ps1 -TestCategory TC_ADDOWNER

# Hoặc chạy trực tiếp
$env:RUN_WINAPPDRIVER_TESTS = "true"
dotnet test --filter "TestCategory=TC_ADDOWNER_001"
```

#### Cách 3: Chạy Từng Group Test

```powershell
# Chạy tất cả test Thêm Chủ Hộ (6 tests)
$env:RUN_WINAPPDRIVER_TESTS = "true"
dotnet test --filter "TestCategory~TC_ADDOWNER"

# Chạy tất cả test Sửa (4 tests)
dotnet test --filter "TestCategory~TC_EDITOWNER"

# Chạy tất cả test Xóa (2 tests)
dotnet test --filter "TestCategory~TC_DELOWNER"

# Chạy tất cả test Tìm Kiếm (7 tests)
dotnet test --filter "TestCategory~TC_SEARCH"
```

---

### Bước 5: Xem Báo Cáo Test

#### 5a. Báo Cáo TRX

File được lưu tại:
```
QuanLyChungCu.Tests.UI/TestResults/ui-tests.trx
```

**Mở bằng Visual Studio**:
```powershell
Start-Process .\QuanLyChungCu.Tests.UI\TestResults\ui-tests.trx
```

**Thông tin trong báo cáo**:
- ✅ Tên từng test
- ✅ Trạng thái (PASSED, FAILED, SKIPPED)
- ✅ Thời gian chạy mỗi test
- ✅ Stack trace nếu lỗi

#### 5b. Báo Cáo Console

Khi chạy test, console sẽ hiển thị:

```
Test summary: total: 19, failed: 0, succeeded: 19, skipped: 0, duration: 2m 45s

PASSED: TC_ADDOWNER_001_WithValidData_AddsSuccessfully (5.2 sec)
PASSED: TC_ADDOWNER_002_WithInvalidRoomNumber_ShowsError (4.8 sec)
...
```

---

## Ví Dụ Thực Tế

### Tình Huống 1: Chạy Lần Đầu Tiên

```powershell
# Terminal 1: Build app
cd d:\Code\KiemThu\QuanLyChungCu_Final
dotnet build .\QuanLyChungCu\QuanLyChungCu.csproj

# Terminal 2: Start WinAppDriver (keep running)
WinAppDriver.exe 127.0.0.1 4723

# Terminal 1 (sau khi build xong): Run tests
$env:RUN_WINAPPDRIVER_TESTS = "true"
.\run-ui-tests.ps1

# Kết quả: 19 tests passed ✅
```

### Tình Huống 2: Chạy Test Cụ Thể (Debug)

```powershell
# Chỉ chạy test thêm chủ hộ đầu tiên
$env:RUN_WINAPPDRIVER_TESTS = "true"
dotnet test --filter "TestCategory=TC_ADDOWNER_001" --logger "console;verbosity=detailed"

# Kết quả chi tiết từng bước
[Test] TC_ADDOWNER_001_WithValidData_AddsSuccessfully
  [Setup] Seeding test data...
  [Action] Logging in as admin...
  [Action] Navigating to Owner screen...
  [Action] Opening Add Owner dialog...
  [Assert] Owner 'Chu ho test 001' is visible
PASSED
```

### Tình Huống 3: Test Lỗi

```powershell
# Test bị timeout
FAILED: TC_SEARCH_001_SearchByName_FiltersSuccessfully
Message: Condition was not met within 3 seconds.

[Search by Name with 'timkiem']
Element snapshot (Name | AutomationId | ClassName):
- Danh Sách Cư dân | | ...
- Nhập từ khóa... | Owner.SearchInput | TextBox
- Tìm Kiếm | Owner.SearchButton | Button
...

# Giải pháp:
1. Kiểm tra app chạy chậm?
2. Kiểm tra locator Owner.SearchInput có tồn tại?
3. Tăng timeout nếu cần
```

---

## Các Lệnh Thường Dùng

```powershell
# ========== BUILD & RUN ==========

# Build app
dotnet build .\QuanLyChungCu\QuanLyChungCu.csproj

# Run all tests
$env:RUN_WINAPPDRIVER_TESTS = "true"
dotnet test .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj

# Run specific test
dotnet test --filter "TestCategory=TC_ADDOWNER_001"

# Run with script
.\run-ui-tests.ps1

# ========== CLEANUP ==========

# Kill app if locked
Get-Process QuanLyChungCu | Stop-Process -Force

# Clear test results
Remove-Item .\QuanLyChungCu.Tests.UI\TestResults -Recurse -Force

# ========== DIAGNOSTICS ==========

# Check .NET version
dotnet --version

# Check WinAppDriver
Get-Command WinAppDriver

# Check port 4723
netstat -ano | findstr :4723

# View test results
Start-Process .\QuanLyChungCu.Tests.UI\TestResults\ui-tests.trx

# ========== ADVANCED ==========

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"

# Run and generate HTML report
dotnet test --logger "html;LogFileName=report.html"

# Run without UI tests (unit tests only)
# (Don't set $env:RUN_WINAPPDRIVER_TESTS)
dotnet test
```

---

## Một Số Vấn Đề Thường Gặp

### ❌ "Tests are skipped"

```powershell
# Problem:
Test summary: total: 19, failed: 0, succeeded: 0, skipped: 19, duration: 0.7s

# Solution:
$env:RUN_WINAPPDRIVER_TESTS = "true"
# Then run test again
```

### ❌ "Unable to connect to WinAppDriver"

```powershell
# Problem:
WebDriverException: Unable to connect to the remote server

# Solution:
# Terminal 2 (new):
WinAppDriver.exe 127.0.0.1 4723

# Check listening:
netstat -ano | findstr :4723
```

### ❌ "App executable not found"

```powershell
# Problem:
App executable not found at 'C:\...\QuanLyChungCu.exe'

# Solution:
dotnet build .\QuanLyChungCu\QuanLyChungCu.csproj
```

### ❌ "Process is locked"

```powershell
# Problem:
The process cannot access the file because it is being used by another process

# Solution:
Get-Process QuanLyChungCu | Stop-Process -Force
```

---

## Đọc Thêm

- 📖 **TESTING_PROJECT_GUIDE.md** - Hướng dẫn chi tiết đầy đủ (40+ trang)
- 📋 **README.md** - Khởi động nhanh
- 💾 **Tests/LoginTests.cs** - Source code 19 test case
- 🔧 **Infrastructure/TestConfig.cs** - Cấu hình paths và URL
- 🧪 **Pages/OwnerManagementPage.cs** - Helper methods

---

**Last Updated**: Tháng 3, 2026
**Status**: ✅ 19/19 Test Cases - All Implemented

