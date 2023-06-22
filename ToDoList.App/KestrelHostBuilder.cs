namespace ToDoList.App;

public class KestrelHostBuilder
{
    public WebApplicationBuilder CreateHostBuilder(string[] args, string webRoot = null)
    {
        var options = new WebApplicationOptions
        {
            Args = args,
            ApplicationName = typeof(Program).Assembly.FullName,
        };

        return WebApplication.CreateBuilder(options);
    }
}