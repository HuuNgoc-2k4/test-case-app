# Hướng Dẫn Cài Đặt và Chạy Dự Án QuanLyChungCu_Final

## 🎯 Mục Đích

Tài liệu này hướng dẫn **chi tiết từng bước** cách cài đặt dự án kiểm thử tự động cho ứng dụng Quản Lý Chung Cư, từ chuẩn bị môi trường hệ điều hành đến chạy test lần đầu tiên.

## 📌 Tóm Tắt Nhanh (Quick Start)

Nếu bạn đã quen với các công cụ, có thể thực hiện nhanh như sau:

```powershell
# 1. Cài .NET SDK 8, WinAppDriver, Git

# 2. Clone dự án
git clone https://github.com/YourUsername/QuanLyChungCu_Final.git
cd QuanLyChungCu_Final

# 3. Build app
dotnet build .\QuanLyChungCu\QuanLyChungCu.csproj -c Release

# 4. Khởi động WinAppDriver (Terminal 1)
WinAppDriver.exe 127.0.0.1 4723

# 5. Chạy test (Terminal 2)
$env:RUN_WINAPPDRIVER_TESTS = "true"
dotnet test .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj
```

---

## 📋 Yêu Cầu Ban Đầu

Trước khi bắt đầu, hãy chắc chắn:
- ✅ Máy tính chạy **Windows 10 hoặc 11**
- ✅ Kết nối Internet để tải công cụ
- ✅ Quyền quản trị viên (để cài phần mềm)
- ✅ Ít nhất **4GB RAM** (8GB khuyến khích)

---

## 🔧 Bước 1: Cài Đặt .NET SDK 8

### Tại Sao Cần .NET SDK?

**.NET SDK** là nền tảng chạy ứng dụng C# và dự án test của chúng ta.

### Cách Cài Đặt

**Cách 1: Tải từ Website Microsoft (Khuyến Khích)**

1. Mở trình duyệt, truy cập: https://dotnet.microsoft.com/download
2. Bạn sẽ thấy trang chọn phiên bản .NET
3. Tìm **.NET 8** (phiên bản mới nhất hoặc LTS)
4. Click **Download** (chọn phiên bản **x64** cho Windows 64-bit)
5. Chạy file installer `dotnet-sdk-8.x.xxx-win-x64.exe`
6. Chọn **Install**, đợi hoàn tất (khoảng 2-5 phút)
7. Nhấn **Close** khi xong

**Cách 2: Dùng Chocolatey (Nếu Đã Cài)**

Nếu máy đã cài Chocolatey, mở PowerShell với quyền admin:

```powershell
choco install dotnet-sdk
```

### Kiểm Tra Cài Đặt

Mở **PowerShell** và chạy:

```powershell
dotnet --version
```

**Kết quả mong đợi:**
```
8.0.x [os info]
```

Nếu thấy phiên bản 8.0.x hoặc mới hơn, tức là cài đặt thành công! ✅

---

## 🎮 Bước 2: Cài Đặt WinAppDriver

### Tại Sao Cần WinAppDriver?

**WinAppDriver** (Windows Application Driver) là công cụ giúp test tự động điều khiển giao diện ứng dụng desktop Windows (WPF, WinForms, v.v.).

### Cách Cài Đặt

**Phương Pháp 1: Tải từ GitHub (Khuyến Khích)**

1. Mở trình duyệt, truy cập: https://github.com/microsoft/WinAppDriver/releases
2. Tìm phiên bản mới nhất (ví dụ: **v1.2.3**) - thường ở trên cùng
3. Tìm **Assets** section, download file: `WindowsAppDriver-x64-...exe`
4. Chạy file installer
5. Chọn **Install**, chọn thư mục mặc định hoặc custom
6. Nhấn **Next** → **Finish** khi xong

**Phương Pháp 2: Dùng Winget (Windows 11)**

Nếu máy chạy Windows 11:

```powershell
winget install Microsoft.WindowsAppDriver
```

**Phương Pháp 3: Dùng Chocolatey**

```powershell
choco install winappdrivr
```

### Kiểm Tra Cài Đặt

Mở **PowerShell** và chạy:

```powershell
WinAppDriver.exe --help
```

**Kết quả mong đợi:**
```
Windows Application Driver v1.2.3
...
```

Hoặc tìm **WinAppDriver** trong Windows Start Menu → nhấp chuột phải → **Pin to Taskbar** để dễ tìm.

---

## 💾 Bước 3: Cài Đặt Git

### Tại Sao Cần Git?

