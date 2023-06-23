using System.Collections.Generic;
using OpenQA.Selenium;

namespace ToDoList.Tests.Utils;

public class SharedSelectors : BasePage
{
    private readonly IWebDriver _driver;

    protected SharedSelectors(IWebDriver driver) : base(driver)
    {
        _driver = driver;
    }

    public IWebElement GetNavigationBar()
    {
        return _driver.FindElement(By.TagName("nav"));
    }

    public IList<IWebElement> GetNavigationItems()
    {
        return _driver.FindElements(By.XPath("//header//nav//ul/li"));
    }

    public IWebElement Heading()
    {
        return _driver.FindElement(By.TagName("h1"));
    }

    public string Title()
    {
        return _driver.Title;
    }
}