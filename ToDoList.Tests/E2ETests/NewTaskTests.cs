using System;
using FluentAssertions;
using OpenQA.Selenium;
using ToDoList.App.Data.Context;
using ToDoList.Tests.Drivers;
using ToDoList.Tests.E2ETests.Pages;
using ToDoList.Tests.Factories;
using ToDoList.Tests.Utils;
using Xunit;

namespace ToDoList.Tests.E2ETests;

public class NewTaskTests : IDisposable
{
    private readonly IWebDriver? _driver;
    private readonly TaskContext _dbContext;
    private readonly WebServerDriver _server;
    private readonly NewTaskPage _page;
    private readonly string _serverUrl;

    private const string TestDatabase = "db_e2e_new_task_tests";
    private const bool Headless = true;

    public NewTaskTests()
    {
        _server = new WebServerDriver();
        _server.Start(TestDatabase);
        _serverUrl = $"{_server.BaseUrl}:{_server.Port}";

        _dbContext = new TaskContextFactory()
            .CreateDbContext(new[] { _server.TestConnectionString });

        var isConnected = DbHelper.CheckDbConnection(_dbContext);
        if (!isConnected)
        {
            Dispose();
            throw new Exception("Failed to connect to database.");
        }

        _driver = new ChromeDriverFactory().CreateWebDriver(Headless);

        _page = new NewTaskPage(_driver);
        _page.NavigateToNewTask();
    }

    [Fact]
    public void ShouldRedirectToHomePage_WhenEnterValidTitle()
    {
        var currentUrl = _page.CurrentUrl();
        var expectedUrl = $"{_serverUrl}/";

        _page.EnterTitle("Task Title");
        _page.ClickCreateTask();
        _page.WaitUrlToChange(currentUrl);

        _page.CurrentUrl().Should().Be(expectedUrl);
    }

    [Fact]
    public void ShouldRedirectToHomePage_WhenClickOnCancel()
    {
        var currentUrl = _page.CurrentUrl();
        var expectedUrl = $"{_serverUrl}/";

        _page.ClickCancel();
        _page.WaitUrlToChange(currentUrl);

        _page.CurrentUrl().Should().Be(expectedUrl);
    }

    [Fact]
    public void ShouldNotRedirectToHomePage_WhenEnterInvalidTitle()
    {
        var expectedUrl = _page.CurrentUrl();

        _page.EnterTitle("");
        _page.ClickCreateTask();

        _page.CurrentUrl().Should().Be(expectedUrl);
    }

    public void Dispose()
    {
        _driver?.Dispose();

        DbHelper.Dispose(_dbContext);

        _server.Stop().Wait();
    }
}