# Test Case Summary - Tóm Tắt 19 Test Case

## 📊 Bảng Tóm Tắt

### Thêm Chủ Hộ (TC_ADDOWNER) - 6 Test Cases

| ID | Tên Test | Mục Tiêu | Dữ Liệu Input | Kết Quả Mong Đợi | Thời Gian |
|----|----|----|----|----|----|
| TC_ADDOWNER_001 | Thêm với dữ liệu hợp lệ | Kiểm thử thêm chủ hộ thành công | Tên: "Chu ho test 001", SĐT: "0901999999", Phòng: 105 | ✅ Chủ hộ được thêm, xuất hiện trong danh sách | ~5s |
| TC_ADDOWNER_002 | Lỗi số phòng không hợp lệ | Kiểm thử xác thực số phòng | Tên: "...", Phòng: "abc" | ❌ Thông báo: "Số phòng phải là số nguyên!" | ~5s |
| TC_ADDOWNER_003 | Lỗi SĐT không hợp lệ | Kiểm thử xác thực SĐT | Tên: "...", SĐT: "123" | ❌ Thông báo: "Số điện thoại phải có 10 chữ số..." | ~5s |
| TC_ADDOWNER_004 | Lỗi SĐT trùng lặp | Kiểm thử phát hiện trùng lặp SĐT | Tên: "...", SĐT: "0901000001" (hiện có) | ❌ Thông báo: "Số điện thoại đã tồn tại..." | ~5s |
| TC_ADDOWNER_005 | Lỗi số phòng trùng lặp | Kiểm thử phát hiện trùng lặp phòng | Tên: "...", Phòng: 101 (hiện có) | ❌ Thông báo: "Số phòng đã tồn tại..." | ~5s |
| TC_ADDOWNER_006 | Thêm dữ liệu đầy đủ | Kiểm thử thêm với tất cả field | Tên: "Chu ho test 006", SĐT: "0901666666", Phòng: 108 | ✅ Chủ hộ được thêm, không có chủ hộ nào khác | ~5s |

### Sửa Chủ Hộ (TC_EDITOWNER) - 4 Test Cases

| ID | Tên Test | Mục Tiêu | Bước | Kết Quả Mong Đợi | Thời Gian |
|----|----|----|----|----|----|
| TC_EDITOWNER_001 | Sửa tên chủ hộ | Kiểm thử sửa thông tin | Tìm "Chu ho sua" → Sửa tên → Lưu | ✅ Tên được cập nhật, xuất hiện trong danh sách | ~5s |
| TC_EDITOWNER_002 | Lỗi số phòng khi sửa | Kiểm thử xác thực khi sửa | Tìm "Chu ho sua" → Sửa phòng = "chu" → Lưu | ❌ Thông báo: "Số phòng phải là số nguyên!" | ~5s |
| TC_EDITOWNER_003 | Lỗi SĐT khi sửa | Kiểm thử xác thực SĐT khi sửa | Tìm "Chu ho sua" → Sửa SĐT = "999" → Lưu | ❌ Thông báo: "Số điện thoại không hợp lệ!" | ~5s |
| TC_EDITOWNER_004 | Sửa tất cả thông tin | Kiểm thử sửa nhiều field | Tìm "Chu ho sua" → Sửa toàn bộ → Lưu | ✅ Thông tin được cập nhật, xuất hiện trong danh sách | ~5s |

### Xóa Chủ Hộ (TC_DELOWNER) - 2 Test Cases

| ID | Tên Test | Mục Tiêu | Bước | Kết Quả Mong Đợi | Thời Gian |
|----|----|----|----|----|----|
| TC_DELOWNER_001 | Xóa và xác nhận | Kiểm thử xóa chủ hộ | Tìm "Chu ho xoa" → Click Xóa → Xác nhận "Có" | ✅ Chủ hộ bị xóa, không còn trong danh sách | ~5s |
| TC_DELOWNER_002 | Xóa nhưng hủy bỏ | Kiểm thử hủy xóa | Tìm "Chu ho khac" → Click Xóa → Click "Không" | ✅ Chủ hộ vẫn còn trong danh sách | ~5s |

### Tìm Kiếm Chủ Hộ (TC_SEARCH) - 7 Test Cases

| ID | Tên Test | Tiêu Chí Tìm | Input | Kết Quả Mong Đợi | Thời Gian |
|----|----|----|----|----|----|
| TC_SEARCH_001 | Tìm theo tên | Tên | "timkiem" | ✅ Hiển thị "Chu ho timkiem", ẩn các chủ hộ khác | ~3s |
| TC_SEARCH_002 | Tìm theo SĐT | Số điện thoại | "0901000003" | ✅ Hiển thị "Chu ho timkiem" (có SĐT này), ẩn khác | ~3s |
| TC_SEARCH_003 | Tìm theo số phòng | Số phòng | "103" | ✅ Hiển thị "Chu ho timkiem" (phòng 103), ẩn khác | ~3s |
| TC_SEARCH_004 | Tìm theo quê quán | Quê quán | "da nang" | ✅ Hiển thị "Chu ho timkiem" (quê Da Nang), ẩn khác | ~3s |
| TC_SEARCH_005 | Tìm theo ngày sinh | Ngày sinh | "03/03/1983" | ✅ Hiển thị "Chu ho timkiem" (sinh 03/03/1983), ẩn khác | ~3s |
| TC_SEARCH_006 | Tìm không có kết quả | Tìm keyword không tồn tại | "khongcokeyword" | ✅ Không hiển thị bất kỳ chủ hộ nào | ~3s |
| TC_SEARCH_007 | Xóa bộ lọc tìm kiếm | Clear search | Click "Xóa Lọc" | ✅ Hiển thị lại tất cả chủ hộ | ~3s |

