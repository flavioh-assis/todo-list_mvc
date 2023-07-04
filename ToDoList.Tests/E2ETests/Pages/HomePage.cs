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

    private string GetModalDeleteSelectorString(int taskId)
    {
        return $"//form[@id='delete-{taskId}']";
    }

    private IWebElement ButtonOnCard(string buttonText, string taskTitle)
    {
        var cardSelector = GetCardSelectorString(taskTitle);
        var buttonCompleteSelector = GetButtonSelectorString(buttonText);

        return WaitToBeClickable(By.XPath(
            $"{cardSelector}{buttonCompleteSelector}")
        );
    }

    private IWebElement ButtonOnModalComplete(int taskId, string buttonText)
    {
        var buttonSelector = By.XPath(
            $"{GetModalCompleteSelectorString(taskId)}{GetButtonSelectorString(buttonText)}"
        );

        return WaitToBeClickable(buttonSelector);
    }

    private IWebElement ButtonOnModalDelete(int taskId, string buttonText)
    {
        var buttonSelector = By.XPath(
            $"{GetModalDeleteSelectorString(taskId)}{GetButtonSelectorString(buttonText)}"
        );

        return WaitToBeClickable(buttonSelector);
    }

    public IWebElement CardBody(string taskTitle)
    {
        var cardSelector = GetCardSelectorString(taskTitle);
        var cardBodySelector = GetCardBodySelectorString();

        return WaitToBeVisible(By.XPath(
            $"{cardSelector}{cardBodySelector}")
        );
    }

    public void ClickToCompleteTask(string taskTitle)
    {
        var buttonComplete = ButtonOnCard("Concluir", taskTitle);
        buttonComplete.Click();
    }

    public void ClickToDeleteTask(string taskTitle)
    {
        var buttonDelete = ButtonOnCard("Excluir", taskTitle);
        buttonDelete.Click();
    }

    public void ClickToEditTask(string taskTitle)
    {
        var buttonEdit = ButtonOnCard("Editar", taskTitle);
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

    public void ClickOkOnModalDelete(int taskId)
    {
        var okButton = ButtonOnModalDelete(taskId, "Confirmar");
        okButton.Click();
    }

    public void ClickCancelOnModalDelete(int taskId)
    {
        var cancelButton = ButtonOnModalDelete(taskId, "Cancelar");
        cancelButton.Click();
    }
}