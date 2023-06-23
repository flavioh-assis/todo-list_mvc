using System;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.Tests.Utils;

public abstract class DbHelper
{
    public static bool CheckDbConnection(DbContext dbContext)
    {
        var startTime = DateTime.Now;
        var timeout = TimeSpan.FromSeconds(10);
        var waitTime = 1000;

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
}