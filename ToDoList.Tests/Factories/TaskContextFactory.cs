using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ToDoList.App.Data.Context;

namespace ToDoList.Tests.Factories;

public class TaskContextFactory : IDesignTimeDbContextFactory<TaskContext>
{
    public TaskContext CreateDbContext(string[] args)
    {
        var connectionString = args[0];

        var optionsBuilder = new DbContextOptionsBuilder<TaskContext>();
        optionsBuilder.UseSqlServer(
            connectionString
        );

        return new TaskContext(optionsBuilder.Options);
    }
}