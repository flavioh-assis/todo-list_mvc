using OpenQA.Selenium;

namespace ToDoList.Tests.Utils;

public class BasePage
{
    private readonly IWebDriver _driver;
    private const string BaseUrl = "http://localhost";
    private const int ServerPort = 32000;

    protected BasePage(IWebDriver driver)
    {
        _driver = driver;
    }

    public void NavigateToHome()
    {
        _driver.Navigate().GoToUrl($"{BaseUrl}:{ServerPort}");
    }
}