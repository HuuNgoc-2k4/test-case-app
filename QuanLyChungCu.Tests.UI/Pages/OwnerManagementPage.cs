using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System.Text;

namespace QuanLyChungCu.Tests.UI.Pages;

internal sealed class OwnerManagementPage(WindowsDriver<WindowsElement> session)
{
    internal enum OwnerSearchField
    {
        Name,
        Phone,
        RoomNumber,
        QueQuan,
        BirthDate
    }

    private readonly WindowsDriver<WindowsElement> _session = session;

    public void OpenOwnerListScreen()
    {
        if (IsOwnerScreenReady())
        {
            return;
        }

        var menuButton = FindFirstElement(
            () => _session.FindElements(By.Name("Danh Sách Cư dân")).FirstOrDefault());

        menuButton?.Click();
    }

    public bool WaitForOwnerScreenReady(TimeSpan timeout)
    {
        var start = DateTime.UtcNow;
        while (DateTime.UtcNow - start < timeout)
        {
            if (IsOwnerScreenReady())
            {
                return true;
            }

            Thread.Sleep(60);
        }

        return IsOwnerScreenReady();
    }

    public bool IsOwnerScreenReady()
    {
        WindowsElement? addButton = FindFirstElement(
            () => _session.FindElements(MobileBy.AccessibilityId("Owner.AddButton")).FirstOrDefault(),
            () => _session.FindElements(MobileBy.AccessibilityId("addbutton")).FirstOrDefault(),
            () => _session.FindElements(By.Name("Thêm chủ hộ")).FirstOrDefault());

        WindowsElement? searchInput = FindFirstElement(
            () => _session.FindElements(MobileBy.AccessibilityId("Owner.SearchInput")).FirstOrDefault(),
            () => _session.FindElements(MobileBy.AccessibilityId("txtSearch")).FirstOrDefault(),
            () => _session.FindElements(By.Name("Nhập từ khóa...")).FirstOrDefault());

        return addButton != null && searchInput != null;
    }

    public void SearchByName(string keyword)
    {
        Search(OwnerSearchField.Name, keyword);
    }

    public void Search(OwnerSearchField field, string keyword)
    {
        SelectSearchField(field);

        var searchInput = FindRequiredElement(
            () => _session.FindElements(MobileBy.AccessibilityId("Owner.SearchInput")).FirstOrDefault(),
            () => _session.FindElements(MobileBy.AccessibilityId("txtSearch")).FirstOrDefault(),
            () => _session.FindElements(By.Name("Nhập từ khóa...")).FirstOrDefault());

        searchInput.Click();
        searchInput.SendKeys(Keys.Control + "a");
        searchInput.SendKeys(Keys.Backspace);
        searchInput.SendKeys(keyword);

        var searchButton = FindRequiredElement(
            () => _session.FindElements(MobileBy.AccessibilityId("Owner.SearchButton")).FirstOrDefault(),
            () => _session.FindElements(MobileBy.AccessibilityId("btnSearch")).FirstOrDefault(),
            () => _session.FindElements(By.Name("Tìm Kiếm")).FirstOrDefault());

        searchButton.Click();
    }

    private void SelectSearchField(OwnerSearchField field)
    {
        if (field == OwnerSearchField.Name)
        {
            return;
        }

        var searchColumn = FindRequiredElement(
            () => _session.FindElements(MobileBy.AccessibilityId("Owner.SearchColumn")).FirstOrDefault(),
            () => _session.FindElements(MobileBy.AccessibilityId("cmbSearchColumn")).FirstOrDefault());

        searchColumn.Click();

        searchColumn.SendKeys(Keys.Home);
        for (int i = 0; i < (int)field; i++)
        {
            searchColumn.SendKeys(Keys.ArrowDown);
        }

        searchColumn.SendKeys(Keys.Enter);
    }

