using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ToDoList.Tests.Factories;

public class ChromeDriverFactory
{
    private IWebDriver? _driver;

    public IWebDriver CreateWebDriver(bool headless = false)
    {
        var options = new ChromeOptions
        {
            AcceptInsecureCertificates = true,
        };

        if (headless)
        {
            options.AddArgument("--headless");
        }

        _driver = new ChromeDriver(options);
        return _driver;
    }
}