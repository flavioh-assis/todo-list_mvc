using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using ToDoList.App;

namespace ToDoList.Tests.Drivers;

public class WebServerDriver
{
    private WebApplication? _host;
    private const int Port = 32000;
    private const string BaseUrl = "http://localhost";
    public string TestConnectionString { get; private set; }

    public WebServerDriver()
    {
        TestConnectionString = "";
    }

    public void Start(string connection)
    {
        var location = typeof(KestrelHostBuilder).Assembly.Location;
        var applicationAssemblyPath = Path.GetDirectoryName(location);

        if (applicationAssemblyPath is null)
            throw new Exception("Location of application assembly could not be found.");

        var webRoot = Path.Combine(
            applicationAssemblyPath, "..", "..", "..", "..", "ToDoList.App", "wwwroot"
        );

        var builder = new KestrelHostBuilder().CreateHostBuilder(Array.Empty<string>(), webRoot);
        TestConnectionString = builder.Configuration.GetConnectionString(connection);

        var startup = new Startup(builder.Configuration);
        startup.ConfigureServices(builder.Services, connection);

        _host = builder.Build();

        startup.Configure(_host, _host.Environment);
        _host.RunAsync($"{BaseUrl}:{Port}").ConfigureAwait(false);
    }

    public async Task Stop()
    {
        if (_host is not null) await _host.StopAsync();
    }
}