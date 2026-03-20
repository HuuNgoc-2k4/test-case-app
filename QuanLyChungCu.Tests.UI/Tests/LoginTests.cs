using QuanLyChungCu.Tests.UI.Infrastructure;
using QuanLyChungCu.Tests.UI.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System.Diagnostics;
using System.Globalization;

namespace QuanLyChungCu.Tests.UI.Tests;

[TestClass]
public sealed class OwnerManagementTests
{
    private static readonly TimeSpan AddTimeout = TimeSpan.FromSeconds(2);
    private static readonly TimeSpan EditTimeout = TimeSpan.FromSeconds(2);
    private static readonly TimeSpan DeleteTimeout = TimeSpan.FromSeconds(2);
    private static readonly TimeSpan SearchTimeout = TimeSpan.FromSeconds(2.2);
    private static readonly TimeSpan DialogTimeout = TimeSpan.FromSeconds(3.2);
    private static readonly TimeSpan NavigationTimeout = TimeSpan.FromSeconds(3.2);
    private static readonly TimeSpan RetryNavigationTimeout = TimeSpan.FromSeconds(1.8);
    private static readonly TimeSpan LoginTimeout = TimeSpan.FromSeconds(4.5);

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
    [TestCategory("TC_ADDOWNER_001")]
    public void AddOwner_001_WithValidData_AddsSuccessfully()
    {
        var ownersPage = new OwnerManagementPage(GetSession());

        NavigateToOwnerScreenOrFail(ownersPage, "TC_ADDOWNER_001");

        ownersPage.OpenAddOwnerDialog();
        using (WindowsDriver<WindowsElement> addOwnerSession = CreateSessionForWindow("Thêm Chủ Hộ", DialogTimeout))
        {
            var ownerDialog = new OwnerDialogPage(addOwnerSession);
            ownerDialog.FillAddOwnerForm("Chu ho test 001", "0901999999", "Nam Dinh", "05/05/1985", "109");
            ownerDialog.SubmitAddOwner();
        }

        WaitUntil(() => ownersPage.IsOwnerVisible("Chu ho test 001"), AddTimeout, () => ownersPage.BuildLocatorDiagnostics());
        Assert.IsTrue(ownersPage.IsOwnerVisible("Chu ho test 001"));
    }

    [TestMethod]
    [TestCategory("TC_ADDOWNER_002")]
    public void AddOwner_002_WithInvalidRoomNumber_ShowsError()
    {
        var ownersPage = new OwnerManagementPage(GetSession());

        NavigateToOwnerScreenOrFail(ownersPage, "TC_ADDOWNER_002");

        ownersPage.OpenAddOwnerDialog();
        using (WindowsDriver<WindowsElement> addOwnerSession = CreateSessionForWindow("Thêm Chủ Hộ", DialogTimeout))
        {
            var ownerDialog = new OwnerDialogPage(addOwnerSession);
            ownerDialog.FillAddOwnerForm("Chu ho test 002", "0901888888", "Ha Nam", "06/06/1986", "abc");
            ownerDialog.SubmitAddOwner();
            // Should show validation error
            WaitUntil(() => addOwnerSession.FindElements(By.Name("Số phòng phải là số nguyên!")).Count > 0, DialogTimeout, () => "Expected error message not found");
            Assert.IsTrue(addOwnerSession.FindElements(By.Name("Số phòng phải là số nguyên!")).Count > 0);
        }
    }

    [TestMethod]
    [TestCategory("TC_ADDOWNER_003")]
    public void AddOwner_003_WithInvalidPhone_ShowsError()
    {
        var ownersPage = new OwnerManagementPage(GetSession());

        NavigateToOwnerScreenOrFail(ownersPage, "TC_ADDOWNER_003");

        ownersPage.OpenAddOwnerDialog();
        using (WindowsDriver<WindowsElement> addOwnerSession = CreateSessionForWindow("Thêm Chủ Hộ", DialogTimeout))
        {
            var ownerDialog = new OwnerDialogPage(addOwnerSession);
            ownerDialog.FillAddOwnerForm("Chu ho test 003", "123", "Ha Noi", "07/07/1987", "106");
            ownerDialog.SubmitAddOwner();
            // Should show validation error for phone
            WaitUntil(() => addOwnerSession.FindElements(By.Name("Số điện thoại phải có 10 chữ số và bắt đầu bằng số 0!")).Count > 0, DialogTimeout, () => "Expected error message not found");
            Assert.IsTrue(addOwnerSession.FindElements(By.Name("Số điện thoại phải có 10 chữ số và bắt đầu bằng số 0!")).Count > 0);
        }
    }

