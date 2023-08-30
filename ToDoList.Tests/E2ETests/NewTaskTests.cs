using System;
using System.Collections.Generic;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ToDoList.App.Data.Context;
using ToDoList.Tests.Builders;
using ToDoList.Tests.Drivers;
using ToDoList.Tests.E2ETests.Pages;
using ToDoList.Tests.Factories;
using ToDoList.Tests.Utils;
using Xunit;

namespace ToDoList.Tests.E2ETests;

public class NewTaskTests : IDisposable
{
    private readonly IWebDriver _driver;
    private readonly TaskContext _dbContext;
    private readonly WebServerDriver _server;
    private readonly NewTaskPage _page;
    private readonly string _serverUrl;

    private const string TestDatabase = "db_e2e_new_task_tests";

    public NewTaskTests()
    {
        var taskBuilder = new TaskModelBuilder();

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

        var options = new ChromeOptions
        {
            AcceptInsecureCertificates = true,
        };
        // options.AddArgument("--headless=new");
        _driver = new ChromeDriver(options);

        _page = new NewTaskPage(_driver);
        _page.NavigateToNewTask();
    }

    [Fact]
    public void Test()
    {
    }

    public void Dispose()
    {
        _driver?.Quit();
        _driver?.Dispose();

        DbHelper.Dispose(_dbContext);

        _server.Stop().Wait();
    }
}