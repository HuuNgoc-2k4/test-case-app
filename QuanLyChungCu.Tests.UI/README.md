# QuanLyChungCu.Tests.UI - Bộ Kiểm Thử UI

Bộ kiểm thử UI tự động End-to-End cho ứng dụng WPF **Quản Lý Chung Cư** sử dụng WinAppDriver + MSTest.

## 📋 Thông Tin Nhanh

- **Tổng số test case**: 27
- **Phạm vi kiểm thử**: Đăng nhập + Quản lý chủ hộ (Add, Edit, Delete, Search)
- **Công nghệ**: WinAppDriver, MSTest, .NET 8
- **Thời gian chạy**: ~3-4 phút cho tất cả 27 test
- **Trạng thái**: ✅ Hoàn thành

## ⚡ Khởi Động Nhanh

```powershell
# 1. Build app
cd d:\Code\KiemThu\QuanLyChungCu_Final
dotnet build .\QuanLyChungCu\QuanLyChungCu.csproj

# 2. Start WinAppDriver (terminal riêng)
WinAppDriver.exe 127.0.0.1 4723

# 3. Run tests
$env:RUN_WINAPPDRIVER_TESTS = "true"
.\run-ui-tests.ps1
```

## 📚 Tài Liệu

| Tài liệu | Nội dung |
|---------|---------|
| **TESTING_PROJECT_GUIDE.md** | 📖 Hướng dẫn chi tiết đầy đủ (cài đặt, test case, gỡ rối) |
| **README.md** | 📋 Tài liệu này (khởi động nhanh) |
| **Tests/LoginTests.cs** | 💾 Source code 19 test quản lý chủ hộ |
| **Tests/AuthenticationTests.cs** | 🔐 Source code 8 test đăng nhập |
| **Pages/\*.cs** | 🔧 Helper classes để tương tác UI |

**👉 Đọc `TESTING_PROJECT_GUIDE.md` để hiểu chi tiết về:**
- Cài đặt WinAppDriver
- 27 test case và mục tiêu từng test
- Cách chạy test
- Cách đọc báo cáo
- Gỡ rối các vấn đề thường gặp

## ✅ Test Case (27 Tests)

### Đăng Nhập (TC_LOGIN) - 8 tests
- TC_LOGIN_001: Đăng nhập với tài khoản và mật khẩu trống
- TC_LOGIN_002: Đăng nhập với tài khoản đúng, mật khẩu sai
- TC_LOGIN_003: Đăng nhập với tài khoản sai, mật khẩu đúng
- TC_LOGIN_004: Đăng nhập thành công với tài khoản admin
- TC_LOGIN_005: Đăng nhập thành công với tài khoản cư dân
- TC_LOGIN_006: Hiển thị mật khẩu
- TC_LOGIN_007: Ẩn mật khẩu
- TC_LOGIN_008: Thu nhỏ cửa sổ đăng nhập

### Thêm Chủ Hộ (TC_ADDOWNER) - 6 tests
- TC_ADDOWNER_001: Thêm với dữ liệu hợp lệ
- TC_ADDOWNER_002: Lỗi số phòng không hợp lệ
- TC_ADDOWNER_003: Lỗi SĐT không hợp lệ
- TC_ADDOWNER_004: Lỗi SĐT trùng lặp
- TC_ADDOWNER_005: Lỗi số phòng trùng lặp
- TC_ADDOWNER_006: Thêm dữ liệu đầy đủ

### Sửa Chủ Hộ (TC_EDITOWNER) - 4 tests
- TC_EDITOWNER_001: Sửa tên
- TC_EDITOWNER_002: Lỗi số phòng
- TC_EDITOWNER_003: Lỗi SĐT
- TC_EDITOWNER_004: Sửa dữ liệu đầy đủ

### Xóa Chủ Hộ (TC_DELOWNER) - 2 tests
- TC_DELOWNER_001: Xóa và xác nhận
- TC_DELOWNER_002: Xóa nhưng hủy

### Tìm Kiếm (TC_SEARCH) - 7 tests
- TC_SEARCH_001: Tìm theo tên
- TC_SEARCH_002: Tìm theo SĐT
- TC_SEARCH_003: Tìm theo số phòng
- TC_SEARCH_004: Tìm theo quê quán
- TC_SEARCH_005: Tìm theo ngày sinh
- TC_SEARCH_006: Tìm không có kết quả
- TC_SEARCH_007: Xóa bộ lọc

## 🛠️ Yêu Cầu Hệ Thống

- Windows 10/11
- .NET SDK 8.0+
- WinAppDriver 1.2.x+

## 🔧 Cài Đặt Cơ Bản

### 1. Cài WinAppDriver
```powershell
# Tải từ: https://github.com/Microsoft/WinAppDriver/releases
# Hoặc dùng Chocolatey
choco install winappdriver
```