    [TestMethod]
    [TestCategory("TC_ADDOWNER_004")]
    public void AddOwner_004_WithDuplicatePhone_ShowsError()
    {
        var ownersPage = new OwnerManagementPage(GetSession());

        NavigateToOwnerScreenOrFail(ownersPage, "TC_ADDOWNER_004");

        ownersPage.OpenAddOwnerDialog();
        using (WindowsDriver<WindowsElement> addOwnerSession = CreateSessionForWindow("Thêm Chủ Hộ", DialogTimeout))
        {
            var ownerDialog = new OwnerDialogPage(addOwnerSession);
            // Using existing phone number
            ownerDialog.FillAddOwnerForm("Chu ho test 004", "0901000001", "Ha Noi", "08/08/1988", "107");
            ownerDialog.SubmitAddOwner();
            // Should show error for duplicate
            WaitUntil(() => addOwnerSession.FindElements(By.Name("Số điện thoại đã tồn tại, vui lòng nhập số khác!")).Count > 0, DialogTimeout, () => "Expected error message not found");
            Assert.IsTrue(addOwnerSession.FindElements(By.Name("Số điện thoại đã tồn tại, vui lòng nhập số khác!")).Count > 0);
        }
    }

    [TestMethod]
    [TestCategory("TC_ADDOWNER_005")]
    public void AddOwner_005_WithDuplicateRoom_ShowsError()
    {
        var ownersPage = new OwnerManagementPage(GetSession());

        NavigateToOwnerScreenOrFail(ownersPage, "TC_ADDOWNER_005");

        ownersPage.OpenAddOwnerDialog();
        using (WindowsDriver<WindowsElement> addOwnerSession = CreateSessionForWindow("Thêm Chủ Hộ", DialogTimeout))
        {
            var ownerDialog = new OwnerDialogPage(addOwnerSession);
            // Using existing room number
            ownerDialog.FillAddOwnerForm("Chu ho test 005", "0901777777", "Ha Noi", "09/09/1989", "101");
            ownerDialog.SubmitAddOwner();
            // Should show error for duplicate room
            WaitUntil(() => addOwnerSession.FindElements(By.Name("Số phòng đã tồn tại, vui lòng nhập số phòng khác!")).Count > 0, DialogTimeout, () => "Expected error message not found");
            Assert.IsTrue(addOwnerSession.FindElements(By.Name("Số phòng đã tồn tại, vui lòng nhập số phòng khác!")).Count > 0);
        }
    }

    [TestMethod]
    [TestCategory("TC_ADDOWNER_006")]
    public void AddOwner_006_WithValidDataComplex_AddsSuccessfully()
    {
        var ownersPage = new OwnerManagementPage(GetSession());

        NavigateToOwnerScreenOrFail(ownersPage, "TC_ADDOWNER_006");

        ownersPage.OpenAddOwnerDialog();
        using (WindowsDriver<WindowsElement> addOwnerSession = CreateSessionForWindow("Thêm Chủ Hộ", DialogTimeout))
        {
            var ownerDialog = new OwnerDialogPage(addOwnerSession);
            ownerDialog.FillAddOwnerForm("Chu ho test 006", "0901666666", "Nam Dinh", "10/10/1990", "110");
            ownerDialog.SubmitAddOwner();
        }

        WaitUntil(() => ownersPage.IsOwnerVisible("Chu ho test 006"), AddTimeout, () => ownersPage.BuildLocatorDiagnostics());
        Assert.IsTrue(ownersPage.IsOwnerVisible("Chu ho test 006"));
    }