**Git** là công cụ quản lý mã nguồn, giúp bạn tải (clone) dự án từ GitHub.

### Cách Cài Đặt

1. Truy cập: https://git-scm.com/
2. Click **Download for Windows** → chọn **64-bit**
3. Chạy installer
4. Chọn cài đặt mặc định, nhấn **Next** liên tục
5. Nhấn **Finish** khi xong

### Kiểm Tra Cài Đặt

Mở **PowerShell** hoặc **Command Prompt** và chạy:

```powershell
git --version
```

**Kết quả mong đợi:**
```
git version 2.x.x...
```

---

## 📥 Bước 4: Clone Dự Án từ GitHub

### Bước 4.1: Tạo Thư Mục Làm Việc

Mở **PowerShell** và chạy:

```powershell
# Tạo thư mục D:\Projects (hoặc chỗ khác tùy ý)
mkdir D:\Projects
cd D:\Projects
```

### Bước 4.2: Clone Repository

```powershell
# Thay "YourUsername" bằng username GitHub của bạn
git clone https://github.com/YourUsername/QuanLyChungCu_Final.git

# Vào thư mục dự án
cd QuanLyChungCu_Final
```

**Kết quả mong đợi:**
```
Cloning into 'QuanLyChungCu_Final'...
remote: Enumerating objects: 123, done.
...
Unpacking objects: 100% (123/123), done.
```

### Bước 4.3: Kiểm Tra Cấu Trúc

```powershell
# Liệt kê các file/thư mục
ls

# Hoặc dùng Windows Explorer để xem
explorer .
```

Bạn sẽ thấy:
```
📁 QuanLyChungCu/
📁 QuanLyChungCu.Tests.UI/
📄 QuanLyChungCu.sln
📄 README.md
📄 TESTING_PROJECT_GUIDE.md
...
```

---

## 🔨 Bước 5: Build Ứng Dụng Chính

### Tại Sao Cần Build?

**Build** là quá trình biên dịch mã nguồn C# thành file `.exe` có thể chạy được.

### Cách Build

Trong PowerShell, từ thư mục dự án:

```powershell
# Build phiên bản Release (nhanh hơn, ổn định hơn)
dotnet build .\QuanLyChungCu\QuanLyChungCu.csproj -c Release
```

**Quá trình:**
```
Determining projects to restore...
  Restored D:\Projects\QuanLyChungCu_Final\QuanLyChungCu\QuanLyChungCu.csproj
...
Build succeeded.
```

### Xác Nhận Build Thành Công

```powershell
# Kiểm tra file .exe có tồn tại không
Test-Path .\QuanLyChungCu\bin\Release\net8.0-windows\QuanLyChungCu.exe
```

**Kết quả mong đợi:**
```
True
```

---

## 🛠️ Bước 6: Restore NuGet Packages cho Dự Án Test

### Tại Sao?

**NuGet packages** là các thư viện bên ngoài cần thiết cho dự án test (MSTest, Appium, v.v.).

```powershell
# Restore packages
dotnet restore .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj
```

**Kết quả mong đợi:**
```
Restore succeeded.
```

---

## ⚙️ Bước 7: Cấu Hình Biến Môi Trường (Tùy Chọn)

### Khi Nào Cần?

- Nếu WinAppDriver chạy trên địa chỉ khác (không phải 127.0.0.1)
- Nếu file `QuanLyChungCu.exe` ở vị trí khác
- Nếu cần tùy chỉnh timeout

### Cách Cấu Hình

Mở PowerShell và chạy:

```powershell
# Bật chế độ chạy WinAppDriver tests (BẮT BUỘC)
$env:RUN_WINAPPDRIVER_TESTS = "true"

# (Tùy chọn) Nếu WinAppDriver không ở địa chỉ mặc định
$env:WINAPPDRIVER_URL = "http://127.0.0.1:4723/"

# (Tùy chọn) Nếu app.exe ở chỗ khác
$env:QLCC_APP_EXE = "D:\Projects\QuanLyChungCu_Final\QuanLyChungCu\bin\Release\net8.0-windows\QuanLyChungCu.exe"
```

⚠️ **LƯU Ý:** Các biến này chỉ tồn tại trong session PowerShell hiện tại. Khi đóng PowerShell, chúng sẽ mất. Bạn cần set lại lần tới.

---

## 🚀 Bước 8: Chạy Test Lần Đầu

### Chuẩn Bị

Bạn cần **2 terminal PowerShell chạy song song**:

