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

    private string GetCardSelectorString(string taskTitle)
    {
        return $"//{CardXPath}//h5[text()='{taskTitle}']/ancestor::{CardXPath}";
    }

    private string GetButtonSelectorString(string buttonText)
    {
        return $"//button[text()='{buttonText}']";
    }

    private string GetModalCompleteSelectorString(int taskId)
    {
        return $"//form[@id='complete-{taskId}']";
    }

    private IWebElement ButtonOnModalComplete(int taskId, string buttonText)
    {
        var buttonSelector = By.XPath(
            $"{GetModalCompleteSelectorString(taskId)}{GetButtonSelectorString(buttonText)}"
        );

        return WaitToBeClickable(buttonSelector);
    }

    private IWebElement ButtonCompleteOnCard(string taskTitle)
    {
        var selector = By.XPath(
            $"{GetCardSelectorString(taskTitle)}{GetButtonSelectorString("Concluir")}"
        );

        return WaitToBeClickable(selector);
    }

    public void ClickToCompleteTask(string taskTitle)
    {
        var buttonComplete = ButtonCompleteOnCard(taskTitle);
        buttonComplete.Click();
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