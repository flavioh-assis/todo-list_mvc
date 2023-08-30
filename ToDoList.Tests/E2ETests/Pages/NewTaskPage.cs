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
}