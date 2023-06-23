using System;
using System.Threading;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ToDoList.App.Data.Context;
using ToDoList.Tests.Drivers;
using ToDoList.Tests.E2ETests.Pages;
using ToDoList.Tests.Factories;
using Xunit;

namespace ToDoList.Tests.E2ETests;

public class HomeTests : IDisposable
{
    private readonly IWebDriver _driver;
    private readonly TaskContext _dbContext;
    private readonly WebServerDriver _server;
    private readonly HomePage _page;

    public HomeTests()
    {
        _server = new WebServerDriver();

        var options = new ChromeOptions
        {
            AcceptInsecureCertificates = true,
        };
        options.AddArgument("--headless=new");

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

        _page = new HomePage(_driver);
        var pageUrl = $"http://localhost:{_server.Port}{_page.Path}";

        _driver.Navigate().GoToUrl(pageUrl);
    }

    [Fact]
    public void ShouldDisplayPageTitleInBrowserTab()
    {
        var expectedTitle = "Tarefas Pendentes - Lista de Tarefas";

        var title = _page.Title();

        title.Should().Be(expectedTitle);
    }

    [Fact]
    public void ShouldDisplayNavigationBar()
    {
        var navbar = _page.GetNavigationBar();

        navbar.Should().NotBeNull();
    }

    [Fact]
    public void ShouldDisplayThreeItemsIntoNavigationBar()
    {
        var expectedLength = 3;

        var navbarItems = _page.GetNavigationItems();

        navbarItems.Count.Should().Be(expectedLength);
    }

    [Fact]
    public void ShouldDisplayCorrectItemsTextIntoNavigationBar()
    {
        var expectedFirstItemText = "Tarefas Pendentes";
        var expectedSecondItemText = "Tarefas Concluídas";
        var expectedThirdItemText = "Criar Nova Tarefa";

        var navbarItems = _page.GetNavigationItems();
        var firstItemText = navbarItems[navbarItems.Count - 3].Text;
        var secondItemText = navbarItems[navbarItems.Count - 2].Text;
        var thirdItemText = navbarItems[navbarItems.Count - 1].Text;

        firstItemText.Should().Be(expectedFirstItemText);
        secondItemText.Should().Be(expectedSecondItemText);
        thirdItemText.Should().Be(expectedThirdItemText);
    }

    [Fact]
    public void ShouldDisplayHeadingText()
    {
        var expectedHeadingText = "Pendentes";

        var heading = _page.Heading();

        heading.Text.Should().Be(expectedHeadingText);
    }

    private bool CheckDbConnection()
    {
        var startTime = DateTime.Now;
        var timeout = TimeSpan.FromSeconds(10);
        var waitTime = 1000;

        while (DateTime.Now - startTime < timeout)
        {
            try
            {
                _dbContext.Database.EnsureCreated();
                return true;
            }
            catch
            {
                Thread.Sleep(waitTime);
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