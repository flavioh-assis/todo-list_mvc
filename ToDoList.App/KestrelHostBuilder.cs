using Microsoft.EntityFrameworkCore;
using ToDoList.App.Data.Context;
using ToDoList.App.Repository;
using ToDoList.App.Repository.Base;
using ToDoList.App.Repository.Interfaces;
using ToDoList.App.Services;
using ToDoList.App.Services.Interfaces;

namespace ToDoList.App;

public class KestrelHostBuilder
{
    public WebApplicationBuilder CreateHostBuilder(string[] args, string webRoot = null)
    {
        // webRoot ??= Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        var options = new WebApplicationOptions
        {
            Args = args,
            ApplicationName = typeof(Program).Assembly.FullName,
            // ContentRootPath = Directory.GetCurrentDirectory(),
            // EnvironmentName = Environments.Development,
            // WebRootPath = webRoot,
        };

        var builder = WebApplication.CreateBuilder(options);

        builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
        builder.Services.AddScoped(typeof(ITaskService), typeof(TaskService));

        builder.Services.AddControllersWithViews();
        builder.Services.AddDbContext<TaskContext>(opt =>
            opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddTransient<ITaskRepository, TaskRepository>();

        return builder;
    }
}