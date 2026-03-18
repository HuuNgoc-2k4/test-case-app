# Hướng Dẫn Đọc và Phân Tích Báo Cáo Kiểm Thử

## 🎯 Mục Đích

Tài liệu này giúp bạn hiểu và phân tích **báo cáo test** từ dự án kiểm thử tự động `QuanLyChungCu_Final`, bao gồm:
- Cách đọc console output
- Cách mở file báo cáo TRX
- Cách phân tích kết quả test (Pass/Fail/Timeout)
- Cách hiểu và sử dụng thông tin debug từ log

---

## 📊 1. Các Loại Báo Cáo

Sau khi chạy test, bạn sẽ nhận được:

| Loại Báo Cáo | Định Dạng | Nơi Lưu | Mục Đích |
|-------------|----------|--------|---------|
| **Console Output** | Text in terminal | Terminal | Xem kết quả nhanh |
| **TRX File** | XML (.trx) | Project root | Chi tiết đầy đủ mỗi test |
| **Log File** (tùy chọn) | Text (.log) | Project root | Thông tin debug chi tiết |

---

## 📺 2. Đọc Console Output

### Khi Chạy Test

Bạn sẽ thấy output như sau:

```powershell
Microsoft (R) Test Execution Command Line Tool Version 17.x

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Starting execution of test project: D:\Projects\QuanLyChungCu_Final\...

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
Warnings: 0

Test run duration: 46.146 Seconds
```

### Giải Thích Chi Tiết

**Tiêu đề:**
```
Microsoft (R) Test Execution Command Line Tool Version 17.x
```
- Phiên bản công cụ test runner

**Thông tin chung:**
```
A total of 1 test files matched the specified pattern.
Starting execution of test project: D:\...
```
- Số lượng file test tìm thấy
- Đường dẫn file test được chạy

**Kết Quả Từng Test:**
```
  Passed  AddOwner_AddsNewOwnerToList (Duration: 15.234s)
```
- ✅ **Passed**: Test thành công
- ❌ **Failed**: Test thất bại
- ⏭️ **Skipped**: Test bị bỏ qua
- 📋 Tên test
- ⏱️ Thời gian chạy (ms hoặc s)

**Kết Quả Tổng Hợp:**
```
Test Run Completed
Total tests: 4          # Tổng số test
Passed: 2              # Test thành công
Failed: 1              # Test thất bại
Skipped: 1             # Test bị bỏ qua
Warnings: 0            # Cảnh báo (nếu có)
```

### Các Ký Hiệu Phổ Biến

| Ký Hiệu | Ý Nghĩa | Ví Dụ |
|--------|--------|-------|
| ✅ **P** hay **Passed** | Test thành công | ✅ AddOwner_AddsNewOwnerToList |
| ❌ **F** hay **Failed** | Test thất bại | ❌ DeleteOwner_RemovesOwnerFromList |
| ⏭️ **S** hay **Skipped** | Test bị bỏ qua | ⏭️ SearchOwner_FiltersByKeyword |
| 🔸 **W** hay **Warning** | Cảnh báo (không fail) | (hiếm gặp) |

---

## 📄 3. Mở và Đọc File Báo Cáo TRX

### Tệp TRX Là Gì?

**TRX** (Test Results eXtension) là file XML chứa thông tin chi tiết về kết quả test, bao gồm:
- Tên từng testcase
- Kết quả (Pass/Fail/Skip)
- Thời gian chạy
- Thông báo lỗi
- Stack trace (ngăn xếp lỗi)

### Các Cách Mở File TRX

#### Cách 1: Mở Với Visual Studio (Khuyến Khích)

1. Mở **Visual Studio**
2. Vào menu `Test` → `Test Explorer`
3. Click biểu tượng 📂 **Open Test Results File** (hoặc `File` → `Open`)
4. Chọn file `.trx` (ví dụ: `test-results.trx`)
5. Visual Studio sẽ hiển thị báo cáo chi tiết

**Lợi Ích:** Giao diện đẹp, dễ đọc, có tính năng lọc

#### Cách 2: Mở Với Visual Studio Code

1. Mở **Visual Studio Code**
2. Cài extension **"Test Results Viewer"** (hoặc tìm `trx` viewer)
3. Nhấp chuột phải file `.trx` → `Open with` → `Test Results Viewer`

