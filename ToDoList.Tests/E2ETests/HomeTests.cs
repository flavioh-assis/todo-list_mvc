using System;
using System.Threading;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ToDoList.App.Data.Context;
using ToDoList.Tests.Drivers;
using ToDoList.Tests.Factories;
using Xunit;

namespace ToDoList.Tests.E2ETests;

public class HomeTests : IDisposable
{
    private readonly IWebDriver _driver;
    private readonly TaskContext _dbContext;
    private readonly WebServerDriver _server;

    private readonly int _serverPort;

    public HomeTests()
    {
        _server = new WebServerDriver();
        _serverPort = _server.Port;

        var options = new ChromeOptions
        {
            AcceptInsecureCertificates = true
        };
        _driver = new ChromeDriver(options);

        _dbContext = new TaskContextFactory()
            .CreateDbContext(Array.Empty<string>());

        var isConnected = CheckDbConnection();
        if (!isConnected)
        {
            Dispose();
            throw new Exception("Failed to connect to database.");
        }

        _server.Start();
    }

    [Fact]
    public void Test()
    {
        _driver.Navigate().GoToUrl($"http://localhost:{_serverPort}");

        var title = _driver.Title;
        title.Should().Be("Tarefas Pendentes - Lista de Tarefas");
    }

    private bool CheckDbConnection()
    {
        var startTime = DateTime.Now;
        var timeout = TimeSpan.FromSeconds(10);

        while (DateTime.Now - startTime < timeout)
        {
            try
            {
                _dbContext.Database.EnsureCreated();
                return true;
            }
            catch
            {
                Thread.Sleep(1000);
            }
        }

        return false;
    }

    public void Dispose()
    {
        _driver.Quit();
        _driver.Dispose();

        _server.Stop().Wait();
    }
}