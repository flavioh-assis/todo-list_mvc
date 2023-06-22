using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using ToDoList.App;

namespace ToDoList.Tests.Drivers;

public class WebServerDriver
{
    private WebApplication? _host;
    public int Port { get; private set; }

    public WebServerDriver()
    {
        GeneratePort();
    }

    public void Start()
    {
        var builder = new KestrelHostBuilder().CreateHostBuilder(Array.Empty<string>());

        var startup = new Startup(builder.Configuration);
        startup.ConfigureServices(builder.Services, "TestConnection");

        _host = builder.Build();

        startup.Configure(_host, _host.Environment);
        _host.RunAsync($"http://localhost:{Port}").ConfigureAwait(false);
    }

    public async Task Stop()
    {
        if (_host is not null) await _host.StopAsync();
    }

    private void GeneratePort()
    {
        Port = new Random().Next(5000, 32000);
    }
}