#### Cách 3: Mở Bằng Trình Duyệt Web

1. Nhấp chuột phải file `.trx`
2. Chọn `Open with` → **Microsoft Edge** hoặc **Chrome**
3. File sẽ hiển thị dạng HTML

#### Cách 4: Xem Trực Tiếp File XML

Mở file `.trx` bằng **Notepad** hoặc **VS Code** để xem nội dung XML:

```xml
<?xml version="1.0" encoding="utf-8"?>
<TestRun name="..." started="2026-03-18T10:30:00" ...>
  <Results>
    <UnitTestResult testName="AddOwner_AddsNewOwnerToList" 
                    outcome="Passed" 
                    duration="00:00:15.234">
    </UnitTestResult>
    
    <UnitTestResult testName="DeleteOwner_RemovesOwnerFromList" 
                    outcome="Failed" 
                    duration="00:00:12.345">
      <Output>
        <ErrorInfo>
          <Message>OpenQA.Selenium.NoSuchElementException...</Message>
          <StackTrace>...</StackTrace>
        </ErrorInfo>
      </Output>
    </UnitTestResult>
  </Results>
</TestRun>
```

### Cấu Trúc File TRX

```
<TestRun>
  ├── Thông tin chung (date, time, duration)
  └── <Results>
      ├── <UnitTestResult> Test 1
      │   ├── @testName: Tên test
      │   ├── @outcome: Kết quả (Passed/Failed/NotRunnable)
      │   ├── @duration: Thời gian chạy
      │   └── <Output>: Thông báo lỗi/chi tiết
      │
      ├── <UnitTestResult> Test 2
      │   ...
```

---

## 📈 4. Phân Tích Kết Quả Test

### Khi Test Thành Công (✅ PASSED)

**Console Output:**
```
Passed AddOwner_AddsNewOwnerToList (Duration: 15.234s)
```

**Điều Này Có Nghĩa:**
- ✅ Tất cả assertion/kiểm thử đều đúng
- ✅ Không có exception
- ✅ Luồng test hoàn tất đúng như kỳ vọng
- ✅ Element UI được tìm thấy và tương tác thành công

**Hành Động:** Không cần làm gì! Test pass là tốt 🎉

---

### Khi Test Thất Bại (❌ FAILED)

**Console Output:**
```
Failed DeleteOwner_RemovesOwnerFromList (Duration: 12.345s)
  Error: OpenQA.Selenium.NoSuchElementException
  Could not locate element with name: "btnDelete" or AutomationId: "btnDelete"
  
  at OpenQA.Selenium.WebDriverWait.Until(Func`1 condition)
  at QuanLyChungCu.Tests.UI.Pages.OwnerManagementPage.ClickDeleteButton()
```

**Giải Thích:**
- ❌ Test thất bại
- ❌ Lý do: Element "btnDelete" không tìm thấy
- ❌ Nơi lỗi: Method `ClickDeleteButton()` trong `OwnerManagementPage.cs`

**Nguyên Nhân Thường Gặp:**

| Lỗi | Nguyên Nhân | Cách Fix |
|-----|-----------|---------|
| `NoSuchElementException` | Element không tìm thấy | Kiểm tra locator, update tên trong UI |
| `TimeoutException` | Chờ element quá lâu (> timeout) | Tăng timeout hoặc fix logic app |
| `StaleElementReferenceException` | Element bị reload sau find | Tìm lại element trong test |
| `InvalidOperationException` | Element tồn tại nhưng không enabled | Check xem control có bị disable không |
| `AssertionFailedError` | Kết quả khác với kỳ vọng | Check dữ liệu test hoặc logic app |

**Hành Động:** Xem phần "Xử Lý Lỗi" dưới đây

---

### Khi Test Bị Skip (⏭️ SKIPPED)

**Console Output:**
```
Skipped SearchOwner_FiltersByKeyword
```

**Điều Này Có Nghĩa:**
- ⏭️ Test không được chạy (do lý do nào đó)
- Có thể do: `[Ignore]` attribute, điều kiện `Assume.That()` không thỏa, test setup fail

**Hành Động:** Kiểm tra lý do skip trong code test

---

### Khi Test Timeout (⏱️ TIMEOUT)

**Console Output:**
```
Failed SearchOwner_FiltersByKeyword (Duration: 30.001s)
  Error: OpenQA.Selenium.WebDriverTimeoutException
  Timeout after 10000ms waiting for element
