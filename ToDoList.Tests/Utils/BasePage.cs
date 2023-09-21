using OpenQA.Selenium;

namespace ToDoList.Tests.Utils;

public class BasePage
{
    private readonly IWebDriver _driver;
    private const string BaseUrl = "http://localhost";
    private const int ServerPort = 32000;

    private const string HomeUrl = "/";
    private const string NewTaskUrl = "/Task/Create";
    private const string EditTaskUrl = "/Task/Edit";

    protected BasePage(IWebDriver driver)
    {
        _driver = driver;
    }

    private void NavigateTo(string url)
    {
        _driver.Navigate().GoToUrl(
            $"{BaseUrl}:{ServerPort}{url}"
        );
    }

    public void NavigateToHome()
    {
        NavigateTo(HomeUrl);
    }

    public void NavigateToNewTask()
    {
        NavigateTo(NewTaskUrl);
    }

    public void NavigateToEditTask(int taskId)
    {
        NavigateTo($"{EditTaskUrl}/{taskId}");
    }
}