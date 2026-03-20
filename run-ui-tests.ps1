# Script to run UI tests with WinAppDriver enabled
# This script sets the required environment variable and runs the tests

param(
    [string]$TestCategory = $null
)

$env:RUN_WINAPPDRIVER_TESTS = "true"

Write-Host "Environment variable RUN_WINAPPDRIVER_TESTS set to: $($env:RUN_WINAPPDRIVER_TESTS)"
Write-Host ""

$testCommand = "dotnet test .\QuanLyChungCu.Tests.UI\QuanLyChungCu.Tests.UI.csproj --logger `"trx;LogFileName=ui-tests.trx`""

if ($TestCategory) {
    $testCommand += " --filter `"TestCategory=$TestCategory`""
    Write-Host "Running tests for category: $TestCategory"
}
else {
    Write-Host "Running all UI tests..."
}

Write-Host ""
Invoke-Expression $testCommand

