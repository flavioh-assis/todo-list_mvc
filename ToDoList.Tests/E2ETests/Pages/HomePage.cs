using OpenQA.Selenium;
using ToDoList.Tests.Utils;

namespace ToDoList.Tests.E2ETests.Pages;

public class HomePage : SharedSelectors
{
    private readonly IWebDriver _driver;
    public string Path { get; private set; }

    public HomePage(IWebDriver driver) : base(driver)
    {
        _driver = driver;
        Path = "/Task/Index";
    }
}