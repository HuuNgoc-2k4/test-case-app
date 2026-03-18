using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace QuanLyChungCu.Tests.UI.Pages;

internal sealed class LoginPage(WindowsDriver<WindowsElement> session)
{
    private readonly WindowsDriver<WindowsElement> _session = session;

    public void EnterUsername(string username)
    {
        var usernameInput = _session.FindElement(MobileBy.AccessibilityId("Login.Username"));
        usernameInput.Clear();
        usernameInput.SendKeys(username);
    }

    public void EnterPassword(string password)
    {
        var passwordInput = _session.FindElement(MobileBy.AccessibilityId("Login.Password"));
        passwordInput.Clear();
        passwordInput.SendKeys(password);
    }

    public void Submit()
    {
        _session.FindElement(MobileBy.AccessibilityId("Login.Submit")).Click();
    }

    public bool IsInvalidLoginMessageVisible()
    {
        return _session.FindElements(MobileBy.Name("SDT không đúng hoặc người dùng không tồn tại!")).Count > 0;
    }

    public bool IsAdminScreenVisible()
    {
        return _session.FindElements(MobileBy.Name("Danh Sách Cư dân")).Count > 0;
    }

    public bool IsAuthenticatedScreenVisible()
    {
        return _session.FindElements(MobileBy.Name("Đăng xuất")).Count > 0;
    }

    public bool IsLoginScreenVisible()
    {
        return _session.FindElements(MobileBy.AccessibilityId("Login.Submit")).Count > 0;
    }
}
