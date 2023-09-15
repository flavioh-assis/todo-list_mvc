using System;
using System.Collections.Generic;
using FluentAssertions;
using OpenQA.Selenium;
using ToDoList.App.Data.Context;
using ToDoList.App.Models;
using ToDoList.Tests.Builders;
using ToDoList.Tests.Drivers;
using ToDoList.Tests.E2ETests.Pages;
using ToDoList.Tests.Factories;
using ToDoList.Tests.Utils;
using Xunit;

namespace ToDoList.Tests.E2ETests;

public class HomeTests : IDisposable
{
    private readonly IWebDriver? _driver;
    private readonly TaskContext _dbContext;
    private readonly WebServerDriver _server;
    private readonly HomePage _page;
    private readonly string _serverUrl;

    private readonly TaskModel _task1Pending;
    private readonly TaskModel _task2Pending;
    private readonly int _totalPendingTask;

    private const string TestDatabase = "db_e2e_home_tests";
    private const bool Headless = true;

    public HomeTests()
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

        _task1Pending = taskBuilder
            .WithTitle("Task 1")
            .WithDescription("Description of task 1")
            .Pending()
            .Build();

        _task2Pending = taskBuilder
            .WithTitle("Task 2")
            .WithDescription("Description of task 2")
            .Pending()
            .Build();

        var pendingTasks = new List<TaskModel>
        {
            _task1Pending,
            _task2Pending,
        };

        DbHelper.InsertTasks(_dbContext, pendingTasks).Wait();

        _totalPendingTask = pendingTasks.Count;

        _driver = new ChromeDriverFactory().CreateWebDriver(Headless);

        _page = new HomePage(_driver);
        _page.NavigateToHome();
    }

    [Fact]
    public void WhenOptionPendingIsClickedOnNavigationBar_ShouldRedirectToPagePending()
    {
        var expectedUrl = $"{_serverUrl}/";

        _page.SelectPendingItemOnNavigationBar();

        var currentUrl = _page.CurrentUrl();
        currentUrl.Should().Be(expectedUrl);
    }

    [Fact]
    public void WhenOptionCompletedIsClickedOnNavigationBar_ShouldRedirectToPageCompleted()
    {
        var expectedUrl = $"{_serverUrl}/Task/Completed";

        _page.SelectCompletedItemOnNavigationBar();

        var currentUrl = _page.CurrentUrl();
        currentUrl.Should().Be(expectedUrl);
    }

    [Fact]
    public void WhenOptionNewTaskIsClickedOnNavigationBar_ShouldRedirectToPageNewTask()
    {
        var expectedUrl = $"{_serverUrl}/Task/Create";

        _page.SelectCreateNewTaskItemOnNavigationBar();

        var currentUrl = _page.CurrentUrl();
        currentUrl.Should().Be(expectedUrl);
    }

    [Fact]
    public void WhenCompletingTask_ShouldRedirectToPageCompleted()
    {
        var taskId = _task1Pending.Id;
        var taskTitle = _task1Pending.Title;
        var expectedUrl = $"{_serverUrl}/Task/Completed";

        _page.ClickToCompleteTask(taskTitle);
        _page.ClickOkOnModalComplete(taskId);

        var currentUrl = _page.CurrentUrl();
        currentUrl.Should().Be(expectedUrl);
    }

    [Fact]
    public void WhenCompletingTask_ShouldChangeStatusToCompleted()
    {
        var taskId = _task2Pending.Id;
        var taskTitle = _task2Pending.Title;
        var expectedBodyText = "Status: Concluída";

        _page.ClickToCompleteTask(taskTitle);
        _page.ClickOkOnModalComplete(taskId);

        var cardBody = _page.CardBody(taskTitle);
        cardBody.Text.Should().Contain(expectedBodyText);
    }

    [Fact]
    public void WhenTaskCompletionIsAborted_ShouldNotRedirect()
    {
        var taskId = _task1Pending.Id;
        var taskTitle = _task1Pending.Title;
        var expectedUrl = $"{_serverUrl}/";

        _page.ClickToCompleteTask(taskTitle);
        _page.ClickCancelOnModalComplete(taskId);

        var currentUrl = _page.CurrentUrl();
        currentUrl.Should().Be(expectedUrl);
    }

    [Fact]
    public void WhenEditingTask_ShouldRedirectToPageEdit()
    {
        var taskId = _task2Pending.Id;
        var taskTitle = _task2Pending.Title;
        var expectedUrl = $"{_serverUrl}/Task/Edit/{taskId}";

        _page.ClickToEditTask(taskTitle);

        var currentUrl = _page.CurrentUrl();
        currentUrl.Should().Be(expectedUrl);
    }

    [Fact]
    public void WhenDeletingTask_ShouldNotDisplayRemovedTask()
    {
        var taskId = _task1Pending.Id;
        var taskTitle = _task1Pending.Title;
        var expectedLength = _totalPendingTask - 1;

        _page.ClickToDeleteTask(taskTitle);
        _page.ClickOkOnModalDelete(taskId);

        var taskCardsElements = _page.Cards();
        taskCardsElements.Count.Should().Be(expectedLength);
    }

    [Fact]
    public void WhenTaskDeletionIsAborted_ShouldNotRedirect()
    {
        var taskId = _task1Pending.Id;
        var taskTitle = _task1Pending.Title;
        var expectedUrl = $"{_serverUrl}/";

        _page.ClickToDeleteTask(taskTitle);
        _page.ClickCancelOnModalDelete(taskId);

        var currentUrl = _page.CurrentUrl();
        currentUrl.Should().Be(expectedUrl);
    }

    public void Dispose()
    {
        _driver?.Dispose();

        DbHelper.Dispose(_dbContext);

        _server.Stop().Wait();
    }
}