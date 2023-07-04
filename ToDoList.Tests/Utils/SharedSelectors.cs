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

    public void SelectPendingItemOnNavigationBar()
    {
        var itemSubString = "Pendentes";

        var itemPending = NavigationItem(itemSubString);
        itemPending.Click();
    }

    public void SelectCompletedItemOnNavigationBar()
    {
        var optionSubString = "Concluídas";

        var optionCompleted = NavigationItem(optionSubString);
        optionCompleted.Click();
    }

    public void SelectCreateNewTaskItemOnNavigationBar()
    {
        var optionSubString = "Criar";

        var optionCreateNewTask = NavigationItem(optionSubString);
        optionCreateNewTask.Click();
    }

    public IWebElement Heading()
    {
        return _driver.FindElement(By.TagName("h1"));
    }

    public IList<IWebElement> Cards()
    {
        return _driver.FindElements(By.ClassName("card"));
    }

    public IWebElement CardByTitle(string taskTitle)
    {
        var cardXPath = "div[contains(@class, 'card')]";
        var cardSelector = By.XPath(
            $"//{cardXPath}//h5[text()='{taskTitle}']/ancestor::{cardXPath}"
        );

        return WaitToBeVisible(cardSelector);
    }

    public IWebElement CardBody(IWebElement card)
    {
        return card.FindElement(By.ClassName("card-body"));
    }

    public IWebElement CardBodyByTitle(string taskTitle)
    {
        var card = CardByTitle(taskTitle);

        return CardBody(card);
    }

    public void ClickButtonOnElement(IWebElement element, string buttonText)
    {
        var button = element.FindElement(
            By.XPath($".//button[text()='{buttonText}']")
        );

        WaitToBeClickable(button);

        button.Click();
    }

    public void EditTask(string taskTitle)
    {
        var card = CardByTitle(taskTitle);

        ClickButtonOnElement(card, "Editar");
    }

    public void DeleteTask(string taskTitle)
    {
        var card = CardByTitle(taskTitle);

        ClickButtonOnElement(card, "Excluir");
    }

    public IWebElement ModalDelete(int taskId)
    {
        return _driver.FindElement(By.Id($"delete-{taskId}"));
    }

    public void ClickOkOnModalDelete(int taskId)
    {
        var modalDelete = ModalDelete(taskId);
        ClickButtonOnElement(modalDelete, "Confirmar");
    }

    public void ClickCancelOnModalDelete(int taskId)
    {
        var modalDelete = ModalDelete(taskId);
        ClickButtonOnElement(modalDelete, "Cancelar");
    }

    public string CurrentUrl()
    {
        return _driver.Url;
    }

    private IWebElement WaitToBeClickable(IWebElement element)
    {
        return _wait.Until(ExpectedConditions.ElementToBeClickable(element));
    }

    protected IWebElement WaitToBeVisible(By by)
    {
        return _wait.Until(ExpectedConditions.ElementIsVisible(by));
    }
}