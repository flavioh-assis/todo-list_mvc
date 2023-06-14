using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ToDoList.App.Data.Context;

namespace ToDoList.Test.Factories;

public class TaskContextFactory : IDesignTimeDbContextFactory<TaskContext>
{
    public TaskContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TaskContext>();
        optionsBuilder.UseSqlServer(
            "Server=localhost,1450;Database=db_todo_test;User ID=sa;Password=SqlServer2017!;TrustServerCertificate=True;"
        );

        return new TaskContext(optionsBuilder.Options);
    }
}