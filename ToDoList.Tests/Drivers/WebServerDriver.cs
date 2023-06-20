using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
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
        var location = typeof(KestrelHostBuilder).Assembly.Location;
        var applicationAssemblyPath = Path.GetDirectoryName(location);

        if (applicationAssemblyPath is null)
            throw new Exception("Location of application assembly could not be found");

        var webRoot = Path.Combine(applicationAssemblyPath, "..", "..", "..", "..",
            "ToDoList.App", "wwwroot");

        var hostBuilder = new KestrelHostBuilder();
        var builder = hostBuilder.CreateHostBuilder(Array.Empty<string>(), webRoot);

        builder.WebHost.ConfigureKestrel(options => { options.ListenAnyIP(Port); });

        _host = builder.Build();

        _host.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(webRoot)
        });

        _host.UseStaticFiles();

        _host.UseRouting();

        _host.UseAuthorization();

        _host.MapControllerRoute(
            name: "default",
            pattern: "{controller=Task}/{action=Index}/{id?}");

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