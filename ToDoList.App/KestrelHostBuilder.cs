namespace ToDoList.App;

public class KestrelHostBuilder
{
    public WebApplicationBuilder CreateHostBuilder(string[] args, string webRoot = null)
    {
        webRoot ??= Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        var options = new WebApplicationOptions
        {
            Args = args,
            ApplicationName = typeof(Program).Assembly.FullName,
            WebRootPath = webRoot,
        };

        return WebApplication.CreateBuilder(options);
    }
}