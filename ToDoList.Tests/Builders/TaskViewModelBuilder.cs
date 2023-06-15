using ToDoList.App.ViewModels;

namespace ToDoList.Tests.Builders;

public class TaskViewModelBuilder
{
    private TaskViewModel _task = new TaskViewModel();

    public TaskViewModelBuilder()
    {
        Reset();
    }

    private void Reset()
    {
        _task = new TaskViewModel
        {
            Title = "Title",
            Description = "Description",
        };
    }

    public TaskViewModelBuilder WithTitle(string title)
    {
        _task.Title = title;
        return this;
    }

    public TaskViewModelBuilder WithDescription(string description)
    {
        _task.Description = description;
        return this;
    }

    public TaskViewModel Build()
    {
        var builtTask = _task;
        Reset();

        return builtTask;
    }
}