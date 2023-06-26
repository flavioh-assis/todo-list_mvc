namespace ToDoList.App;

public class KestrelHostBuilder
{
    public static WebApplicationBuilder CreateHostBuilder(string[] args)
    {
        var webRoot = args.Length > 0 ? args[0] : null;
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