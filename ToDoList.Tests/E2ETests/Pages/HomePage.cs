using OpenQA.Selenium;
using ToDoList.Tests.Utils;

namespace ToDoList.Tests.E2ETests.Pages;

public class HomePage : SharedSelectors
{
    private readonly IWebDriver _driver;
    private const string CardXPath = "div[contains(@class, 'card')]";

    public HomePage(IWebDriver driver) : base(driver)
    {
        _driver = driver;
    }

    private string GetButtonSelectorString(string buttonText)
    {
        return $"//button[text()='{buttonText}']";
    }

    private string GetCardSelectorString(string taskTitle)
    {
        return $"//{CardXPath}//h5[text()='{taskTitle}']/ancestor::{CardXPath}";
    }

    private string GetModalCompleteSelectorString(int taskId)
    {
        return $"//form[@id='complete-{taskId}']";
    }

    private IWebElement ButtonCompleteOnCard(string taskTitle)
    {
        var selector = By.XPath(
            $"{GetCardSelectorString(taskTitle)}{GetButtonSelectorString("Concluir")}"
        );

        return WaitToBeClickable(selector);
    }

    private IWebElement ButtonDeleteOnCard(string taskTitle)
    {
        var selector = By.XPath(
            $"{GetCardSelectorString(taskTitle)}{GetButtonSelectorString("Excluir")}"
        );

        return WaitToBeClickable(selector);
    }

    private IWebElement ButtonEditOnCard(string taskTitle)
    {
        var selector = By.XPath(
            $"{GetCardSelectorString(taskTitle)}{GetButtonSelectorString("Editar")}"
        );

        return WaitToBeClickable(selector);
    }

    private IWebElement ButtonOnModalComplete(int taskId, string buttonText)
    {
        var buttonSelector = By.XPath(
            $"{GetModalCompleteSelectorString(taskId)}{GetButtonSelectorString(buttonText)}"
        );

        return WaitToBeClickable(buttonSelector);
    }

    public void ClickToCompleteTask(string taskTitle)
    {
        var buttonComplete = ButtonCompleteOnCard(taskTitle);
        buttonComplete.Click();
    }

    public void ClickToDeleteTask(string taskTitle)
    {
        var buttonDelete = ButtonDeleteOnCard(taskTitle);
        buttonDelete.Click();
    }

    public void ClickToEditTask(string taskTitle)
    {
        var buttonEdit = ButtonEditOnCard(taskTitle);
        buttonEdit.Click();
    }

    public void ClickOkOnModalComplete(int taskId)
    {
        var okButton = ButtonOnModalComplete(taskId, "Confirmar");
        okButton.Click();
    }

    public void ClickCancelOnModalComplete(int taskId)
    {
        var cancelButton = ButtonOnModalComplete(taskId, "Cancelar");
        cancelButton.Click();
    }
}