    [TestMethod]
    [TestCategory("TC_EDITOWNER_001")]
    public void EditOwner_001_UpdateName_SuccessfullyEdited()
    {
        var ownersPage = new OwnerManagementPage(GetSession());

        NavigateToOwnerScreenOrFail(ownersPage, "TC_EDITOWNER_001");

        ownersPage.SearchByName("Chu ho sua");
        WaitUntil(() => ownersPage.IsOwnerVisible("Chu ho sua"), EditTimeout, () => ownersPage.BuildLocatorDiagnostics());

        ownersPage.OpenFirstResultForEdit();
        using (WindowsDriver<WindowsElement> editOwnerSession = CreateSessionForWindow("Sửa Thông Tin Chủ Hộ", DialogTimeout))
        {
            var ownerDialog = new OwnerDialogPage(editOwnerSession);
            ownerDialog.FillEditOwnerForm("Chu ho da sua ten", "0901000001", "Ha Noi", "01/01/1980", "101");
            ownerDialog.SubmitEditOwner();
        }

        ClickDialogButtonByName("OK");

        ownersPage.SearchByName("Chu ho da sua ten");
        WaitUntil(() => ownersPage.IsOwnerVisible("Chu ho da sua ten"), EditTimeout, () => ownersPage.BuildLocatorDiagnostics());
        Assert.IsTrue(ownersPage.IsOwnerVisible("Chu ho da sua ten"));
    }

    [TestMethod]
    [TestCategory("TC_EDITOWNER_002")]
    public void EditOwner_002_WithInvalidRoomNumber_ShowsError()
    {
        var ownersPage = new OwnerManagementPage(GetSession());

        NavigateToOwnerScreenOrFail(ownersPage, "TC_EDITOWNER_002");

        ownersPage.SearchByName("Chu ho sua");
        WaitUntil(() => ownersPage.IsOwnerVisible("Chu ho sua"), EditTimeout, () => ownersPage.BuildLocatorDiagnostics());

        ownersPage.OpenFirstResultForEdit();
        using (WindowsDriver<WindowsElement> editOwnerSession = CreateSessionForWindow("Sửa Thông Tin Chủ Hộ", DialogTimeout))
        {
            var ownerDialog = new OwnerDialogPage(editOwnerSession);
            ownerDialog.FillEditOwnerForm("Chu ho sua", "0901000001", "Ha Noi", "01/01/1980", "chu");
            ownerDialog.SubmitEditOwner();
            // Should show validation error
            WaitUntil(() => editOwnerSession.FindElements(By.Name("Số phòng phải là số nguyên!")).Count > 0, DialogTimeout, () => "Expected error message not found");
            Assert.IsTrue(editOwnerSession.FindElements(By.Name("Số phòng phải là số nguyên!")).Count > 0);
        }
    }

    [TestMethod]
    [TestCategory("TC_EDITOWNER_003")]
    public void EditOwner_003_WithInvalidPhone_ShowsError()
    {
        var ownersPage = new OwnerManagementPage(GetSession());

        NavigateToOwnerScreenOrFail(ownersPage, "TC_EDITOWNER_003");

        ownersPage.SearchByName("Chu ho sua");
        WaitUntil(() => ownersPage.IsOwnerVisible("Chu ho sua"), EditTimeout, () => ownersPage.BuildLocatorDiagnostics());

        ownersPage.OpenFirstResultForEdit();
        using (WindowsDriver<WindowsElement> editOwnerSession = CreateSessionForWindow("Sửa Thông Tin Chủ Hộ", DialogTimeout))
        {
            var ownerDialog = new OwnerDialogPage(editOwnerSession);
            ownerDialog.FillEditOwnerForm("Chu ho sua", "999", "Ha Noi", "01/01/1980", "101");
            ownerDialog.SubmitEditOwner();
            // Should show validation error for phone
            WaitUntil(() => editOwnerSession.FindElements(By.Name("Số điện thoại không hợp lệ!")).Count > 0, DialogTimeout, () => "Expected error message not found");
            Assert.IsTrue(editOwnerSession.FindElements(By.Name("Số điện thoại không hợp lệ!")).Count > 0);
        }
    }

