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

    private const string CardXPath = "div[contains(@class, 'card')]";

    protected SharedSelectors(IWebDriver driver) : base(driver)
    {
        _driver = driver;
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(WaitTimeInSeconds));
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

    public IList<IWebElement> Cards()
    {
        return _driver.FindElements(By.ClassName("card"));
    }

    public IWebElement CardByTitle(string taskTitle)
    {
        return _driver.FindElement(
            By.XPath($"//{CardXPath}//h5[text()='{taskTitle}']/ancestor::{CardXPath}")
        );
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

    public void CompleteTask(string taskTitle)
    {
        var card = CardByTitle(taskTitle);

        ClickButtonOnElement(card, "Concluir");
    }

    public IWebElement ModalComplete(int taskId)
    {
        return _driver.FindElement(By.Id($"complete-{taskId}"));
    }

    public void ClickOkOnModalComplete(int taskId)
    {
        var modalComplete = ModalComplete(taskId);
        ClickButtonOnElement(modalComplete, "Confirmar");
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

    public string CurrentUrl()
    {
        return _driver.Url;
    }

    public void WaitToBeClickable(IWebElement element)
    {
        _wait.Until(ExpectedConditions.ElementToBeClickable(element));
    }
}