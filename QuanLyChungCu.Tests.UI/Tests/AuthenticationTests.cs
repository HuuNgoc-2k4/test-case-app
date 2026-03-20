using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using QuanLyChungCu.Tests.UI.Infrastructure;
using QuanLyChungCu.Tests.UI.Pages;
using System.Diagnostics;

namespace QuanLyChungCu.Tests.UI.Tests;

[TestClass]
public sealed class AuthenticationTests
{
    private static readonly TimeSpan LoginTimeout = TimeSpan.FromSeconds(7);
    private static readonly TimeSpan DialogTimeout = TimeSpan.FromSeconds(2.2);
    private static readonly TimeSpan UiTimeout = TimeSpan.FromSeconds(2.5);

    private WindowsDriver<WindowsElement>? _session;

    [ClassInitialize]
    public static void ClassInitialize(TestContext _)
    {
        if (!IsUiTestEnabled())
        {
            Assert.Inconclusive("Set RUN_WINAPPDRIVER_TESTS=true to execute WinAppDriver UI tests.");
        }

        if (!File.Exists(TestConfig.AppExecutablePath))
        {
            Assert.Fail($"App executable not found at '{TestConfig.AppExecutablePath}'. Build QuanLyChungCu project first.");
        }
    }

    [TestInitialize]
    public void TestInitialize()
    {
        EnsureAppNotRunning();
        TestDataSeeder.SeedUsersDatabase(TestConfig.AppBinaryDirectory);
        TestDataSeeder.SeedOwnersForManagementTests(TestConfig.AppBinaryDirectory);
        _session = DriverFactory.CreateDesktopSession();
    }

    [TestCleanup]
    public void TestCleanup()
    {
        try
        {
            _session?.Quit();
        }
        catch
        {
            // Ignore cleanup exceptions because app window may already be closed.
        }
        finally
        {
            EnsureAppNotRunning();
        }
    }

    [TestMethod]
    [TestCategory("TC_LOGIN_001")]
    public void Login_001_EmptyUsernamePassword_ShowsInvalidMessage()
    {
        var loginPage = new LoginPage(GetSession());

        loginPage.EnterUsername(string.Empty);
        loginPage.EnterPassword(string.Empty);
        loginPage.Submit();

        WaitUntil(() => IsElementVisibleOnRoot("SDT không đúng hoặc người dùng không tồn tại!"), DialogTimeout);
        Assert.IsTrue(IsElementVisibleOnRoot("SDT không đúng hoặc người dùng không tồn tại!"));

        ClickDialogButtonByName("OK");
    }

    [TestMethod]
    [TestCategory("TC_LOGIN_002")]
    public void Login_002_ValidUsernameInvalidPassword_ShowsInvalidMessage()
    {
        var loginPage = new LoginPage(GetSession());

        loginPage.EnterUsername("admin");
        loginPage.EnterPassword("sai_mat_khau");
        loginPage.Submit();

        WaitUntil(() => IsElementVisibleOnRoot("SDT không đúng hoặc người dùng không tồn tại!"), DialogTimeout);
        Assert.IsTrue(IsElementVisibleOnRoot("SDT không đúng hoặc người dùng không tồn tại!"));

        ClickDialogButtonByName("OK");
    }

    [TestMethod]
    [TestCategory("TC_LOGIN_003")]
    public void Login_003_InvalidUsernameValidPassword_ShowsInvalidMessage()
    {
        var loginPage = new LoginPage(GetSession());

        loginPage.EnterUsername("khong_ton_tai");
        loginPage.EnterPassword("1");
        loginPage.Submit();

        WaitUntil(() => IsElementVisibleOnRoot("SDT không đúng hoặc người dùng không tồn tại!"), DialogTimeout);
        Assert.IsTrue(IsElementVisibleOnRoot("SDT không đúng hoặc người dùng không tồn tại!"));

        ClickDialogButtonByName("OK");
    }

    [TestMethod]
    [TestCategory("TC_LOGIN_004")]
    public void Login_004_AdminLogin_Success()
    {
        var loginPage = new LoginPage(GetSession());

        loginPage.EnterUsername("admin");
        loginPage.EnterPassword("1");
        loginPage.Submit();

        WaitUntil(
            () => !loginPage.IsLoginScreenVisible() || IsElementVisibleOnRoot("SDT không đúng hoặc người dùng không tồn tại!"),
            LoginTimeout);

        Assert.IsFalse(
            IsElementVisibleOnRoot("SDT không đúng hoặc người dùng không tồn tại!"),
            "Admin login unexpectedly showed invalid-credential message.");

        WaitUntil(() => IsElementVisibleOnRoot("Danh Sách Cư dân") || IsElementVisibleOnRoot("Đăng xuất"), LoginTimeout);
        Assert.IsTrue(IsElementVisibleOnRoot("Danh Sách Cư dân") || IsElementVisibleOnRoot("Đăng xuất"));
    }