1. **Terminal 1**: Chạy WinAppDriver Service
2. **Terminal 2**: Chạy Test

### Terminal 1: Khởi Động WinAppDriver

Mở PowerShell mới (hoặc cmd) và chạy:

```powershell
WinAppDriver.exe 127.0.0.1 4723
```

**Kết quả:**
```
WinAppDriver started listening on http://127.0.0.1:4723
```

⚠️ **QUAN TRỌNG**: Đừng đóng terminal này! Giữ nó chạy trong suốt quá trình test.

### Terminal 2: Chạy Test

Mở PowerShell mới khác, chạy:

```powershell
# Vào thư mục dự án
cd D:\Projects\QuanLyChungCu_Final

# Bật biến môi trường
$env:RUN_WINAPPDRIVER_TESTS = "true"

# Chạy test
dotnet test .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj --logger "console"
```

### Quá Trình Chạy

Bạn sẽ thấy:

```
Microsoft (R) Test Execution Command Line Tool Version 17.x
...
Starting test execution, please wait...

  Passed  AddOwner_AddsNewOwnerToList
  Passed  EditOwner_UpdatesOwnerInformation
  Passed  DeleteOwner_RemovesOwnerFromList
  Passed  SearchOwner_FiltersByKeyword

Test Run Successful.
Total tests: 4
Passed: 4
```

🎉 **Chúc mừng! Test chạy thành công!**

---

## 🔍 Các Lệnh Test Thường Dùng

### Chạy Toàn Bộ Test

```powershell
dotnet test .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj
```

### Chạy Một Test Riêng (Test Add)

```powershell
dotnet test .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj `
  --filter "FullyQualifiedName~AddOwner_AddsNewOwnerToList"
```

### Chạy Với Báo Cáo Chi Tiết

```powershell
dotnet test .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj `
  --logger "trx;LogFileName=test-results.trx" `
  -v detailed
```

### Chạy Test Nhiều Lần

```powershell
# Chạy 3 lần để kiểm tra ổn định
for ($i=1; $i -le 3; $i++) {
    Write-Host "Run $i..."
    dotnet test .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj
}
```

---

## ❌ Xử Lý Sự Cố

### Sự Cố 1: "Command 'dotnet' Not Found"

**Nguyên Nhân:** .NET SDK chưa được cài hoặc chưa reload PowerShell

**Cách Fix:**
```powershell
# Đóng PowerShell hiện tại
# Mở PowerShell mới

# Kiểm tra
dotnet --version
```

### Sự Cố 2: "Unable to connect to http://127.0.0.1:4723/"

**Nguyên Nhân:** WinAppDriver chưa chạy hoặc port bị chiếm

**Cách Fix:**
```powershell
# Kiểm tra WinAppDriver có chạy không
Get-Process | Select-String "WinAppDriver"

# Nếu không, khởi động
WinAppDriver.exe 127.0.0.1 4723

# Nếu port bị chiếm
netstat -ano | findstr :4723
# Kết quả: tcp ... LISTENING <PID>
# Kill process
taskkill /PID <PID> /F
```

### Sự Cố 3: "QuanLyChungCu.exe Lock (Access Denied)"

**Nguyên Nhân:** App vẫn đang chạy từ test trước

**Cách Fix:**
```powershell
# Kill app
taskkill /IM QuanLyChungCu.exe /F

# Chạy test lại
```

### Sự Cố 4: "Could Not Find Element"

**Nguyên Nhân:** Locator element sai hoặc UI thay đổi

**Cách Fix:**
1. Kiểm tra tên control trong ứng dụng WPF
2. Xem file page object tương ứng (LoginPage.cs, OwnerManagementPage.cs)
3. Update locator nếu UI đã thay đổi

---

## 📚 Bước Tiếp Theo

Sau khi cài đặt thành công, hãy:

1. **Đọc tài liệu chi tiết**: `README_DETAILED.md`
2. **Hiểu test cases**: `TESTING_PROJECT_GUIDE.md`
3. **Explore source code**: Xem `QuanLyChungCu.Tests.UI/` để hiểu cấu trúc test
4. **Modify tests**: Thêm test cases mới cho các feature khác

---

## 📞 Cần Giúp?

- 📖 Xem file `README_DETAILED.md` để hiểu chi tiết hơn
- 🐛 Tìm solution cho lỗi trong phần **Xử Lý Sự Cố** ở trên
- 💬 Mở Issue trên GitHub
- 📧 Liên hệ qua email

---

**Chúc bạn cài đặt thành công! 🚀**