---

## 📈 Thống Kê Test

```
┌─────────────────┬─────────┬──────────────────────────┐
│ Danh Mục        │ Số Test │ Tổng Thời Gian          │
├─────────────────┼─────────┼──────────────────────────┤
│ Thêm (ADD)      │    6    │ ~30 giây                 │
│ Sửa (EDIT)      │    4    │ ~20 giây                 │
│ Xóa (DELETE)    │    2    │ ~10 giây                 │
│ Tìm (SEARCH)    │    7    │ ~21 giây                 │
├─────────────────┼─────────┼──────────────────────────┤
│ TỔNG CỘNG       │   19    │ ~2-3 phút                │
└─────────────────┴─────────┴──────────────────────────┘
```

## 🎯 Mục Tiêu Kiểm Thử

### 1. Chức Năng Cơ Bản (Happy Path)
- ✅ Thêm chủ hộ mới
- ✅ Sửa thông tin chủ hộ
- ✅ Xóa chủ hộ (có xác nhận)
- ✅ Tìm kiếm chủ hộ (theo nhiều tiêu chí)

### 2. Xác Thực Dữ Liệu (Validation)
- ✅ Số phòng phải là số
- ✅ SĐT phải có 10 chữ số, bắt đầu bằng 0
- ✅ Không cho phép SĐT trùng lặp
- ✅ Không cho phép số phòng trùng lặp

### 3. Tính Năng Phụ (Edge Cases)
- ✅ Hủy xóa (Cancel delete)
- ✅ Tìm kiếm không có kết quả
- ✅ Xóa bộ lọc tìm kiếm

---

## 🔄 Vòng Đời Dữ Liệu Test

### Dữ Liệu Ban Đầu (Seeded)
```
Owners bị tạo sẵn:
1. "Chu ho sua" (0901000001, phòng 101) - Cho test Edit
2. "Chu ho xoa" (0901000002, phòng 102) - Cho test Delete
3. "Chu ho timkiem" (0901000003, phòng 103) - Cho test Search
4. "Chu ho khac" (0901000004, phòng 104) - Cho test Delete Cancel
5. "Test Owner 1-4" (0901000005-008, phòng 105-108) - Cho test Add validation
```

### Dữ Liệu Tạo Trong Test
```
Mỗi test TC_ADDOWNER tạo chủ hộ mới:
- TC_ADDOWNER_001: "Chu ho test 001"
- TC_ADDOWNER_006: "Chu ho test 006"
```

### Dữ Liệu Bị Xóa Trong Test
```
- TC_DELOWNER_001: Xóa "Chu ho xoa"
- TC_DELOWNER_002: Giữ lại "Chu ho khac"
```

---

## 🔍 Chi Tiết Từng Test

### TC_ADDOWNER_001 - Chi Tiết

```gherkin
Feature: Thêm Chủ Hộ Mới

  Scenario: Thêm chủ hộ với dữ liệu hợp lệ
    Given Đã đăng nhập với tài khoản admin
    And Đang ở màn hình danh sách chủ hộ
    When Click nút "Thêm chủ hộ"
    And Điền form:
      | Trường | Giá Trị |
      | Tên | Chu ho test 001 |
      | SĐT | 0901999999 |
      | Quê quán | Nam Dinh |
      | Ngày sinh | 05/05/1985 |
      | Số phòng | 105 |
    And Click nút "Thêm"
    Then Dialog đóng lại
    And Chủ hộ "Chu ho test 001" xuất hiện trong danh sách
    And Không có thông báo lỗi
```

---

## ⚙️ Cấu Hình Test

### Timeout (Giây)

| Hoạt Động | Timeout | Giải Thích |
|-----------|---------|-----------|
| Add Owner | 2.5s | Thêm chủ hộ nhanh |
| Edit Owner | 2.5s | Sửa thông tin nhanh |
| Delete Owner | 2.5s | Xóa nhanh |
| Search | 3.0s | Tìm kiếm có thể chậm hơn |
| Dialog | 4.0s | Chờ dialog xuất hiện |
| Navigate | 4.0s | Điều hướng màn hình |
| Login | 6.0s | Đăng nhập có thể lâu nhất |

### Dữ Liệu Login

```
Username: admin
Password: 1
Role: Admin (1)
```

---

## 📋 Kiểm Tra Trước Khi Chạy Test

- [ ] WinAppDriver đang chạy trên port 4723?
- [ ] App đã được build (QuanLyChungCu.exe tồn tại)?
- [ ] Biến môi trường `RUN_WINAPPDRIVER_TESTS = "true"`?
- [ ] Không có process `QuanLyChungCu.exe` đang chạy?
- [ ] .NET SDK phiên bản 8.0+?

---

## 📊 Phân Tích Chi Phí & Thời Gian

### Breakdown Thời Gian Chạy 19 Tests

```
Startup & Seeding:          ~15s
  - Build & launch app      ~5s
  - Seed test data          ~5s
  - Login                   ~5s

Test Execution:             ~2m 30s
  - 6 Add tests @ 5s each   ~30s
  - 4 Edit tests @ 5s each  ~20s
  - 2 Delete tests @ 5s     ~10s
  - 7 Search tests @ 3s     ~21s

Cleanup:                    ~10s
  - Kill process            ~3s
  - Close sessions          ~7s

TOTAL:                      ~2m 55s
```

---

**Cập Nhật**: Tháng 3, 2026
**Phiên Bản**: 1.0
**Trạng Thái**: ✅ Đầy Đủ (19/19 Test Cases)