    [TestMethod]
    [TestCategory("TC_EDITOWNER_004")]
    public void EditOwner_004_UpdateAllFields_SuccessfullyEdited()
    {
        var ownersPage = new OwnerManagementPage(GetSession());

        NavigateToOwnerScreenOrFail(ownersPage, "TC_EDITOWNER_004");

        ownersPage.SearchByName("Chu ho sua");
        WaitUntil(() => ownersPage.IsOwnerVisible("Chu ho sua"), EditTimeout, () => ownersPage.BuildLocatorDiagnostics());

        ownersPage.OpenFirstResultForEdit();
        using (WindowsDriver<WindowsElement> editOwnerSession = CreateSessionForWindow("Sửa Thông Tin Chủ Hộ", DialogTimeout))
        {
            var ownerDialog = new OwnerDialogPage(editOwnerSession);
            ownerDialog.FillEditOwnerForm("Chu ho da sua day du", "0901000001", "Ha Noi", "01/01/1980", "101");
            ownerDialog.SubmitEditOwner();
        }

        ClickDialogButtonByName("OK");

        ownersPage.SearchByName("Chu ho da sua day du");
        WaitUntil(() => ownersPage.IsOwnerVisible("Chu ho da sua day du"), EditTimeout, () => ownersPage.BuildLocatorDiagnostics());
        Assert.IsTrue(ownersPage.IsOwnerVisible("Chu ho da sua day du"));
    }

    [TestMethod]
    [TestCategory("TC_DELOWNER_001")]
    public void DeleteOwner_001_ConfirmDelete_RemovesSuccessfully()
    {
        var ownersPage = new OwnerManagementPage(GetSession());

        NavigateToOwnerScreenOrFail(ownersPage, "TC_DELOWNER_001");

        ownersPage.SearchByName("Chu ho xoa");
        WaitUntil(() => ownersPage.IsOwnerVisible("Chu ho xoa"), DeleteTimeout, () => ownersPage.BuildLocatorDiagnostics());

        ownersPage.DeleteFirstResult();
        ClickDialogButtonByName("Yes", "Có");

        ownersPage.SearchByName("Chu ho xoa");
        WaitUntil(() => !ownersPage.IsOwnerVisible("Chu ho xoa"), DeleteTimeout, () => ownersPage.BuildLocatorDiagnostics());
        Assert.IsFalse(ownersPage.IsOwnerVisible("Chu ho xoa"));
    }

    [TestMethod]
    [TestCategory("TC_DELOWNER_002")]
    public void DeleteOwner_002_CancelDelete_StaysInList()
    {
        var ownersPage = new OwnerManagementPage(GetSession());

        NavigateToOwnerScreenOrFail(ownersPage, "TC_DELOWNER_002");

        ownersPage.SearchByName("Chu ho khac");
        WaitUntil(() => ownersPage.IsOwnerVisible("Chu ho khac"), DeleteTimeout, () => ownersPage.BuildLocatorDiagnostics());

        ownersPage.DeleteFirstResult();
        ClickDialogButtonByName("No", "Không");

        ownersPage.SearchByName("Chu ho khac");
        WaitUntil(() => ownersPage.IsOwnerVisible("Chu ho khac"), DeleteTimeout, () => ownersPage.BuildLocatorDiagnostics());
        Assert.IsTrue(ownersPage.IsOwnerVisible("Chu ho khac"));
    }

    [TestMethod]
    [TestCategory("TC_SEARCH_001")]
    public void SearchOwner_001_SearchByName_FiltersSuccessfully()
    {
        var ownersPage = new OwnerManagementPage(GetSession());

        NavigateToOwnerScreenOrFail(ownersPage, "TC_SEARCH_001");

        ownersPage.SearchByName("timkiem");

        WaitUntil(
            () => ownersPage.IsOwnerVisible("Chu ho timkiem"),
            SearchTimeout,
            () => $"[Search by Name with 'timkiem']\n{ownersPage.BuildLocatorDiagnostics()}");

        Assert.IsTrue(ownersPage.IsOwnerVisible("Chu ho timkiem"));
    }

    [TestMethod]
    [TestCategory("TC_SEARCH_002")]
    public void SearchOwner_002_SearchByPhone_FiltersSuccessfully()
    {
        var ownersPage = new OwnerManagementPage(GetSession());

        NavigateToOwnerScreenOrFail(ownersPage, "TC_SEARCH_002");

        ownersPage.Search(OwnerManagementPage.OwnerSearchField.Phone, "0901000003");

        WaitUntil(
            () => ownersPage.IsOwnerVisible("Chu ho timkiem"),
            SearchTimeout,
            () => $"[Search by Phone with '0901000003']\n{ownersPage.BuildLocatorDiagnostics()}");

        Assert.IsTrue(ownersPage.IsOwnerVisible("Chu ho timkiem"));
    }

