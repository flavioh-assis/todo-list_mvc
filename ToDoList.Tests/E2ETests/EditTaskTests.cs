using System;
using FluentAssertions;
using OpenQA.Selenium;
using ToDoList.App.Data.Context;
using ToDoList.Tests.Builders;
using ToDoList.Tests.Drivers;
using ToDoList.Tests.E2ETests.Pages;
using ToDoList.Tests.Factories;
using ToDoList.Tests.Utils;
using Xunit;

namespace ToDoList.Tests.E2ETests;

public class EditTaskTests : IDisposable
{
    private readonly IWebDriver? _driver;
    private readonly TaskContext _dbContext;
    private readonly WebServerDriver _server;
    private readonly EditTaskPage _page;
    private readonly string _serverUrl;

    private const string TestDatabase = "db_e2e_edit_task_tests";
    private const bool Headless = true;

    public EditTaskTests()
    {
        var taskBuilder = new TaskModelBuilder();

        _server = new WebServerDriver(TestDatabase);
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

        var task1Pending = taskBuilder
            .WithTitle("Task 1")
            .Pending()
            .Build();

        DbHelper.InsertTask(_dbContext, task1Pending).Wait();

        _page = new EditTaskPage(_driver);
        _page.NavigateToEditTask(task1Pending.Id);
    }

    [Fact]
    public void ShouldRedirectToHomePage_WhenEnteringValidData()
    {
        var expectedUrl = $"{_serverUrl}/";

        _page.EnterTitle("New Title");
        _page.ClickSave();

        _page.CurrentUrl().Should().Be(expectedUrl);
    }

    [Fact]
    public void ShouldRedirectToHomePage_WhenClickingOnCancel()
    {
        var expectedUrl = $"{_serverUrl}/";

        _page.ClickCancel();

        _page.CurrentUrl().Should().Be(expectedUrl);
    }

    public void Dispose()
    {
        _driver?.Dispose();

        DbHelper.Dispose(_dbContext);

        _server.Stop().Wait();
    }
}