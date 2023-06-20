using Microsoft.EntityFrameworkCore;
using ToDoList.App;
using ToDoList.App.Data.Context;
using ToDoList.App.Repository;
using ToDoList.App.Repository.Base;
using ToDoList.App.Repository.Interfaces;
using ToDoList.App.Services;
using ToDoList.App.Services.Interfaces;

var builder = new KestrelHostBuilder().CreateHostBuilder(args);

builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
builder.Services.AddScoped(typeof(ITaskService), typeof(TaskService));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<TaskContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<ITaskRepository, TaskRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Task/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Task}/{action=Index}/{id?}");

app.Run();