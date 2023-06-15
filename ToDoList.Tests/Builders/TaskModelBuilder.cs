using System;
using ToDoList.App.Models;

namespace ToDoList.Tests.Builders;

public class TaskModelBuilder
{
    private TaskModel _task = new TaskModel();

    public TaskModelBuilder()
    {
        Reset();
    }

    private void Reset()
    {
        _task = new TaskModel
        {
            Title = "Title",
            Description = "Description",
            CreatedAt = DateTime.UtcNow
        };
    }
    
    public TaskModelBuilder WithTitle(string title)
    {
        _task.Title = title;
        return this;
    }

    public TaskModelBuilder WithDescription(string description)
    {
        _task.Description = description;
        return this;
    }

    public TaskModelBuilder Pending()
    {
        _task.CompletedAt = null;
        return this;
    }

    public TaskModelBuilder Completed()
    {
        _task.CompletedAt = DateTime.UtcNow;
        return this;
    }

    public TaskModel Build()
    {
        var builtTask = _task;
        Reset();

        return builtTask;
    }
}