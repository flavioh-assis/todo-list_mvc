using OpenQA.Selenium;
using ToDoList.Tests.Utils;

namespace ToDoList.Tests.E2ETests.Pages;

public class EditTaskPage : SharedSelectors
{
    public EditTaskPage(IWebDriver driver) : base(driver)
    {
    }

    public IWebElement InputTitleField()
    {
        return GetInputById("title");
    }

    public void EnterTitle(string value)
    {
        InputTitleField().SendKeys(value);
    }

    public void ClickSave()
    {
        var currentUrl = CurrentUrl();

        ButtonByText("Salvar").Click();

        WaitUrlToChange(currentUrl);
    }

    public void ClickCancel()
    {
        var currentUrl = CurrentUrl();

        ButtonByText("Cancelar").Click();

        WaitUrlToChange(currentUrl);
    }
}