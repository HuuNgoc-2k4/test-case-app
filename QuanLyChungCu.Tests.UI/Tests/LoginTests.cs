using QuanLyChungCu.Tests.UI.Infrastructure;
using QuanLyChungCu.Tests.UI.Pages;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System.Diagnostics;
using System.Globalization;

namespace QuanLyChungCu.Tests.UI.Tests;

[TestClass]
public sealed class OwnerManagementTests
{
    private static readonly TimeSpan AddTimeout = TimeSpan.FromSeconds(2.5);
    private static readonly TimeSpan EditTimeout = TimeSpan.FromSeconds(2.5);
    private static readonly TimeSpan DeleteTimeout = TimeSpan.FromSeconds(2.5);
    private static readonly TimeSpan SearchTimeout = TimeSpan.FromSeconds(3);
    private static readonly TimeSpan DialogTimeout = TimeSpan.FromSeconds(4);
    private static readonly TimeSpan NavigationTimeout = TimeSpan.FromSeconds(4);
    private static readonly TimeSpan RetryNavigationTimeout = TimeSpan.FromSeconds(2.5);
    private static readonly TimeSpan LoginTimeout = TimeSpan.FromSeconds(6);

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

        LoginAsAdmin();
        ReattachToProgramsWindow();
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
            // Ignore cleanup exceptions because the app may close itself during test.
        }
        finally
        {
            EnsureAppNotRunning();
        }
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        EnsureAppNotRunning();
    }

    [TestMethod]
    public void AddOwner_AddsNewOwnerToList()
    {
        var ownersPage = new OwnerManagementPage(GetSession());

        NavigateToOwnerScreenOrFail(ownersPage, "AddOwner");

        ownersPage.OpenAddOwnerDialog();
        using (WindowsDriver<WindowsElement> addOwnerSession = CreateSessionForWindow("Thêm Chủ Hộ", DialogTimeout))
        {
            var ownerDialog = new OwnerDialogPage(addOwnerSession);
            ownerDialog.FillAddOwnerForm("Chu ho moi", "0901999999", "Nam Dinh", "05/05/1985", "105");
            ownerDialog.SubmitAddOwner();
        }

        WaitUntil(() => ownersPage.IsOwnerVisible("Chu ho moi"), AddTimeout, () => ownersPage.BuildLocatorDiagnostics());
        Assert.IsTrue(ownersPage.IsOwnerVisible("Chu ho moi"));
    }

    [TestMethod]
    public void EditOwner_UpdatesOwnerInformation()
    {
        var ownersPage = new OwnerManagementPage(GetSession());

        NavigateToOwnerScreenOrFail(ownersPage, "EditOwner");

        ownersPage.SearchByName("Chu ho sua");
        WaitUntil(() => ownersPage.IsOwnerVisible("Chu ho sua"), EditTimeout, () => ownersPage.BuildLocatorDiagnostics());

        ownersPage.OpenFirstResultForEdit();
        using (WindowsDriver<WindowsElement> editOwnerSession = CreateSessionForWindow("Sửa Thông Tin Chủ Hộ", DialogTimeout))
        {
            var ownerDialog = new OwnerDialogPage(editOwnerSession);
            ownerDialog.FillEditOwnerForm("Chu ho da sua", "0901888888", "Ha Nam", "06/06/1986", "106");
            ownerDialog.SubmitEditOwner();
        }

        ClickDialogButtonByName("OK");

        ownersPage.SearchByName("Chu ho da sua");
        WaitUntil(() => ownersPage.IsOwnerVisible("Chu ho da sua"), EditTimeout, () => ownersPage.BuildLocatorDiagnostics());
        Assert.IsTrue(ownersPage.IsOwnerVisible("Chu ho da sua"));
    }

    [TestMethod]
    public void DeleteOwner_RemovesOwnerFromList()
    {
        var ownersPage = new OwnerManagementPage(GetSession());

        NavigateToOwnerScreenOrFail(ownersPage, "DeleteOwner");

        ownersPage.SearchByName("Chu ho xoa");
        WaitUntil(() => ownersPage.IsOwnerVisible("Chu ho xoa"), DeleteTimeout, () => ownersPage.BuildLocatorDiagnostics());

        ownersPage.DeleteFirstResult();
        ClickDialogButtonByName("Yes", "Có");

        ownersPage.SearchByName("Chu ho xoa");
        WaitUntil(() => !ownersPage.IsOwnerVisible("Chu ho xoa"), DeleteTimeout, () => ownersPage.BuildLocatorDiagnostics());
        Assert.IsFalse(ownersPage.IsOwnerVisible("Chu ho xoa"));
    }

    [TestMethod]
    public void SearchOwner_FiltersByKeyword()
    {
        var ownersPage = new OwnerManagementPage(GetSession());

        NavigateToOwnerScreenOrFail(ownersPage, "SearchOwner");

        var searchCases = new (OwnerManagementPage.OwnerSearchField Field, string Keyword, string Label)[]
        {
            (OwnerManagementPage.OwnerSearchField.Name, "timkiem", "Tên"),
            (OwnerManagementPage.OwnerSearchField.Phone, "0901000003", "Số điện thoại"),
            (OwnerManagementPage.OwnerSearchField.RoomNumber, "103", "Số phòng"),
            (OwnerManagementPage.OwnerSearchField.QueQuan, "da nang", "Quê quán"),
            (OwnerManagementPage.OwnerSearchField.BirthDate, "03/03/1983", "Ngày sinh")
        };

        foreach (var searchCase in searchCases)
        {
            ownersPage.ClearSearch();
            ownersPage.Search(searchCase.Field, searchCase.Keyword);

            WaitUntil(
                () => ownersPage.IsOwnerVisible("Chu ho timkiem"),
                SearchTimeout,
                () => $"[Search by {searchCase.Label} with '{searchCase.Keyword}']\n{ownersPage.BuildLocatorDiagnostics()}");

            Assert.IsTrue(
                ownersPage.IsOwnerVisible("Chu ho timkiem"),
                $"Expected owner 'Chu ho timkiem' for search field '{searchCase.Label}' and keyword '{searchCase.Keyword}'.");
            Assert.IsFalse(
                ownersPage.IsOwnerVisible("Chu ho khac"),
                $"Unexpected owner 'Chu ho khac' still visible for search field '{searchCase.Label}' and keyword '{searchCase.Keyword}'.");
        }
    }

    private WindowsDriver<WindowsElement> GetSession()
    {
        return _session ?? throw new InvalidOperationException("WinAppDriver session has not been initialized.");
    }

    private static void NavigateToOwnerScreenOrFail(OwnerManagementPage ownersPage, string scenarioName)
    {
        ownersPage.OpenOwnerListScreen();

        if (ownersPage.WaitForOwnerScreenReady(NavigationTimeout))
        {
            return;
        }

        ownersPage.OpenOwnerListScreen();
        if (ownersPage.WaitForOwnerScreenReady(RetryNavigationTimeout))
        {
            return;
        }

        string diagnostics = ownersPage.BuildLocatorDiagnostics();
        Assert.Fail($"[{scenarioName}] Owner screen is not ready.\n{diagnostics}");
    }

    private void LoginAsAdmin()
    {
        var loginPage = new LoginPage(GetSession());

        loginPage.EnterUsername("admin");
        loginPage.EnterPassword("1");
        loginPage.Submit();

        WaitUntil(
            () => !loginPage.IsLoginScreenVisible() || loginPage.IsInvalidLoginMessageVisible(),
            LoginTimeout,
            () => BuildElementTreeSnapshot(GetSession()));

        Assert.IsFalse(
            loginPage.IsLoginScreenVisible(),
            "Expected authenticated screen after admin login, but app remained on login or showed error.");
    }

    private void ReattachToProgramsWindow()
    {
        using WindowsDriver<WindowsElement> rootSession = CreateRootSession();

        WindowsElement? programWindow = null;

        WaitUntil(
            () =>
            {
                programWindow = FindFirstByNameSafe(rootSession, "Quản Lý Chung Cư");
                return programWindow != null;
            },
            NavigationTimeout,
            () => BuildElementTreeSnapshot(rootSession));

        if (programWindow is null)
        {
            Assert.Fail("Programs window was not found after login.");
            return;
        }

        string nativeHandle = programWindow.GetAttribute("NativeWindowHandle");
        if (!int.TryParse(nativeHandle, NumberStyles.Integer, CultureInfo.InvariantCulture, out int handle))
        {
            Assert.Fail($"Cannot parse NativeWindowHandle '{nativeHandle}' for Programs window.");
            return;
        }

        string topLevelWindowHandle = handle.ToString("x");

        var appiumOptions = new AppiumOptions();
        appiumOptions.PlatformName = "Windows";
        appiumOptions.AddAdditionalCapability("appTopLevelWindow", topLevelWindowHandle);

        WindowsDriver<WindowsElement> attachedSession = new WindowsDriver<WindowsElement>(TestConfig.WinAppDriverUri, appiumOptions);
        attachedSession.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);

        _session?.Quit();
        _session = attachedSession;
    }

    private static void ClickDialogButtonByName(params string[] buttonNames)
    {
        using WindowsDriver<WindowsElement> rootSession = CreateRootSession();

        WindowsElement? targetButton = null;

        WaitUntil(
            () =>
            {
                targetButton = buttonNames
                    .Select(name => FindFirstByNameSafe(rootSession, name))
                    .FirstOrDefault(button => button != null);
                return targetButton != null;
            },
            SearchTimeout,
            () => BuildElementTreeSnapshot(rootSession));

        if (targetButton != null)
        {
            targetButton.Click();
            return;
        }

        Assert.Fail($"Unable to find dialog button with any of names: {string.Join(", ", buttonNames)}");
    }

    private static WindowsDriver<WindowsElement> CreateRootSession()
    {
        var rootOptions = new AppiumOptions();
        rootOptions.PlatformName = "Windows";
        rootOptions.AddAdditionalCapability("app", "Root");
        return new WindowsDriver<WindowsElement>(TestConfig.WinAppDriverUri, rootOptions);
    }

    private static WindowsDriver<WindowsElement> CreateSessionForWindow(string windowTitle, TimeSpan timeout)
    {
        using WindowsDriver<WindowsElement> rootSession = CreateRootSession();

        WindowsElement? dialogWindow = null;

        WaitUntil(
            () =>
            {
                dialogWindow = FindFirstByNameSafe(rootSession, windowTitle);
                return dialogWindow != null;
            },
            timeout,
            () => BuildElementTreeSnapshot(rootSession));

        if (dialogWindow is null)
        {
            throw new InvalidOperationException($"Unable to attach to window '{windowTitle}'.");
        }

        string nativeHandle = dialogWindow.GetAttribute("NativeWindowHandle");
        if (!int.TryParse(nativeHandle, NumberStyles.Integer, CultureInfo.InvariantCulture, out int handle))
        {
            throw new InvalidOperationException($"Cannot parse NativeWindowHandle '{nativeHandle}' for window '{windowTitle}'.");
        }

        string topLevelWindowHandle = handle.ToString("x");

        var appiumOptions = new AppiumOptions();
        appiumOptions.PlatformName = "Windows";
        appiumOptions.AddAdditionalCapability("appTopLevelWindow", topLevelWindowHandle);

        WindowsDriver<WindowsElement> windowSession = new WindowsDriver<WindowsElement>(TestConfig.WinAppDriverUri, appiumOptions);
        windowSession.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
        return windowSession;
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

    private static string BuildElementTreeSnapshot(WindowsDriver<WindowsElement> session, int maxElements = 120)
    {
        var lines = new List<string>
        {
            "Element snapshot (Name | AutomationId | ClassName):"
        };

        foreach (WindowsElement element in session.FindElements(OpenQA.Selenium.By.XPath("//*")).Take(maxElements))
        {
            string name = element.GetAttribute("Name") ?? string.Empty;
            string automationId = element.GetAttribute("AutomationId") ?? string.Empty;
            string className = element.GetAttribute("ClassName") ?? string.Empty;

            if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(automationId))
            {
                continue;
            }

            lines.Add($"- {name} | {automationId} | {className}");
        }

        return string.Join(Environment.NewLine, lines);
    }

    private static WindowsElement? FindFirstByNameSafe(WindowsDriver<WindowsElement> session, string name)
    {
        try
        {
            return session.FindElements(OpenQA.Selenium.By.Name(name)).FirstOrDefault();
        }
        catch
        {
            return null;
        }
    }

    private static void WaitUntil(Func<bool> condition, TimeSpan timeout, Func<string>? onTimeoutDiagnostics = null)
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

            Thread.Sleep(80);
        }

        string message = $"Condition was not met within {timeout.TotalSeconds:0.##} seconds.";

        if (lastException != null)
        {
            message += $" Last exception: {lastException.GetType().Name}: {lastException.Message}";
        }

        if (onTimeoutDiagnostics != null)
        {
            try
            {
                message += Environment.NewLine + onTimeoutDiagnostics();
            }
            catch (Exception diagnosticsException)
            {
                message += Environment.NewLine + $"Failed to collect diagnostics: {diagnosticsException.Message}";
            }
        }

        Assert.Fail(message);
    }
}
