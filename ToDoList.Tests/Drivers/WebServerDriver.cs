using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using ToDoList.App;

namespace ToDoList.Tests.Drivers;

public class WebServerDriver
{
    private WebApplication? _host;
    public int Port { get; }
    public string BaseUrl { get; }
    public string TestConnectionString { get; private set; }

    public WebServerDriver(string testDatabase)
    {
        BaseUrl = "http://localhost";
        Port = 32000;
        TestConnectionString = "";

        Start(testDatabase);
    }

    public void Start(string testDatabase)
    {
        var location = typeof(KestrelHostBuilder).Assembly.Location;
        var applicationAssemblyPath = Path.GetDirectoryName(location);

        if (applicationAssemblyPath is null)
            throw new Exception("Location of application assembly could not be found.");

        var webRoot = Path.Combine(
            applicationAssemblyPath, "..", "..", "..", "..", "ToDoList.App", "wwwroot"
        );

        var builder = KestrelHostBuilder.CreateHostBuilder(new[] { webRoot });

        var startup = new Startup(builder.Configuration, testDatabase);
        startup.ConfigureServices(builder.Services);
        TestConnectionString = startup.ConnectionString;

        _host = builder.Build();

        startup.Configure(_host, _host.Environment);
        _host.RunAsync($"{BaseUrl}:{Port}").ConfigureAwait(false);
    }

    public async Task Stop()
    {
        if (_host is not null) await _host.StopAsync();
    }
}