# QuanLyChungCu_Final - Hệ Thống Kiểm Thử Tự Động UI

[![GitHub](https://img.shields.io/badge/GitHub-QuanLyChungCu_Final-blue)](https://github.com)
[![.NET](https://img.shields.io/badge/.NET-8-blue)](https://dotnet.microsoft.com/)
[![WinAppDriver](https://img.shields.io/badge/WinAppDriver-UI%20Test-green)](https://github.com/microsoft/WinAppDriver)

Hệ thống kiểm thử tự động End-to-End (E2E) cho ứng dụng desktop **Quản Lý Chung Cư** sử dụng **WinAppDriver** và **MSTest**. Dự án này bao gồm mục tiêu kiểm thử, kiến trúc, hướng dẫn cài đặt chi tiết, cách chạy test, cách đọc báo cáo, xử lý lỗi, và hướng dẫn đẩy lên GitHub.

---

## 📚 Mục Lục

1. [Tổng Quan Dự Án](#tổng-quan-dự-án)
2. [Quick Start - 5 Phút](#quick-start---5-phút)
3. [Yêu Cầu Hệ Thống](#yêu-cầu-hệ-thống)
4. [Hướng Dẫn Cài Đặt Chi Tiết](#hướng-dẫn-cài-đặt-chi-tiết)
5. [Phạm Vi Kiểm Thử](#phạm-vi-kiểm-thử)
6. [Cấu Trúc Dự Án](#cấu-trúc-dự-án)
7. [Tổng Hợp Lệnh](#tổng-hợp-lệnh)
8. [Cách Đọc Báo Cáo](#cách-đọc-báo-cáo)
9. [Xử Lý Lỗi (Troubleshooting)](#xử-lý-lỗi-troubleshooting)
10. [Đẩy Lên GitHub](#đẩy-lên-github)
11. [Tài Liệu Tham Khảo](#tài-liệu-tham-khảo)

---

## Tổng Quan Dự Án

### 🎯 Mục Tiêu

Dự án kiểm thử tự động UI cho ứng dụng WPF **Quản Lý Chung Cư**, tập trung vào:

- ✅ Kiểm thử các luồng nghiệp vụ quan trọng (Add/Edit/Delete/Search chủ hộ) theo hướng **End-to-End (E2E)**
- ✅ Phát hiện sớm lỗi giao diện, locator và điều hướng màn hình
- ✅ Đảm bảo test **ổn định, lặp lại** thông qua seed dữ liệu tự động
- ✅ Hỗ trợ debug nhanh khi test fail bằng thông tin chẩn đoán chi tiết

### 🛠️ Công Nghệ Chính

| Công Nghệ | Mục Đích | Phiên Bản |
|-----------|---------|----------|
| **WPF** | Framework giao diện ứng dụng desktop | - |
| **MSTest** | Framework kiểm thử | v3 |
| **WinAppDriver** | Điều khiển ứng dụng desktop Windows | v1.2.3+ |
| **Page Object Model** | Tách lớp thao tác giao diện từ logic test | - |
| **.NET SDK** | Nền tảng chạy | 8.0+ |

### 💡 Lợi Ích của Test Tự Động

- 🚀 **Chạy nhanh**: Giảm thời gian kiểm thử thủ công
- 🔄 **Lặp lại ổn định**: Kết quả không bị ảnh hưởng bởi hành động con người
- 🐛 **Phát hiện sớm lỗi hồi quy**: Chạy sau mỗi lần thay đổi mã nguồn
- 📊 **Báo cáo chi tiết**: Dễ dàng theo dõi kết quả và tìm nguyên nhân lỗi

---

## Quick Start - 5 Phút

Nếu bạn vừa tải dự án này, hãy làm theo các bước dưới đây để chạy test lần đầu **trong vòng 5 phút**.

### ✅ Bước 1: Kiểm Tra Điều Kiện Tiên Quyết (2 phút)

```powershell
# Kiểm tra .NET SDK
dotnet --version
# Expected: 8.0.x

# Kiểm tra WinAppDriver
WinAppDriver.exe --help
# Expected: Windows Application Driver v1.2.x

# Kiểm tra Git
git --version
# Expected: git version 2.x.x
```

**Nếu thiếu gì:**
- **.NET SDK** → [Tải tại đây](https://dotnet.microsoft.com/download)
- **WinAppDriver** → [Tải tại đây](https://github.com/microsoft/WinAppDriver/releases)
- **Git** → [Tải tại đây](https://git-scm.com/)

### ✅ Bước 2: Chuẩn Bị Dự Án (1 phút)

```powershell
# Vào thư mục dự án
cd d:\Code\KiemThu\QuanLyChungCu_Final

# Build ứng dụng
dotnet build .\QuanLyChungCu\QuanLyChungCu.csproj -c Release

# Restore packages test
dotnet restore .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj
```

### ✅ Bước 3: Khởi Động WinAppDriver (Bước này quan trọng!)

Mở **PowerShell mới** (Terminal 1) và chạy:

```powershell
WinAppDriver.exe 127.0.0.1 4723
```

**Bạn sẽ thấy:**
```
WinAppDriver started listening on http://127.0.0.1:4723
```

⚠️ **Đừng đóng terminal này!** Giữ nó chạy trong suốt quá trình test.

### ✅ Bước 4: Chạy Test (2 phút)

Mở **PowerShell mới khác** (Terminal 2) và chạy:

```powershell
# Vào thư mục dự án
cd d:\Code\KiemThu\QuanLyChungCu_Final

# Bật biến môi trường (BẮT BUỘC)
$env:RUN_WINAPPDRIVER_TESTS = "true"

# Chạy test
dotnet test .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj
```

### 🎉 Kết Quả

Bạn sẽ thấy:

```
  Passed  AddOwner_AddsNewOwnerToList
  Passed  EditOwner_UpdatesOwnerInformation
  Passed  DeleteOwner_RemovesOwnerFromList
  Passed  SearchOwner_FiltersByKeyword

Test Run Successful.
Total tests: 4
Passed: 4
Failed: 0
Duration: ~45s
```

**Xin chúc mừng! Test chạy thành công! 🎊**

---

## Yêu Cầu Hệ Thống

### Phần Cứng Tối Thiểu

- **CPU**: Dual-core 2.0 GHz trở lên
- **RAM**: 4GB tối thiểu (8GB khuyến khích)
- **Ổ Cứng**: 5GB dung lượng trống

### Phần Mềm

| Phần Mềm | Phiên Bản | Mục Đích | Tải Xuống |
|---------|----------|--------|----------|
| Windows | 10, 11 trở lên | Hệ điều hành | [Microsoft](https://www.microsoft.com/windows) |
| .NET SDK | 8.0 hoặc mới hơn | Nền tảng phát triển | [dotnet.microsoft.com](https://dotnet.microsoft.com/download) |
| WinAppDriver | 1.2.3+ | Điều khiển UI test | [GitHub - WinAppDriver](https://github.com/microsoft/WinAppDriver/releases) |
| Visual Studio | 2022+ (tuỳ chọn) | IDE phát triển | [Visual Studio](https://visualstudio.microsoft.com/) |
| Git | Mới nhất | Quản lý mã nguồn | [git-scm.com](https://git-scm.com/) |

---

## Hướng Dẫn Cài Đặt Chi Tiết

### 🔧 Bước 1: Cài Đặt .NET SDK 8

#### Tại Sao Cần .NET SDK?
**.NET SDK** là nền tảng chạy ứng dụng C# và dự án test của chúng ta.

#### Cách Cài Đặt

**Cách 1: Tải từ Website Microsoft (Khuyến Khích)**

1. Truy cập: https://dotnet.microsoft.com/download
2. Tìm **.NET 8** (phiên bản mới nhất hoặc LTS)
3. Click **Download** (chọn phiên bản **x64** cho Windows 64-bit)
4. Chạy file installer `dotnet-sdk-8.x.xxx-win-x64.exe`
5. Chọn **Install**, đợi hoàn tất (khoảng 2-5 phút)
6. Nhấn **Close** khi xong

**Cách 2: Dùng Chocolatey (Nếu Đã Cài)**

```powershell
choco install dotnet-sdk
```

#### Kiểm Tra Cài Đặt

```powershell
dotnet --version
# Kết quả mong đợi: 8.0.x
```

### 🎮 Bước 2: Cài Đặt WinAppDriver

#### Tại Sao Cần WinAppDriver?
**WinAppDriver** giúp test tự động điều khiển giao diện ứng dụng desktop Windows.

#### Cách Cài Đặt

**Phương Pháp 1: Tải từ GitHub (Khuyến Khích)**

1. Truy cập: https://github.com/microsoft/WinAppDriver/releases
2. Tìm phiên bản mới nhất (ví dụ: **v1.2.3**)
3. Download file: `WindowsAppDriver-x64-...exe`
4. Chạy file installer
5. Chọn **Install**, chọn thư mục mặc định
6. Nhấn **Next** → **Finish** khi xong

#### Kiểm Tra Cài Đặt

```powershell
WinAppDriver.exe --help
# Hoặc tìm WinAppDriver trong Start Menu
```

### 📦 Bước 3: Cài Đặt Git

#### Cách Cài Đặt

1. Truy cập: https://git-scm.com/
2. Download Windows x64
3. Chạy installer
4. Chọn **Install**, giữ mặc định
5. Nhấn **Next** → **Finish**

#### Kiểm Tra Cài Đặt

```powershell
git --version
# Kết quả: git version 2.x.x
```

#### Cấu Hình Git (Một Lần)

```powershell
git config --global user.name "Tên của bạn"
git config --global user.email "email@example.com"
```

### 🔗 Bước 4: Clone Dự Án

```powershell
# Chọn thư mục để clone
cd d:\

# Clone repository
git clone https://github.com/YourUsername/QuanLyChungCu_Final.git

# Vào thư mục dự án
cd QuanLyChungCu_Final
```

### 🏗️ Bước 5: Build Ứng Dụng

```powershell
# Build ứng dụng chính
dotnet build .\QuanLyChungCu\QuanLyChungCu.csproj -c Release

# Build test project
dotnet build .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj
```

### 📦 Bước 6: Restore Packages

```powershell
# Restore packages
dotnet restore .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj
```

### 🔐 Bước 7: Cấu Hình Biến Môi Trường

**Thêm biến môi trường** (tuỳ chọn nếu muốn custom):

```powershell
# Tùy chọn 1: Đặt tạm thời (chỉ session hiện tại)
$env:RUN_WINAPPDRIVER_TESTS = "true"

# Tùy chọn 2: Đặt vĩnh viễn (cho tất cả session)
[Environment]::SetEnvironmentVariable("RUN_WINAPPDRIVER_TESTS", "true", "User")
```

### ✅ Bước 8: Chạy Test Lần Đầu

```powershell
# Khởi động WinAppDriver (Terminal 1)
WinAppDriver.exe 127.0.0.1 4723

# Chạy test (Terminal 2)
$env:RUN_WINAPPDRIVER_TESTS = "true"
dotnet test .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj
```

---

## Phạm Vi Kiểm Thử

### Luồng Nghiệp Vụ Được Kiểm Thử

| Testcase | Mô Tả | Trạng Thái |
|----------|-------|-----------|
| **AddOwner_AddsNewOwnerToList** | Thêm chủ hộ mới | ✅ Tự động hóa |
| **EditOwner_UpdatesOwnerInformation** | Sửa thông tin chủ hộ | ✅ Tự động hóa |
| **DeleteOwner_RemovesOwnerFromList** | Xóa chủ hộ | ✅ Tự động hóa |
| **SearchOwner_FiltersByKeyword** | Tìm kiếm chủ hộ theo 5 tiêu chí | ✅ Tự động hóa |

### Chi Tiết Testcase Tìm Kiếm

Test `SearchOwner_FiltersByKeyword` kiểm tra đầy đủ theo 5 tiêu chí:

- Tìm theo **Tên**
- Tìm theo **Số Điện Thoại**
- Tìm theo **Số Phòng**
- Tìm theo **Quê Quán**
- Tìm theo **Ngày Sinh**

### Dữ Liệu Test

Dữ liệu test được seed tự động tại `TestInitialize` thông qua `TestDataSeeder`:

- Seed tài khoản đăng nhập admin
- Seed dữ liệu chủ hộ phục vụ Add/Edit/Delete/Search
- Dọn dữ liệu cũ trước khi seed để giữ tính lặp lại

---

## Cấu Trúc Dự Án

```
QuanLyChungCu_Final/
├── QuanLyChungCu/                      # Ứng dụng chính WPF
│   ├── *.xaml / *.xaml.cs              # Giao diện và logic
│   ├── DatabaseHelper.cs               # Thao tác cơ sở dữ liệu
│   └── QuanLyChungCu.csproj
│
├── QuanLyChungCu.Tests.UI/            # Dự án kiểm thử tự động
│   ├── Infrastructure/
│   │   ├── DriverFactory.cs            # Tạo WinAppDriver session
│   │   ├── TestConfig.cs               # Cấu hình môi trường
│   │   └── TestDataSeeder.cs           # Seed dữ liệu test
│   ├── Pages/
│   │   ├── LoginPage.cs                # Thao tác màn hình đăng nhập
│   │   ├── OwnerManagementPage.cs      # Thao tác danh sách chủ hộ
│   │   └── OwnerDialogPage.cs          # Thao tác popup thêm/sửa chủ hộ
│   ├── Tests/
│   │   └── LoginTests.cs               # Testcase (chứa OwnerManagementTests)
│   └── QuanLyChungCu.Tests.UI.csproj
│
├── README.md                           # File này
├── QUICK_START.md                      # Hướng dẫn 5 phút
├── INSTALLATION_GUIDE.md               # Cài đặt chi tiết
├── TEST_REPORT_GUIDE.md                # Đọc báo cáo test
├── TROUBLESHOOTING.md                  # Xử lý lỗi
└── GITHUB_GUIDE.md                     # Đẩy lên GitHub
```

### Giải Thích Thư Mục Test

- **Infrastructure/**: Quản lý cấu hình môi trường, session WinAppDriver, seed dữ liệu test
- **Pages/**: Đóng gói thao tác UI theo màn hình/popup (Page Object Model)
- **Tests/**: Chứa test theo luồng nghiệp vụ

---

## Tổng Hợp Lệnh

### 🎯 Lệnh Cài Đặt

```powershell
# Kiểm tra điều kiện tiên quyết
dotnet --version
WinAppDriver.exe --help
git --version

# Chuẩn bị dự án
cd d:\Code\KiemThu\QuanLyChungCu_Final
dotnet build .\QuanLyChungCu\QuanLyChungCu.csproj -c Release
dotnet restore .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj
```

### 🚀 Lệnh Chạy Test

```powershell
# Terminal 1: Khởi động WinAppDriver
WinAppDriver.exe 127.0.0.1 4723

# Terminal 2: Chạy test
cd d:\Code\KiemThu\QuanLyChungCu_Final
$env:RUN_WINAPPDRIVER_TESTS = "true"
dotnet test .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj

# Chạy test với báo cáo TRX
dotnet test .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj --logger "trx;LogFileName=ui-tests.trx"

# Chạy một testcase cụ thể
dotnet test .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj --filter "SearchOwner_FiltersByKeyword"
```

### 📊 Lệnh Build Và Debug

```powershell
# Build debug
dotnet build .\QuanLyChungCu\QuanLyChungCu.csproj -c Debug

# Build release
dotnet build .\QuanLyChungCu\QuanLyChungCu.csproj -c Release

# Clean
dotnet clean .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj

# Restore
dotnet restore .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj
```

---

## Cách Đọc Báo Cáo

### 📺 Console Output

Sau khi chạy test, bạn sẽ thấy:

```powershell
Microsoft (R) Test Execution Command Line Tool

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

  Passed  AddOwner_AddsNewOwnerToList (Duration: 15.234s)
  Passed  EditOwner_UpdatesOwnerInformation (Duration: 18.567s)
  Failed  DeleteOwner_RemovesOwnerFromList (Duration: 12.345s)
          Error: OpenQA.Selenium.NoSuchElementException: Could not locate element...
  Skipped SearchOwner_FiltersByKeyword

Test Run Completed
Total tests: 4
Passed: 2
Failed: 1
Skipped: 1
```

### 📊 File Báo Cáo TRX

Báo cáo TRX lưu tại: `QuanLyChungCu.Tests.UI/TestResults/`

```xml
<TestRun>
  <Results>
    <UnitTestResult testName="AddOwner_AddsNewOwnerToList" outcome="Passed" 
                    duration="00:00:15.234" />
    <UnitTestResult testName="DeleteOwner_RemovesOwnerFromList" outcome="Failed"
                    duration="00:00:12.345">
      <Message>Element not found</Message>
      <StackTrace>...</StackTrace>
    </UnitTestResult>
  </Results>
</TestRun>
```

### 🔍 Phân Tích Kết Quả

| Trạng Thái | Ý Nghĩa |
|-----------|---------|
| ✅ **Passed** | Test thành công |
| ❌ **Failed** | Test thất bại, cần xem lỗi |
| ⏭️ **Skipped** | Test bị bỏ qua |
| ⏱️ **Timeout** | Test vượt quá thời gian cho phép |

---

## Xử Lý Lỗi (Troubleshooting)

### 🔴 LỖI CÀI ĐẶT

#### 1. "Command 'dotnet' is not recognized"

**Nguyên Nhân:**
- .NET SDK chưa được cài đặt
- .NET SDK cài nhưng chưa reload PowerShell

**Cách Fix:**
```powershell
# Cài .NET SDK từ https://dotnet.microsoft.com/download
# Đóng PowerShell hiện tại, mở PowerShell mới
dotnet --version  # Kiểm tra
```

#### 2. "WinAppDriver is not recognized"

**Nguyên Nhân:**
- WinAppDriver chưa được cài đặt
- Path không chứa thư mục WinAppDriver

**Cách Fix:**
```powershell
# Cài WinAppDriver từ https://github.com/microsoft/WinAppDriver/releases
# Thêm vào PATH hoặc chạy từ thư mục cài đặt
"C:\Program Files\Windows Application Driver\WinAppDriver.exe" 127.0.0.1 4723
```

### 🟡 LỖI CHẠY TEST

#### 3. "Unable to connect WinAppDriver"

**Nguyên Nhân:**
- WinAppDriver chưa khởi động
- WinAppDriver chạy trên port khác

**Cách Fix:**
```powershell
# Terminal 1: Khởi động WinAppDriver
WinAppDriver.exe 127.0.0.1 4723

# Kiểm tra WinAppDriver đã chạy
# Mở http://127.0.0.1:4723 trong trình duyệt
```

#### 4. "RUN_WINAPPDRIVER_TESTS not set"

**Nguyên Nhân:**
- Biến môi trường `RUN_WINAPPDRIVER_TESTS` chưa được đặt

**Cách Fix:**
```powershell
# Đặt biến môi trường
$env:RUN_WINAPPDRIVER_TESTS = "true"

# Chạy test
dotnet test .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj
```

#### 5. "NoSuchElementException"

**Nguyên Nhân:**
- Locator không đúng
- Element chưa load
- Element bị ẩn

**Cách Fix:**
```
- Kiểm tra locator trong code
- Tăng timeout chờ element
- Xem element snapshot trong log
```

### ❌ LỖI LOGIC TEST

#### 6. "AssertionFailedError"

**Nguyên Nhân:**
- Assertion không đúng
- Dữ liệu test không đúng

**Cách Fix:**
```
- Kiểm tra điều kiện assertion
- Seed dữ liệu test lại
- Xem chi tiết lỗi trong log
```

---

## Đẩy Lên GitHub

### 🚀 Bước 1: Chuẩn Bị GitHub Repository

1. Truy cập https://github.com/new
2. Nhập **Repository name**: `QuanLyChungCu_Final`
3. Chọn **Public**
4. Click **Create repository**

### 🔧 Bước 2: Cấu Hình Git

```powershell
# Set global user info
git config --global user.name "Tên của bạn"
git config --global user.email "email@example.com"
```

### 📤 Bước 3: Đẩy Lên GitHub

```powershell
# Vào thư mục dự án
cd d:\Code\KiemThu\QuanLyChungCu_Final

# Thêm remote (thay YourUsername)
git remote add origin https://github.com/YourUsername/QuanLyChungCu_Final.git

# Tạo branch main
git branch -M main

# Stage tất cả thay đổi
git add .

# Commit
git commit -m "Initial commit: QuanLyChungCu testing project"

# Push lên GitHub
git push -u origin main
```

### ✅ Xác Nhận

Truy cập https://github.com/YourUsername/QuanLyChungCu_Final để xác nhận dự án đã được push.

---

## Tài Liệu Tham Khảo

### 📚 Tài Liệu Chi Tiết

Dự án này có các file tài liệu bổ sung:

| File | Mục Đích |
|------|---------|
| **QUICK_START.md** | Bắt đầu nhanh 5 phút |
| **INSTALLATION_GUIDE.md** | Hướng dẫn cài đặt chi tiết |
| **TEST_REPORT_GUIDE.md** | Cách đọc và phân tích báo cáo |
| **TROUBLESHOOTING.md** | Giải pháp cho 18+ lỗi phổ biến |
| **GITHUB_GUIDE.md** | Hướng dẫn đẩy lên GitHub |

### 🔗 Liên Kết Hữu Ích

- [.NET Documentation](https://docs.microsoft.com/dotnet/)
- [WinAppDriver GitHub](https://github.com/microsoft/WinAppDriver)
- [MSTest Documentation](https://docs.microsoft.com/visualstudio/test/unit-test-your-code)
- [Git Documentation](https://git-scm.com/doc)
- [GitHub Documentation](https://docs.github.com/)

### 🆘 Hỗ Trợ

Nếu gặp vấn đề:

1. Xem file **TROUBLESHOOTING.md** để tìm giải pháp
2. Kiểm tra console output để xác định lỗi
3. Xem file log trong `TestResults/`
4. Tạo Issue trên GitHub

---

## 📝 Thay Đổi Gần Đây

### v1.0.0 - Hoàn Thành Đầu Tiên

- ✅ Kiểm thử 4 luồng nghiệp vụ (Add/Edit/Delete/Search chủ hộ)
- ✅ Hướng dẫn cài đặt chi tiết 8 bước
- ✅ Tài liệu toàn diện (cài đặt, chạy, debug, đẩy GitHub)
- ✅ Xử lý lỗi cho 18+ trường hợp phổ biến

---

## 📄 Bản Quyền

Dự án này được cấp phép dưới **MIT License**. Xem file LICENSE để chi tiết.

---

## 👥 Đóng Góp

Chúng ta rất mong nhận được đóng góp từ cộng đồng!

Để đóng góp:

1. Fork dự án
2. Tạo branch cho feature mới (`git checkout -b feature/AmazingFeature`)
3. Commit thay đổi (`git commit -m 'Add some AmazingFeature'`)
4. Push lên branch (`git push origin feature/AmazingFeature`)
5. Tạo Pull Request

---

## 📞 Liên Hệ

Nếu bạn có câu hỏi hoặc góp ý:

- 📧 Email: [your-email@example.com]
- 💬 Issues: [GitHub Issues](https://github.com/YourUsername/QuanLyChungCu_Final/issues)
- 🐦 Twitter: [@YourTwitter]

---

**Last Updated**: 2026-03-18
