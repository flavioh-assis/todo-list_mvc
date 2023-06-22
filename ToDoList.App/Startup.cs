using Microsoft.EntityFrameworkCore;
using ToDoList.App.Data.Context;
using ToDoList.App.Repository;
using ToDoList.App.Repository.Base;
using ToDoList.App.Repository.Interfaces;
using ToDoList.App.Services;
using ToDoList.App.Services.Interfaces;

namespace ToDoList.App
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services, string connectionString = null)
        {
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddScoped(typeof(ITaskService), typeof(TaskService));
            services.AddScoped<DbContext, TaskContext>();

            services.AddControllersWithViews();
            services.AddDbContext<TaskContext>(
                opt =>
                    opt.UseSqlServer(_configuration.GetConnectionString(connectionString ?? "DefaultConnection"))
            );
            services.AddScoped<ITaskContext, TaskContext>();
            services.AddTransient<ITaskRepository, TaskRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Task/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Task}/{action=Index}/{id?}");
            });
        }
    }
}