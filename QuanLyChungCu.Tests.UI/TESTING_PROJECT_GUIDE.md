# Hướng Dẫn Dự Án Kiểm Thử UI - QuanLyChungCu

## Mục Lục
1. [Giới Thiệu](#giới-thiệu)
2. [Yêu Cầu Hệ Thống](#yêu-cầu-hệ-thống)
3. [Cài Đặt và Chuẩn Bị](#cài-đặt-và-chuẩn-bị)
4. [Cấu Trúc Dự Án](#cấu-trúc-dự-án)
5. [Chạy Test](#chạy-test)
6. [Các Test Case Chi Tiết](#các-test-case-chi-tiết)
7. [Cách Đọc Báo Cáo Test](#cách-đọc-báo-cáo-test)
8. [Gỡ Rối và Vấn Đề Thường Gặp](#gỡ-rối-và-vấn-đề-thường-gặp)
9. [Biến Môi Trường](#biến-môi-trường)

---

## Giới Thiệu

Dự án `QuanLyChungCu.Tests.UI` là bộ kiểm thử tự động End-to-End (E2E) cho ứng dụng WPF **Quản Lý Chung Cư**. 
Bộ kiểm thử sử dụng:
- **WinAppDriver**: Tương tác với UI của ứng dụng Windows
- **MSTest Framework**: Khung kiểm thử
- **.NET 8**: Công nghệ

### Phạm Vi Kiểm Thử

Hiện tại, bộ kiểm thử tập trung vào **quản lý chủ hộ (Owner Management)** với 19 test case:

#### **TC_ADDOWNER (Thêm Chủ Hộ) - 6 test case**
- ✅ TC_ADDOWNER_001: Thêm chủ hộ với dữ liệu hợp lệ
- ✅ TC_ADDOWNER_002: Thêm chủ hộ với số phòng không hợp lệ
- ✅ TC_ADDOWNER_003: Thêm chủ hộ với số điện thoại không hợp lệ
- ✅ TC_ADDOWNER_004: Thêm chủ hộ với số điện thoại trùng lặp
- ✅ TC_ADDOWNER_005: Thêm chủ hộ với số phòng trùng lặp
- ✅ TC_ADDOWNER_006: Thêm chủ hộ với dữ liệu đầy đủ và hợp lệ

#### **TC_EDITOWNER (Sửa Chủ Hộ) - 4 test case**
- ✅ TC_EDITOWNER_001: Sửa tên chủ hộ
- ✅ TC_EDITOWNER_002: Sửa chủ hộ với số phòng không hợp lệ
- ✅ TC_EDITOWNER_003: Sửa chủ hộ với số điện thoại không hợp lệ
- ✅ TC_EDITOWNER_004: Sửa tất cả thông tin chủ hộ

#### **TC_DELOWNER (Xóa Chủ Hộ) - 2 test case**
- ✅ TC_DELOWNER_001: Xóa chủ hộ và xác nhận
- ✅ TC_DELOWNER_002: Xóa chủ hộ nhưng hủy bỏ

#### **TC_SEARCH (Tìm Kiếm Chủ Hộ) - 7 test case**
- ✅ TC_SEARCH_001: Tìm kiếm theo tên
- ✅ TC_SEARCH_002: Tìm kiếm theo số điện thoại
- ✅ TC_SEARCH_003: Tìm kiếm theo số phòng
- ✅ TC_SEARCH_004: Tìm kiếm theo quê quán
- ✅ TC_SEARCH_005: Tìm kiếm theo ngày sinh
- ✅ TC_SEARCH_006: Tìm kiếm không có kết quả
- ✅ TC_SEARCH_007: Xóa tìm kiếm và hiển thị tất cả

---

## Yêu Cầu Hệ Thống

- **Hệ điều hành**: Windows 10 hoặc Windows 11
- **.NET SDK**: Phiên bản 8.0 trở lên
- **WinAppDriver**: Phiên bản 1.2.x trở lên
- **Visual Studio** hoặc **Visual Studio Code** (tùy chọn, có thể dùng command line)

### Kiểm Tra Yêu Cầu

```powershell
# Kiểm tra .NET SDK
dotnet --version

# Kiểm tra WinAppDriver đã cài
Get-Command WinAppDriver
```

---

## Cài Đặt và Chuẩn Bị

### Bước 1: Clone/Download Dự Án

```powershell
# Nếu chưa có code, clone từ repository
cd d:\Code\KiemThu
git clone <repository-url> QuanLyChungCu_Final
cd QuanLyChungCu_Final
```

### Bước 2: Cài Đặt WinAppDriver

WinAppDriver là công cụ cần thiết để điều khiển ứng dụng Windows.

**Cách 1: Tải từ GitHub**
- Truy cập: https://github.com/Microsoft/WinAppDriver/releases
- Tải phiên bản mới nhất (ví dụ: `WinAppDriver.exe`)
- Lưu vào thư mục `C:\Program Files` hoặc bất kỳ đường dẫn nào

**Cách 2: Cài qua Chocolatey (nếu có)**
```powershell
choco install winappdriver
```

### Bước 3: Build Ứng Dụng

```powershell
cd d:\Code\KiemThu\QuanLyChungCu_Final
dotnet build .\QuanLyChungCu\QuanLyChungCu.csproj -c Debug
```

### Bước 4: Khởi Động WinAppDriver

Mở **PowerShell** hoặc **Command Prompt** mới và chạy:

```powershell
WinAppDriver.exe 127.0.0.1 4723
```

Hoặc chỉ định đường dẫn đầy đủ nếu WinAppDriver không trong PATH:

```powershell
"C:\Program Files\Windows Application Driver\WinAppDriver.exe" 127.0.0.1 4723
```

**Kết quả mong đợi:**
```
Windows Application Driver listening on http://127.0.0.1:4723/
```

---

## Cấu Trúc Dự Án

```
QuanLyChungCu.Tests.UI/
├── Tests/
│   └── LoginTests.cs              # Tất cả test case (19 tests)
├── Pages/
│   ├── LoginPage.cs               # Helper cho trang Login
│   ├── OwnerManagementPage.cs     # Helper cho trang quản lý chủ hộ
│   └── OwnerDialogPage.cs         # Helper cho dialog thêm/sửa chủ hộ
├── Infrastructure/
│   ├── DriverFactory.cs           # Tạo WinAppDriver session
│   ├── TestConfig.cs              # Cấu hình test (đường dẫn app, URL WinAppDriver)
│   └── TestDataSeeder.cs          # Khởi tạo dữ liệu test trong DB
├── QuanLyChungCu.Tests.UI.csproj  # File dự án
└── README.md                      # Hướng dẫn tóm tắt
```

### Chi Tiết File Quan Trọng

**LoginTests.cs**
- Chứa 19 test method, mỗi test đại diện cho một test case
- Các test được đánh dấu với `[TestCategory("TC_ADDOWNER_001")]` để dễ lọc

**OwnerManagementPage.cs**
- Chứa các method tương tác với UI: `OpenAddOwnerDialog()`, `SearchByName()`, `DeleteFirstResult()`, v.v.
- Xây dựng locator và tìm element linh hoạt

**TestDataSeeder.cs**
- Khởi tạo dữ liệu ban đầu vào database SQLite
- Tạo bảng Owners, Residents, Money
- Insert 8 chủ hộ để phục vụ các test case

---

## Chạy Test

### Cách 1: Chạy Tất Cả Test

**Điều kiện tiên quyết:**
1. WinAppDriver đang chạy (xem Bước 4 ở trên)
2. Ứng dụng QuanLyChungCu không chạy (nếu chạy sẽ bị tự động đóng)

**Chạy test:**

```powershell
cd d:\Code\KiemThu\QuanLyChungCu_Final

# Cách 1a: Dùng script PowerShell (tự động set biến môi trường)
.\run-ui-tests.ps1

# Cách 1b: Chạy trực tiếp với biến môi trường
$env:RUN_WINAPPDRIVER_TESTS = "true"
dotnet test .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj --logger "trx;LogFileName=ui-tests.trx"
```

### Cách 2: Chạy Test Theo Danh Mục (Category)

```powershell
# Chạy tất cả test thêm chủ hộ
.\run-ui-tests.ps1 -TestCategory "TC_ADDOWNER_001"
.\run-ui-tests.ps1 -TestCategory "TC_EDITOWNER_001"
.\run-ui-tests.ps1 -TestCategory "TC_DELOWNER_001"
.\run-ui-tests.ps1 -TestCategory "TC_SEARCH_001"

# Hoặc chạy từng test cụ thể
$env:RUN_WINAPPDRIVER_TESTS = "true"
dotnet test .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj --filter "TestCategory=TC_ADDOWNER_001"
```

### Cách 3: Chạy Test Trong Visual Studio

1. Mở `QuanLyChungCu_Final.sln` trong Visual Studio
2. Vào **Test Explorer** (View → Test Explorer)
3. Right-click vào test case, chọn **Run**

**Lưu ý**: Phải set `RUN_WINAPPDRIVER_TESTS` trong terminal hoặc cấu hình beforehand.

### Kết Quả Chạy Test

Khi test chạy xong, file báo cáo sẽ được lưu:
```
QuanLyChungCu.Tests.UI/TestResults/ui-tests.trx
```

**Thời gian chạy dự kiến:**
- 1 test: ~5-10 giây
- 19 tests: ~2-3 phút

---

## Các Test Case Chi Tiết

### 1. TC_ADDOWNER_001: Thêm Chủ Hộ với Dữ Liệu Hợp Lệ

**Mục tiêu**: Kiểm thử khả năng thêm chủ hộ mới với dữ liệu đầy đủ

**Các bước**:
1. Đăng nhập với tài khoản admin (admin/1)
2. Điều hướng đến danh sách chủ hộ
3. Click nút "Thêm chủ hộ"
4. Điền form:
   - Tên: `Chu ho test 001`
   - SĐT: `0901999999`
   - Quê quán: `Nam Dinh`
   - Ngày sinh: `05/05/1985`
   - Số phòng: `105`
5. Click nút "Thêm"

**Kết quả mong đợi**: 
- ✓ Chủ hộ được thêm thành công
- ✓ Xuất hiện trong danh sách
- ✓ Không có lỗi

---

### 2. TC_ADDOWNER_002: Thêm Chủ Hộ với Số Phòng Không Hợp Lệ

**Mục tiêu**: Kiểm thử xác thực số phòng

**Các bước**:
1. Đăng nhập với tài khoản admin
2. Điều hướng đến danh sách chủ hộ
3. Click nút "Thêm chủ hộ"
4. Điền số phòng = `abc` (không phải số)
5. Click nút "Thêm"

**Kết quả mong đợi**:
- ✓ Xuất hiện thông báo lỗi: "Số phòng phải là số nguyên!"
- ✓ Form không đóng
- ✓ Chủ hộ không được thêm

---

### 3. TC_ADDOWNER_003: Thêm Chủ Hộ với Số Điện Thoại Không Hợp Lệ

**Mục tiêu**: Kiểm thử xác thực số điện thoại

**Các bước**:
1. Đăng nhập với tài khoản admin
2. Điều hướng đến danh sách chủ hộ
3. Click nút "Thêm chủ hộ"
4. Điền SĐT = `123` (quá ngắn, không bắt đầu bằng 0)
5. Click nút "Thêm"

**Kết quả mong đợi**:
- ✓ Xuất hiện thông báo lỗi: "Số điện thoại phải có 10 chữ số và bắt đầu bằng số 0!"
- ✓ Form không đóng
- ✓ Chủ hộ không được thêm

---

### 4. TC_ADDOWNER_004: Thêm Chủ Hộ với Số Điện Thoại Trùng Lặp

**Mục tiêu**: Kiểm thử phát hiện số điện thoại trùng lặp

**Dữ liệu tiền điều kiện**:
- Trong database đã có chủ hộ với SĐT = `0901000001`

**Các bước**:
1. Đăng nhập với tài khoản admin
2. Điều hướng đến danh sách chủ hộ
3. Click nút "Thêm chủ hộ"
4. Điền SĐT = `0901000001` (trùng với chủ hộ hiện có)
5. Click nút "Thêm"

**Kết quả mong đợi**:
- ✓ Xuất hiện thông báo lỗi: "Số điện thoại đã tồn tại, vui lòng nhập số khác!"
- ✓ Form không đóng
- ✓ Chủ hộ không được thêm

---

### 5. TC_ADDOWNER_005: Thêm Chủ Hộ với Số Phòng Trùng Lặp

**Mục tiêu**: Kiểm thử phát hiện số phòng trùng lặp

**Dữ liệu tiền điều kiện**:
- Trong database đã có chủ hộ với phòng = `101`

**Các bước**:
1. Đăng nhập với tài khoản admin
2. Điều hướng đến danh sách chủ hộ
3. Click nút "Thêm chủ hộ"
4. Điền số phòng = `101` (trùng với chủ hộ hiện có)
5. Click nút "Thêm"

**Kết quả mong đợi**:
- ✓ Xuất hiện thông báo lỗi: "Số phòng đã tồn tại, vui lòng nhập số phòng khác!"
- ✓ Form không đóng
- ✓ Chủ hộ không được thêm

---

### 6. TC_ADDOWNER_006: Thêm Chủ Hộ với Dữ Liệu Đầy Đủ

**Mục tiêu**: Kiểm thử thêm chủ hộ toàn bộ dữ liệu

**Các bước**:
1. Đăng nhập với tài khoản admin
2. Điều hướng đến danh sách chủ hộ
3. Click nút "Thêm chủ hộ"
4. Điền toàn bộ thông tin hợp lệ
5. Click nút "Thêm"

**Kết quả mong đợi**:
- ✓ Chủ hộ được thêm thành công
- ✓ Xuất hiện trong danh sách
- ✓ Không có chủ hộ nào khác hiện lên

---

### 7-10. TC_EDITOWNER: Sửa Chủ Hộ

Tương tự các test Add, nhưng:
- Tìm chủ hộ cần sửa
- Click nút "Sửa" (thay vì "Thêm")
- Sửa thông tin
- Click nút "Lưu" (thay vì "Thêm")

---

### 11-12. TC_DELOWNER: Xóa Chủ Hộ

**TC_DELOWNER_001**: Xóa và xác nhận
- Tìm chủ hộ
- Click nút "Xóa"
- Xác nhận "Có"
- ✓ Chủ hộ bị xóa khỏi danh sách

**TC_DELOWNER_002**: Xóa nhưng hủy bỏ
- Tìm chủ hộ
- Click nút "Xóa"
- Nhấn "Không"
- ✓ Chủ hộ vẫn còn trong danh sách

---

### 13-19. TC_SEARCH: Tìm Kiếm Chủ Hộ

Mỗi test tìm kiếm theo một tiêu chí khác nhau:
- TC_SEARCH_001: Tìm theo tên
- TC_SEARCH_002: Tìm theo SĐT
- TC_SEARCH_003: Tìm theo số phòng
- TC_SEARCH_004: Tìm theo quê quán
- TC_SEARCH_005: Tìm theo ngày sinh
- TC_SEARCH_006: Tìm không có kết quả
- TC_SEARCH_007: Xóa bộ lọc (Clear search)

---

## Cách Đọc Báo Cáo Test

### Loại Báo Cáo

#### 1. Báo Cáo TRX (Test Results XML)

Được lưu tại: `QuanLyChungCu.Tests.UI/TestResults/ui-tests.trx`

**Mở với:**
- Visual Studio: Double-click file trx
- Text Editor: Notepad, VS Code
- Browser: Không được (file XML)

**Thông tin chứa trong TRX:**
- ✅ Tên test case
- ✅ Thời gian chạy
- ✅ Kết quả (PASSED, FAILED, SKIPPED)
- ✅ Stack trace nếu lỗi

#### 2. Báo Cáo Console Output

Khi chạy test, kết quả được in ra console:

```
Test summary: total: 19, failed: 0, succeeded: 19, skipped: 0, duration: 2m 45s
```

Giải thích:
- **total**: Tổng số test (19)
- **failed**: Số test thất bại (0 = tất cả thành công)
- **succeeded**: Số test thành công (19)
- **skipped**: Số test bị bỏ qua (0)
- **duration**: Tổng thời gian (2 phút 45 giây)

#### 3. Xem Chi Tiết Test Failed

Nếu test thất bại, console sẽ hiển thị:

```
FAILED: TC_ADDOWNER_001_WithValidData_AddsSuccessfully
Message: Condition was not met within 2.5 seconds.
Last exception: WebDriverException: Unable to locate required owner-management element.
```

**Nguyên nhân phổ biến:**
- Element không được tìm thấy (locator sai)
- Ứng dụng crash hoặc timeout
- WinAppDriver không kết nối được

---

## Gỡ Rối và Vấn Đề Thường Gặp

### Vấn Đề 1: Test Bị Skip

**Lỗi**:
```
Test summary: total: 4, failed: 0, succeeded: 0, skipped: 4, duration: 1.2s
```

**Nguyên nhân**: Biến môi trường `RUN_WINAPPDRIVER_TESTS` chưa được set

**Giải pháp**:
```powershell
$env:RUN_WINAPPDRIVER_TESTS = "true"
dotnet test ...
```

---

### Vấn Đề 2: WinAppDriver Connection Refused

**Lỗi**:
```
WebDriverException: Unexpected error. Unable to connect to the remote server
```

**Nguyên nhân**: WinAppDriver chưa khởi động hoặc địa chỉ sai

**Giải pháp**:
```powershell
# Bước 1: Mở terminal mới
WinAppDriver.exe 127.0.0.1 4723

# Bước 2: Chờ thấy "listening on http://127.0.0.1:4723/"

# Bước 3: Chạy test trong terminal khác
$env:RUN_WINAPPDRIVER_TESTS = "true"
dotnet test ...
```

---

### Vấn Đề 3: App Executable Not Found

**Lỗi**:
```
App executable not found at 'C:\...\QuanLyChungCu.exe'
```

**Nguyên nhân**: Ứng dụng chưa được build

**Giải pháp**:
```powershell
cd d:\Code\KiemThu\QuanLyChungCu_Final
dotnet build .\QuanLyChungCu\QuanLyChungCu.csproj -c Debug
```

---

### Vấn Đề 4: Element Not Found

**Lỗi**:
```
Unable to locate required owner-management element.
[Owner.SearchInput matches: 0, Owner.AddButton matches: 0]
```

**Nguyên nhân**: Control UI có AutomationId/Name khác

**Giải pháp**:
1. Kiểm tra lại tên control trong ứng dụng (XAML file)
2. Cập nhật locator trong `OwnerManagementPage.cs`
3. Chạy test lại

---

### Vấn Đề 5: Process Lock (Access Denied)

**Lỗi**:
```
The process cannot access the file '...\QuanLyChungCu.exe' because it is being used by another process
```

**Nguyên nhân**: Ứng dụng còn chạy từ test trước

**Giải pháp**:
```powershell
# Kết thúc tất cả process
Get-Process QuanLyChungCu | Stop-Process -Force
```

---

### Vấn Đề 6: Timeout Khi Tìm Element

**Lỗi**:
```
Condition was not met within 2.5 seconds.
```

**Nguyên nhân**:
- App chạy chậm
- Locator sai
- Element không visible

**Giải pháp**:
1. Tăng timeout trong code (nếu cần)
2. Thêm diagnostic output để debug
3. Kiểm tra lại AutomationId/Name

---

### Cách Debug Chi Tiết

Khi test thất bại, hãy kiểm tra diagnostic output:

```
[Search by Name with 'timkiem']
Element snapshot (Name | AutomationId | ClassName):
- Danh Sách Cư dân | | WPF.Control
- Thêm chủ hộ | Owner.AddButton | Button
- Nhập từ khóa... | Owner.SearchInput | TextBox
- Tìm Kiếm | Owner.SearchButton | Button
...
```

**Ý nghĩa:**
- Hiển thị tất cả element hiện có
- Giúp tìm thấy đúng AutomationId/Name
- Xác minh app đang ở màn hình nào

---

## Biến Môi Trường

### RUN_WINAPPDRIVER_TESTS

Quyết định có cho phép chạy UI test hay không

```powershell
$env:RUN_WINAPPDRIVER_TESTS = "true"   # Cho phép
$env:RUN_WINAPPDRIVER_TESTS = "false"  # Không cho phép (mặc định)
```

### WINAPPDRIVER_URL

Địa chỉ WinAppDriver (nếu sử dụng server khác)

```powershell
$env:WINAPPDRIVER_URL = "http://127.0.0.1:4723/"  # Mặc định
$env:WINAPPDRIVER_URL = "http://remote-ip:4723/"  # Server khác
```

### QLCC_APP_EXE

Đường dẫn tùy chỉnh đến QuanLyChungCu.exe

```powershell
$env:QLCC_APP_EXE = "C:\Custom\Path\QuanLyChungCu.exe"
```

---

## Tính Năng Nâng Cao

### Chạy Test Cụ Thể

```powershell
# Chỉ chạy test Thêm Chủ Hộ
dotnet test --filter "TestCategory=TC_ADDOWNER_001"

# Chạy tất cả test Thêm (TC_ADDOWNER_*)
dotnet test --filter "TestCategory~TC_ADDOWNER"

# Chạy test không được skip
dotnet test --filter "TestState!=Skipped"
```

### Tạo Báo Cáo Chi Tiết

```powershell
# Báo cáo TRX
dotnet test --logger "trx;LogFileName=detailed-report.trx"

# Báo cáo HTML (nếu có Liquid XML Studio)
dotnet test --logger "html;LogFileName=report.html"

# Báo cáo nhiều định dạng
dotnet test --logger "trx" --logger "console;verbosity=detailed"
```

### Chạy Test Với Breakpoint (Debug Mode)

```powershell
# Sử dụng Visual Studio debugger
# File → Open → QuanLyChungCu_Final.sln
# Set breakpoint trong LoginTests.cs
# Right-click test → Debug
```

---

## Các Lệnh Thường Dùng

```powershell
# 1. Build ứng dụng
dotnet build .\QuanLyChungCu\QuanLyChungCu.csproj -c Debug

# 2. Khởi động WinAppDriver (terminal riêng)
WinAppDriver.exe 127.0.0.1 4723

# 3. Chạy tất cả test
$env:RUN_WINAPPDRIVER_TESTS = "true"
dotnet test .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj

# 4. Chạy test cụ thể
dotnet test --filter "TestCategory=TC_ADDOWNER_001"

# 5. Kết thúc ứng dụng (nếu đông)
Get-Process QuanLyChungCu | Stop-Process -Force

# 6. Xem báo cáo
Start-Process .\QuanLyChungCu.Tests.UI\TestResults\ui-tests.trx
```

---

## Tài Liệu Liên Quan

- `QuanLyChungCu.Tests.UI/README.md` - Hướng dẫn tóm tắt
- `TestConfig.cs` - Cấu hình đường dẫn và URL
- `OwnerManagementPage.cs` - Các method tương tác UI
- `WinAppDriver GitHub` - https://github.com/Microsoft/WinAppDriver

---

## Hỗ Trợ

Nếu gặp vấn đề:

1. **Kiểm tra WinAppDriver**: Đang chạy trên `127.0.0.1:4723`?
2. **Kiểm tra Build**: Ứng dụng đã được build thành công?
3. **Kiểm tra Biến Môi Trường**: `RUN_WINAPPDRIVER_TESTS` đã set = `true`?
4. **Kiểm tra Diagnostic Output**: Xem element snapshot để hiểu app đang ở đâu
5. **Kết thúc Process**: Kill QuanLyChungCu.exe nếu bị lock

---

**Cập nhật lần cuối**: Tháng 3, 2026
**Phiên bản**: 1.0
**Trạng thái**: Hoàn thành (19 test case)

