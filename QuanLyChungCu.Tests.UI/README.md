# QuanLyChungCu.Tests.UI

Bo kiem thu UI hop den cho app WPF `QuanLyChungCu` bang WinAppDriver + MSTest.

## 1. Yeu cau
- Windows 10/11.
- .NET SDK 8.
- WinAppDriver da cai dat.

## 2. Chuan bi
1. Build app:
```powershell
cd d:\Code\KiemThu\QuanLyChungCu_Final
dotnet build .\QuanLyChungCu\QuanLyChungCu.csproj
```

2. Start WinAppDriver (mot terminal rieng):
```powershell
WinAppDriver.exe 127.0.0.1 4723
```

3. Bat co chay UI test:
```powershell
$env:RUN_WINAPPDRIVER_TESTS = "true"
```

## 3. Chay test
```powershell
cd d:\Code\KiemThu\QuanLyChungCu_Final
dotnet test .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj --logger "trx;LogFileName=ui-tests.trx"
```

## 4. Bien moi truong tuy chon
- `WINAPPDRIVER_URL`: URL service WinAppDriver. Mac dinh: `http://127.0.0.1:4723/`.
- `QLCC_APP_EXE`: duong dan tuy chinh toi file `QuanLyChungCu.exe`.
- `RUN_WINAPPDRIVER_TESTS`: dat `true` de test UI duoc phep chay.

## 5. Luu y toc do va cleanup
- Wait da duoc rut ngan theo huong explicit wait + polling ngan de giam tong thoi gian chay.
- Moi test se dong session va kill `QuanLyChungCu.exe` trong cleanup de tranh lock process.
- Khi timeout, test se in them element snapshot (Name/AutomationId/ClassName) de debug locator nhanh hon.

## 6. Tai lieu day du
- Xem file `TESTING_PROJECT_GUIDE.md` de doc toan bo: pham vi kiem thu, cau truc, quy trinh chay, cach doc bao cao, va huong debug loi.
