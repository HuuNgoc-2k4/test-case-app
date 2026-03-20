#!/usr/bin/env powershell
# Quick Setup Guide - Hướng Dẫn Thiết Lập Nhanh

Write-Host "=== QuanLyChungCu.Tests.UI - Quick Setup ===" -ForegroundColor Green
Write-Host ""

# Kiểm tra điều kiện tiên quyết
Write-Host "📋 Kiểm tra yêu cầu hệ thống..." -ForegroundColor Blue

$checks = @{
    ".NET 8" = { $ver = dotnet --version; [version]$ver -ge [version]"8.0" }
    "WinAppDriver" = { Get-Command WinAppDriver -ErrorAction SilentlyContinue }
}

foreach ($check in $checks.GetEnumerator()) {
    try {
        if (& $check.Value) {
            Write-Host "  ✅ $($check.Key)" -ForegroundColor Green
        } else {
            Write-Host "  ❌ $($check.Key)" -ForegroundColor Red
        }
    } catch {
        Write-Host "  ❌ $($check.Key)" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "🔧 Các bước thiết lập:" -ForegroundColor Blue
Write-Host ""

Write-Host "1️⃣  Build ứng dụng:" -ForegroundColor Cyan
Write-Host '   dotnet build .\QuanLyChungCu\QuanLyChungCu.csproj' -ForegroundColor White
Write-Host ""

Write-Host "2️⃣  Khởi động WinAppDriver (Terminal riêng):" -ForegroundColor Cyan
Write-Host '   WinAppDriver.exe 127.0.0.1 4723' -ForegroundColor White
Write-Host ""

Write-Host "3️⃣  Set biến môi trường:" -ForegroundColor Cyan
Write-Host '   $env:RUN_WINAPPDRIVER_TESTS = "true"' -ForegroundColor White
Write-Host ""

Write-Host "4️⃣  Chạy test:" -ForegroundColor Cyan
Write-Host '   .\run-ui-tests.ps1' -ForegroundColor White
Write-Host '   # hoặc chạy test cụ thể:' -ForegroundColor Gray
Write-Host '   .\run-ui-tests.ps1 -TestCategory TC_ADDOWNER_001' -ForegroundColor Gray
Write-Host ""

Write-Host "📚 Tài liệu:" -ForegroundColor Blue
Write-Host "  • README.md - Khởi động nhanh" -ForegroundColor White
Write-Host "  • TESTING_PROJECT_GUIDE.md - Hướng dẫn chi tiết đầy đủ" -ForegroundColor White
Write-Host ""

Write-Host "💡 Lệnh hữu ích:" -ForegroundColor Blue
Write-Host '  # Chạy tất cả test' -ForegroundColor Gray
Write-Host '  $env:RUN_WINAPPDRIVER_TESTS = "true"; dotnet test .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj' -ForegroundColor White
Write-Host ''
Write-Host '  # Chạy test cụ thể' -ForegroundColor Gray
Write-Host '  dotnet test --filter "TestCategory=TC_ADDOWNER_001"' -ForegroundColor White
Write-Host ''
Write-Host '  # Kết thúc app nếu bị lock' -ForegroundColor Gray
Write-Host '  Get-Process QuanLyChungCu | Stop-Process -Force' -ForegroundColor White
Write-Host ""

Write-Host "🎯 Trạng thái: 19/19 Test Case Hoàn Thành" -ForegroundColor Green
Write-Host ""