    public void ClearSearch()
    {
        var clearButton = FindRequiredElement(
            () => _session.FindElements(MobileBy.AccessibilityId("Owner.ClearSearch")).FirstOrDefault(),
            () => _session.FindElements(MobileBy.AccessibilityId("btnClearSearch")).FirstOrDefault(),
            () => _session.FindElements(By.Name("Xóa Lọc")).FirstOrDefault());

        clearButton.Click();
    }

    public void OpenAddOwnerDialog()
    {
        var addButton = FindRequiredElement(
            () => _session.FindElements(MobileBy.AccessibilityId("Owner.AddButton")).FirstOrDefault(),
            () => _session.FindElements(MobileBy.AccessibilityId("addbutton")).FirstOrDefault(),
            () => _session.FindElements(By.Name("Thêm chủ hộ")).FirstOrDefault());

        addButton.Click();
    }

    public void OpenFirstResultForEdit()
    {
        var editButton = FindRequiredElement(
            () => _session.FindElements(MobileBy.AccessibilityId("Owner.EditButton")).FirstOrDefault(),
            () => _session.FindElements(By.Name("Owner.Edit")).FirstOrDefault());

        editButton.Click();
    }

    public void DeleteFirstResult()
    {
        var deleteButton = FindRequiredElement(
            () => _session.FindElements(MobileBy.AccessibilityId("Owner.DeleteButton")).FirstOrDefault(),
            () => _session.FindElements(By.Name("Owner.Delete")).FirstOrDefault());

        deleteButton.Click();
    }

    public bool IsOwnerVisible(string ownerName)
    {
        return _session.FindElements(By.Name(ownerName)).Count > 0;
    }

    public string BuildLocatorDiagnostics(int maxElements = 80)
    {
        int searchInputCount = CountElements(
            MobileBy.AccessibilityId("Owner.SearchInput"),
            MobileBy.AccessibilityId("txtSearch"),
            By.Name("Nhập từ khóa..."));

        int addButtonCount = CountElements(
            MobileBy.AccessibilityId("Owner.AddButton"),
            MobileBy.AccessibilityId("addbutton"),
            By.Name("Thêm chủ hộ"));

        int searchButtonCount = CountElements(
            MobileBy.AccessibilityId("Owner.SearchButton"),
            MobileBy.AccessibilityId("btnSearch"),
            By.Name("Tìm Kiếm"));

        int editButtonCount = CountElements(
            MobileBy.AccessibilityId("Owner.EditButton"),
            By.Name("Owner.Edit"));

        int deleteButtonCount = CountElements(
            MobileBy.AccessibilityId("Owner.DeleteButton"),
            By.Name("Owner.Delete"));

        var lines = new List<string>
        {
            $"Owner.SearchInput matches: {searchInputCount}",
            $"Owner.AddButton matches: {addButtonCount}",
            $"Owner.SearchButton matches: {searchButtonCount}",
            $"Owner.EditButton matches: {editButtonCount}",
            $"Owner.DeleteButton matches: {deleteButtonCount}",
            "Visible element snapshot (Name | AutomationId | ClassName):"
        };

        foreach (WindowsElement element in _session.FindElements(By.XPath("//*")).Take(maxElements))
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

    private int CountElements(params By[] locators)
    {
        foreach (By locator in locators)
        {
            int count = _session.FindElements(locator).Count;
            if (count > 0)
            {
                return count;
            }
        }

        return 0;
    }

    private WindowsElement? FindFirstElement(params Func<WindowsElement?>[] candidates)
    {
        foreach (Func<WindowsElement?> candidate in candidates)
        {
            WindowsElement? element = candidate();
            if (element != null)
            {
                return element;
            }
        }

        return null;
    }

    private WindowsElement FindRequiredElement(params Func<WindowsElement?>[] candidates)
    {
        return FindFirstElement(candidates)
               ?? throw new OpenQA.Selenium.WebDriverException("Unable to locate required owner-management element.");
    }
}