```

**Điều Này Có Nghĩa:**
- ❌ Test chạy quá lâu (trên 10 giây)
- ❌ Element không xuất hiện trong khoảng thời gian chờ
- ❌ App có thể bị lag, network chậm, hoặc locator sai

**Hành Động:**
1. Tăng timeout: `$env:WINAPPDRIVER_TIMEOUT_MS = "20000"`
2. Kiểm tra app performance (CPU, RAM)
3. Kiểm tra network
4. Verify locator element

---

## 🔍 5. Sử Dụng Thông Tin Debug từ Log

### Kích Hoạt Verbose Logging

Chạy test với verbose để xem chi tiết nhất:

```powershell
dotnet test .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj `
  -v detailed `
  --logger "console"
```

**Output:**
```
Starting test execution
  OwnerManagementTests.AddOwner_AddsNewOwnerToList [18:45:32]
    [Setup] Initializing WinAppDriver session...
    [Step 1] Navigating to Login page
    [Step 2] Entering username: admin
    [Step 3] Entering password: admin
    [Step 4] Clicking Login button
    [Wait] Waiting for Main window to appear (timeout: 5000ms)
    [Step 5] Clicking Add button
    [ERROR] NoSuchElementException: Could not locate btnAdd
    [Debug] Element tree snapshot:
      - Window: "QuanLyChungCu"
      - Grid: "gridOwners" 
      - Button: "btnEdit"
      - Button: "btnDelete"
    [Cleanup] Quit session
  Test FAILED (Duration: 15.234s)
```

### Phân Tích Debug Info

**Setup Phase:**
```
[Setup] Initializing WinAppDriver session...
[Setup] Connecting to http://127.0.0.1:4723/
```
- Kiểm tra WinAppDriver service có chạy không
- Kiểm tra port 4723 có bị chiếm không

**Step-by-Step Execution:**
```
[Step 1] Navigating to Login page
[Step 2] Entering username: admin
```
- Theo dõi từng bước test
- Xác định bước nào fail

**Wait & Element Location:**
```
[Wait] Waiting for Main window to appear (timeout: 5000ms)
[Element] Found button: btnAdd (AutomationId: "AddButton")
```
- Kiểm tra element có tìm thấy không
- Kiểm tra thời gian chờ

**Error & Stack Trace:**
```
[ERROR] NoSuchElementException: Could not locate btnAdd
[Location] In OwnerManagementPage.ClickAddButton() line 45
[Cause] Element was not found within timeout
```
- Lý do lỗi
- Vị trí trong code
- Cách khắc phục

**Element Tree Dump (Khi Timeout):**
```
[Debug] Element tree snapshot:
  - Window: "QuanLyChungCu" (AutomationId: "MainWindow")
    - Grid: "gridOwners" 
    - TextBox: "txtSearch"
    - Button: "btnEdit"
    - Button: "btnDelete"
```
- Danh sách element thực tế WinAppDriver nhìn thấy
- So sánh với locator mong muốn để tìm khác biệt

---

## 📊 6. Tạo Báo Cáo Tóm Tắt

### Lệnh Tạo Báo Cáo

```powershell
# Tạo báo cáo TRX chi tiết
dotnet test .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj `
  --logger "trx;LogFileName=test-report-$(Get-Date -Format 'yyyy-MM-dd-HHmmss').trx"
```

### Phân Tích Báo Cáo Toàn Bộ

Sau khi chạy toàn bộ test, tạo tóm tắt:

**Template Báo Cáo:**

