using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoList.App.Models;

namespace ToDoList.Tests.Utils;

public abstract class DbHelper
{
    public static bool CheckDbConnection(DbContext dbContext)
    {
        var startTime = DateTime.Now;
        var timeout = TimeSpan.FromSeconds(10);
        var waitTime = 1000;

        dbContext.Database.EnsureDeleted();

        while (DateTime.Now - startTime < timeout)
        {
            try
            {
                dbContext.Database.EnsureCreated();
                return true;
            }
            catch
            {
                Thread.Sleep(waitTime);
            }
        }

        return false;
    }

    public static void Dispose(DbContext dbContext)
    {
        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }

    public static async Task InsertTasks(DbContext dbContext, List<TaskModel> tasks)
    {
        tasks.ForEach(task => dbContext.Add(task));
        await dbContext.SaveChangesAsync();
    }
}