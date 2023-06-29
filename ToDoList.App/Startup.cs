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
        public readonly string ConnectionString;

        public Startup(IConfiguration configuration, string testDatabase = null)
        {
            var defaultConString = configuration.GetConnectionString("DefaultConnection");

            if (testDatabase == null)
            {
                ConnectionString = defaultConString;
            }
            else
            {
                var testConnectionString = ReplaceString(defaultConString, testDatabase);
                ConnectionString = testConnectionString;
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddScoped(typeof(ITaskService), typeof(TaskService));
            services.AddScoped<DbContext, TaskContext>();

            services.AddControllersWithViews();
            services.AddDbContext<TaskContext>(
                opt => opt.UseSqlServer(ConnectionString)
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

        private string ReplaceString(string originalString, string testDatabase)
        {
            var defaultDatabase = "db_todo";

            return $"{originalString.Replace(defaultDatabase, testDatabase)}";
        }
    }
}