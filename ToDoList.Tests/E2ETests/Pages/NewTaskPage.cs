using OpenQA.Selenium;
using ToDoList.Tests.Utils;

namespace ToDoList.Tests.E2ETests.Pages;

public class NewTaskPage : SharedSelectors
{
    private readonly IWebDriver _driver;

    public NewTaskPage(IWebDriver driver) : base(driver)
    {
        _driver = driver;
    }

    public IWebElement InputTitleField()
    {
        return GetInputById("Title");
    }

    public void EnterTitle(string value)
    {
        var input = InputTitleField();
        input.SendKeys(value);
    }

    public void ClickCreateTask()
    {
        var button = ButtonByText("Criar Tarefa");
        button.Click();
    }

    public void ClickCancel()
    {
        var button = ButtonByText("Cancelar");
        button.Click();
    }
}