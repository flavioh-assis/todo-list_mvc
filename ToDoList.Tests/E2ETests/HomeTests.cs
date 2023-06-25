using System;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ToDoList.App.Data.Context;
using ToDoList.Tests.Drivers;
using ToDoList.Tests.E2ETests.Pages;
using ToDoList.Tests.Factories;
using ToDoList.Tests.Utils;
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
        _server.Start("TestConnection");

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
        options.AddArgument("--headless=new");
        _driver = new ChromeDriver(options);

        _page = new HomePage(_driver);
        _page.NavigateToHome();
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

    public void Dispose()
    {
        _driver.Quit();
        _driver.Dispose();

        DbHelper.Dispose(_dbContext);

        _server.Stop().Wait();
    }
}