using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace QuanLyChungCu.Tests.UI.Pages;

internal sealed class OwnerDialogPage(WindowsDriver<WindowsElement> session)
{
    private readonly WindowsDriver<WindowsElement> _session = session;

    public void FillAddOwnerForm(string name, string phone, string queQuan, string birthDate, string roomNumber)
    {
        SetInput("AddOwner.NameInput", name);
        SetInput("AddOwner.PhoneInput", phone);
        SetInput("AddOwner.QueQuanInput", queQuan);
        SetInput("AddOwner.BirthDateInput", birthDate);
        SetInput("AddOwner.RoomNumberInput", roomNumber);
    }

    public void SubmitAddOwner()
    {
        _session.FindElement(MobileBy.AccessibilityId("AddOwner.Submit")).Click();
    }

    public void FillEditOwnerForm(string name, string phone, string queQuan, string birthDate, string roomNumber)
    {
        SetInput("EditOwner.NameInput", name);
        SetInput("EditOwner.PhoneInput", phone);
        SetInput("EditOwner.QueQuanInput", queQuan);
        SetInput("EditOwner.BirthDateInput", birthDate);
        SetInput("EditOwner.RoomNumberInput", roomNumber);
    }

    public void SubmitEditOwner()
    {
        _session.FindElement(MobileBy.AccessibilityId("EditOwner.Save")).Click();
    }

    private void SetInput(string automationId, string value)
    {
        var input = _session.FindElement(MobileBy.AccessibilityId(automationId));
        input.Click();
        input.SendKeys(Keys.Control + "a");
        input.SendKeys(Keys.Backspace);
        input.SendKeys(value);
    }
}

