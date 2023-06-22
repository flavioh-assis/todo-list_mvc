namespace ToDoList.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = new KestrelHostBuilder().CreateHostBuilder(args);

            var startup = new Startup(builder.Configuration);
            startup.ConfigureServices(builder.Services);

            var app = builder.Build();

            startup.Configure(app, app.Environment);

            app.Run();
        }
    }
}