    [TestMethod]
    [TestCategory("TC_SEARCH_003")]
    public void SearchOwner_003_SearchByRoomNumber_FiltersSuccessfully()
    {
        var ownersPage = new OwnerManagementPage(GetSession());

        NavigateToOwnerScreenOrFail(ownersPage, "TC_SEARCH_003");

        ownersPage.Search(OwnerManagementPage.OwnerSearchField.RoomNumber, "103");

        WaitUntil(
            () => ownersPage.IsOwnerVisible("Chu ho timkiem"),
            SearchTimeout,
            () => $"[Search by RoomNumber with '103']\n{ownersPage.BuildLocatorDiagnostics()}");

        Assert.IsTrue(ownersPage.IsOwnerVisible("Chu ho timkiem"));
    }

    [TestMethod]
    [TestCategory("TC_SEARCH_004")]
    public void SearchOwner_004_SearchByQueQuan_FiltersSuccessfully()
    {
        var ownersPage = new OwnerManagementPage(GetSession());

        NavigateToOwnerScreenOrFail(ownersPage, "TC_SEARCH_004");

        ownersPage.Search(OwnerManagementPage.OwnerSearchField.QueQuan, "da nang");

        WaitUntil(
            () => ownersPage.IsOwnerVisible("Chu ho timkiem"),
            SearchTimeout,
            () => $"[Search by QueQuan with 'da nang']\n{ownersPage.BuildLocatorDiagnostics()}");

        Assert.IsTrue(ownersPage.IsOwnerVisible("Chu ho timkiem"));
    }

    [TestMethod]
    [TestCategory("TC_SEARCH_005")]
    public void SearchOwner_005_SearchByBirthDate_FiltersSuccessfully()
    {
        var ownersPage = new OwnerManagementPage(GetSession());

        NavigateToOwnerScreenOrFail(ownersPage, "TC_SEARCH_005");

        ownersPage.Search(OwnerManagementPage.OwnerSearchField.BirthDate, "03/03/1983");

        WaitUntil(
            () => ownersPage.IsOwnerVisible("Chu ho timkiem"),
            SearchTimeout,
            () => $"[Search by BirthDate with '03/03/1983']\n{ownersPage.BuildLocatorDiagnostics()}");

        Assert.IsTrue(ownersPage.IsOwnerVisible("Chu ho timkiem"));
    }

    [TestMethod]
    [TestCategory("TC_SEARCH_006")]
    public void SearchOwner_006_SearchWithNoResults_ShowsEmpty()
    {
        var ownersPage = new OwnerManagementPage(GetSession());

        NavigateToOwnerScreenOrFail(ownersPage, "TC_SEARCH_006");

        ownersPage.SearchByName("khongcokeyword");

        WaitUntil(
            () => !ownersPage.IsOwnerVisible("Chu ho timkiem") && !ownersPage.IsOwnerVisible("Chu ho sua"),
            SearchTimeout,
            () => $"[Search with no matching results]\n{ownersPage.BuildLocatorDiagnostics()}");

        Assert.IsFalse(ownersPage.IsOwnerVisible("Chu ho timkiem"));
    }

    [TestMethod]
    [TestCategory("TC_SEARCH_007")]
    public void SearchOwner_007_ClearSearch_ShowsAllRecords()
    {
        var ownersPage = new OwnerManagementPage(GetSession());

        NavigateToOwnerScreenOrFail(ownersPage, "TC_SEARCH_007");

        ownersPage.SearchByName("timkiem");
        WaitUntil(() => ownersPage.IsOwnerVisible("Chu ho timkiem"), SearchTimeout, () => ownersPage.BuildLocatorDiagnostics());

        ownersPage.ClearSearch();
        WaitUntil(() => ownersPage.IsOwnerVisible("Chu ho timkiem"), SearchTimeout, () => ownersPage.BuildLocatorDiagnostics());

        Assert.IsTrue(ownersPage.IsOwnerVisible("Chu ho timkiem"));
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
        attachedSession.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(300);

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
        windowSession.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(300);
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

            Thread.Sleep(50);
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
