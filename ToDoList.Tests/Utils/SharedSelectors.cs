using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ToDoList.Tests.Utils;

public class SharedSelectors : BasePage
{
    private readonly IWebDriver _driver;
    private const int WaitTimeInSeconds = 10;
    private readonly WebDriverWait _wait;

    protected SharedSelectors(IWebDriver driver) : base(driver)
    {
        _driver = driver;
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(WaitTimeInSeconds));
    }

    private IWebElement NavigationItem(string itemText)
    {
        var itemSelector = By.XPath($"//nav//a[contains(text(), '{itemText}')]");

        return WaitToBeClickable(itemSelector);
    }

    protected IWebElement WaitToBeClickable(By by)
    {
        return _wait.Until(ExpectedConditions.ElementToBeClickable(by));
    }

    protected IWebElement WaitToBeVisible(By by)
    {
        return _wait.Until(ExpectedConditions.ElementIsVisible(by));
    }

    protected string GetCardBodySelectorString()
    {
        return "//div[contains(@class, 'card-body')]";
    }

    public void SelectPendingItemOnNavigationBar()
    {
        var itemSubString = "Pendentes";

        var itemPending = NavigationItem(itemSubString);
        itemPending.Click();
    }

    public void SelectCompletedItemOnNavigationBar()
    {
        var itemSubString = "Concluídas";

        var optionCompleted = NavigationItem(itemSubString);
        optionCompleted.Click();
    }

    public void SelectCreateNewTaskItemOnNavigationBar()
    {
        var itemSubString = "Criar";

        var optionCreateNewTask = NavigationItem(itemSubString);
        optionCreateNewTask.Click();
    }

    public IWebElement Heading()
    {
        return WaitToBeVisible(By.TagName("h1"));
    }

    public IList<IWebElement> Cards()
    {
        return _driver.FindElements(By.ClassName("card"));
    }

    public string CurrentUrl()
    {
        return _driver.Url;
    }
}