```
┌─────────────────────────────────────────────────────────┐
│          BÁO CÁO KIỂM THỬ (Test Report)                │
└─────────────────────────────────────────────────────────┘

Ngày chạy: 18/03/2026
Thời gian: 10:30:00 AM
Tổng thời gian: 46 giây

┌─────────────────────────────────────────────────────────┐
│ KẾT QUẢ TỔNG HỢP                                        │
├─────────────────────────────────────────────────────────┤
│ Tổng test:      4
│ ✅ Pass:        2 (50%)
│ ❌ Fail:        1 (25%)
│ ⏭️ Skip:        1 (25%)
│ Pass Rate:      50%
│ Status:         ⚠️ NEEDS ATTENTION
└─────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────┐
│ CHI TIẾT TỪNG TEST                                      │
├─────────────────────────────────────────────────────────┤
│
│ 1. AddOwner_AddsNewOwnerToList
│    Status:   ✅ PASSED
│    Time:     15.234s
│    Notes:    OK
│
│ 2. EditOwner_UpdatesOwnerInformation
│    Status:   ✅ PASSED
│    Time:     18.567s
│    Notes:    OK
│
│ 3. DeleteOwner_RemovesOwnerFromList
│    Status:   ❌ FAILED
│    Time:     12.345s
│    Error:    NoSuchElementException: btnDelete not found
│    Location: OwnerManagementPage.cs line 45
│    Action:   FIX NEEDED - Verify button locator in app
│
│ 4. SearchOwner_FiltersByKeyword
│    Status:   ⏭️ SKIPPED
│    Time:     0s
│    Reason:   [Ignore] attribute
│    Action:   Remove [Ignore] to run
│
└─────────────────────────────────────────────────────────┘

KHUYẾN NGHỊ:
✓ Fix lỗi DeleteOwner_RemovesOwnerFromList
✓ Enable SearchOwner_FiltersByKeyword test
✓ Chạy lại sau khi fix
```

---

## 💡 7. Tips & Tricks

### Tip 1: Chạy Test và Lưu Báo Cáo Cùng Lúc

```powershell
$timestamp = Get-Date -Format 'yyyy-MM-dd-HHmmss'
dotnet test .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj `
  --logger "trx;LogFileName=report-$timestamp.trx" `
  --logger "console"
```

### Tip 2: So Sánh Kết Quả Giữa 2 Lần Chạy

```powershell
# Lần 1
dotnet test ... --logger "trx;LogFileName=run-1.trx"

# Lần 2
dotnet test ... --logger "trx;LogFileName=run-2.trx"

# Mở cả 2 file .trx để so sánh
```

### Tip 3: Chạy Lặp Để Kiểm Tra Ổn Định

```powershell
# Chạy 5 lần để kiểm tra flaky test
for ($i=1; $i -le 5; $i++) {
    Write-Host "Run $i..."
    dotnet test .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj
}
```

### Tip 4: Export Báo Cáo Ra CSV

Dùng PowerShell để parse TRX và export CSV:

```powershell
[xml]$trx = Get-Content "test-results.trx"
$trx.TestRun.Results.UnitTestResult | 
  Select-Object @{n='TestName'; e={$_.testName}}, 
                @{n='Outcome'; e={$_.outcome}}, 
                @{n='Duration'; e={$_.duration}} |
  Export-Csv "test-results.csv" -NoTypeInformation
```

---

## 🎯 8. Action Items After Each Test Run

### Nếu Tất Cả Pass ✅

```
□ Xem quá trình lưu báo cáo
□ Update file CHANGELOG với kết quả
□ Commit nếu cần
□ Deploy hoặc release phiên bản tiếp theo
```

### Nếu Có Fail ❌

```
□ Ghi lại lỗi chi tiết
□ Phân tích nguyên nhân (Locator sai? UI thay đổi? App bug?)
□ Tạo Issue trên GitHub nếu cần
□ Sửa test hoặc app
□ Chạy lại để xác nhận fix
□ Update test report
```

### Nếu Có Timeout ⏱️

```
□ Kiểm tra app performance
□ Kiểm tra network
□ Tăng timeout nếu cần
□ Kiểm tra locator element
□ Chạy lại
```

---

## 📚 Tài Liệu Liên Quan

- 📄 `README_DETAILED.md` - Hướng dẫn chi tiết về dự án
- 📄 `INSTALLATION_GUIDE.md` - Hướng dẫn cài đặt
- 📄 `TESTING_PROJECT_GUIDE.md` - Hướng dẫn kiểm thử
- 📁 `QuanLyChungCu.Tests.UI/` - Source code test

---

**Happy Testing! 🚀**

