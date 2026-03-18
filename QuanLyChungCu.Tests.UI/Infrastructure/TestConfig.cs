using System;
using System.IO;

namespace QuanLyChungCu.Tests.UI.Infrastructure;

internal static class TestConfig
{
    public static readonly Uri WinAppDriverUri =
        new(Environment.GetEnvironmentVariable("WINAPPDRIVER_URL") ?? "http://127.0.0.1:4723/");

    public static string AppExecutablePath
    {
        get
        {
            string? configuredPath = Environment.GetEnvironmentVariable("QLCC_APP_EXE");
            if (!string.IsNullOrWhiteSpace(configuredPath))
            {
                return configuredPath;
            }

            return Path.GetFullPath(Path.Combine(
                AppContext.BaseDirectory,
                "..", "..", "..", "..",
                "QuanLyChungCu",
                "bin", "Debug", "net8.0-windows", "QuanLyChungCu.exe"));
        }
    }

    public static string AppBinaryDirectory => Path.GetDirectoryName(AppExecutablePath) ?? string.Empty;
}