### 2. Khởi động WinAppDriver
```powershell
WinAppDriver.exe 127.0.0.1 4723
```

### 3. Build ứng dụng
```powershell
dotnet build .\QuanLyChungCu\QuanLyChungCu.csproj
```

### 4. Chạy test
```powershell
$env:RUN_WINAPPDRIVER_TESTS = "true"
dotnet test .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj
```

## 📊 Kết Quả Dự Kiến

```
Test summary: total: 27, failed: 0, succeeded: 27, skipped: 0, duration: ~4m
```

## 📖 Hướng Dẫn Chi Tiết

**Để hiểu rõ hơn về:**
- ✅ Cách cài đặt WinAppDriver
- ✅ Từng test case làm gì, kỳ vọng gì
- ✅ Cách chạy, cách đọc báo cáo
- ✅ Gỡ rối lỗi
- ✅ Biến môi trường, lệnh advanced

👉 **Xem file `TESTING_PROJECT_GUIDE.md` (Hướng dẫn chi tiết đầy đủ)**

## ⚙️ Biến Môi Trường

```powershell
# Bắt buộc: Cho phép chạy UI test
$env:RUN_WINAPPDRIVER_TESTS = "true"

# Tùy chọn: URL WinAppDriver (mặc định: http://127.0.0.1:4723/)
$env:WINAPPDRIVER_URL = "http://127.0.0.1:4723/"

# Tùy chọn: Đường dẫn app tùy chỉnh
$env:QLCC_APP_EXE = "C:\Path\To\QuanLyChungCu.exe"
```

## 🎯 Các Lệnh Thường Dùng

```powershell
# Chạy tất cả test
.\run-ui-tests.ps1

# Chạy test cụ thể
.\run-ui-tests.ps1 -TestCategory TC_ADDOWNER_001

# Chạy chi tiết với verbose output
$env:RUN_WINAPPDRIVER_TESTS = "true"
dotnet test --logger "console;verbosity=detailed"

# Kết thúc app nếu bị lock
Get-Process QuanLyChungCu | Stop-Process -Force
```

## ⚠️ Vấn Đề Thường Gặp

| Vấn Đề | Giải pháp |
|--------|----------|
| Test bị skip | Set `$env:RUN_WINAPPDRIVER_TESTS = "true"` |
| WinAppDriver connection failed | Khởi động `WinAppDriver.exe 127.0.0.1 4723` |
| App executable not found | Build: `dotnet build .\QuanLyChungCu\QuanLyChungCu.csproj` |
| Element not found | Kiểm tra AutomationId/Name trong XAML |
| Process locked | Kill: `Get-Process QuanLyChungCu \| Stop-Process -Force` |

**👉 Xem `TESTING_PROJECT_GUIDE.md` mục "Gỡ Rối" để chi tiết**

## 📝 File Cấu Trúc

```
QuanLyChungCu.Tests.UI/
├── Tests/
│   ├── LoginTests.cs              # Module quản lý chủ hộ
│   └── AuthenticationTests.cs     # Module testcase đăng nhập (TC_LOGIN_001..008)
├── Pages/
│   ├── LoginPage.cs               # Helper login
│   ├── OwnerManagementPage.cs     # Helper owner management
│   └── OwnerDialogPage.cs         # Helper dialog
├── Infrastructure/
│   ├── DriverFactory.cs           # Tạo WinAppDriver session
│   ├── TestConfig.cs              # Cấu hình
│   └── TestDataSeeder.cs          # Khởi tạo dữ liệu test
├── TESTING_PROJECT_GUIDE.md       # 📖 Hướng dẫn chi tiết (đọc trước!)
└── README.md                      # Tài liệu này
```

## 📞 Hỗ Trợ

Nếu gặp vấn đề:

1. **Kiểm tra yêu cầu**: Windows 10/11, .NET 8, WinAppDriver
2. **Kiểm tra WinAppDriver**: Đang chạy trên port 4723?
3. **Kiểm tra Build**: App đã build thành công?
4. **Kiểm tra Biến Môi Trường**: `RUN_WINAPPDRIVER_TESTS = true`?
5. **Đọc Diagnostic Output**: Xem element snapshot trong error
6. **Xem `TESTING_PROJECT_GUIDE.md`**: Mục "Gỡ Rối"

---

**📖 Đọc tiếp**: `TESTING_PROJECT_GUIDE.md` (Hướng dẫn đầy đủ 27 test case + cài đặt + gỡ rối)

## 🔐 Chạy Riêng Module Đăng Nhập

```powershell
$env:RUN_WINAPPDRIVER_TESTS = "true"
dotnet test .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj --filter "TestCategory~TC_LOGIN"
```