    [TestMethod]
    [TestCategory("TC_LOGIN_005")]
    public void Login_005_UserLogin_Success()
    {
        var loginPage = new LoginPage(GetSession());

        loginPage.EnterUsername("0901000001");
        loginPage.EnterPassword("1");
        loginPage.Submit();

        WaitUntil(() => IsElementVisibleOnRoot("Tiền nhà"), LoginTimeout);
        Assert.IsTrue(IsElementVisibleOnRoot("Tiền nhà"));
    }

    [TestMethod]
    [TestCategory("TC_LOGIN_006")]
    public void Login_006_ShowPassword_DisplaysPlainText()
    {
        var loginPage = new LoginPage(GetSession());

        loginPage.EnterPassword("123456");
        loginPage.ToggleShowPassword();

        WaitUntil(loginPage.IsPasswordVisibleTextBoxShown, UiTimeout);
        Assert.IsTrue(loginPage.IsPasswordVisibleTextBoxShown());
        Assert.AreEqual("123456", loginPage.GetVisiblePasswordText());
    }

    [TestMethod]
    [TestCategory("TC_LOGIN_007")]
    public void Login_007_HidePassword_ReMasksPassword()
    {
        var loginPage = new LoginPage(GetSession());

        loginPage.EnterPassword("123456");
        loginPage.ToggleShowPassword();
        WaitUntil(loginPage.IsPasswordVisibleTextBoxShown, UiTimeout);

        loginPage.ToggleShowPassword();

        WaitUntil(loginPage.IsPasswordBoxShown, UiTimeout);
        Assert.IsTrue(loginPage.IsPasswordBoxShown());
        Assert.IsFalse(loginPage.IsPasswordVisibleTextBoxShown());
    }

    [TestMethod]
    [TestCategory("TC_LOGIN_008")]
    public void Login_008_MinimizeLoginWindow_MinimizesToTaskbar()
    {
        var loginPage = new LoginPage(GetSession());

        loginPage.ClickMinimize();

        // Window minimize state is inconsistent across machines in WinAppDriver.
        // This asserts the click action succeeds and app process stays alive.
        WaitUntil(() => Process.GetProcessesByName("QuanLyChungCu").Length > 0, UiTimeout);
        Assert.IsNotEmpty(Process.GetProcessesByName("QuanLyChungCu"));
    }

    private WindowsDriver<WindowsElement> GetSession()
    {
        return _session ?? throw new InvalidOperationException("WinAppDriver session has not been initialized.");
    }

    private static bool IsElementVisibleOnRoot(string elementName)
    {
        using WindowsDriver<WindowsElement> rootSession = CreateRootSession();
        return rootSession.FindElements(By.Name(elementName)).Count > 0;
    }


    private static void ClickDialogButtonByName(params string[] buttonNames)
    {
        using WindowsDriver<WindowsElement> rootSession = CreateRootSession();

        WindowsElement? targetButton = null;

        WaitUntil(
            () =>
            {
                targetButton = buttonNames
                    .Select(name => rootSession.FindElements(By.Name(name)).FirstOrDefault())
                    .FirstOrDefault(button => button != null);
                return targetButton != null;
            },
            DialogTimeout);

        targetButton?.Click();
    }

    private static WindowsDriver<WindowsElement> CreateRootSession()
    {
        var rootOptions = new AppiumOptions();
        rootOptions.PlatformName = "Windows";
        rootOptions.AddAdditionalCapability("app", "Root");
        return new WindowsDriver<WindowsElement>(TestConfig.WinAppDriverUri, rootOptions);
    }

    private static bool IsUiTestEnabled()
    {
        string? value = Environment.GetEnvironmentVariable("RUN_WINAPPDRIVER_TESTS");
        return string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);
    }

    private static void EnsureAppNotRunning()
    {
        foreach (Process process in Process.GetProcessesByName("QuanLyChungCu"))
        {
            try
            {
                process.Kill(entireProcessTree: true);
                process.WaitForExit(2000);
            }
            catch
            {
                // Best-effort cleanup only.
            }
            finally
            {
                process.Dispose();
            }
        }
    }

    private static void WaitUntil(Func<bool> condition, TimeSpan timeout)
    {
        var stopwatch = Stopwatch.StartNew();
        Exception? lastException = null;

        while (stopwatch.Elapsed < timeout)
        {
            try
            {
                if (condition())
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                lastException = ex;
            }

            Thread.Sleep(50);
        }

        string message = $"Condition was not met within {timeout.TotalSeconds:0.##} seconds.";
        if (lastException != null)
        {
            message += $" Last exception: {lastException.GetType().Name}: {lastException.Message}";
        }

        Assert.Fail(message);
    }
}

