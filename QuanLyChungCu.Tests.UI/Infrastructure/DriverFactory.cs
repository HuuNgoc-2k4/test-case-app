using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace QuanLyChungCu.Tests.UI.Infrastructure;

internal static class DriverFactory
{
    public static WindowsDriver<WindowsElement> CreateDesktopSession()
    {
        const int maxAttempts = 3;
        Exception? lastException = null;

        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                var appiumOptions = new AppiumOptions();
                appiumOptions.PlatformName = "Windows";
                appiumOptions.AddAdditionalCapability("app", TestConfig.AppExecutablePath);

                var session = new WindowsDriver<WindowsElement>(TestConfig.WinAppDriverUri, appiumOptions);
                session.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(300);
                return session;
            }
            catch (Exception ex) when (attempt < maxAttempts)
            {
                lastException = ex;
                Thread.Sleep(120 * attempt);
            }
        }

        throw new InvalidOperationException("Unable to create WinAppDriver session after retries.", lastException);
    }
}
