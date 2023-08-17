using ToDoList.App.ViewModels;

namespace ToDoList.Tests.Builders;

public class TaskViewModelBuilder
{
    private TaskViewModel _task = new();

    public TaskViewModelBuilder NewValidTask()
    {
        _task = new TaskViewModel
        {
            Title = "Task Title",
            Description = "Task description",
        };
        return this;
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

    public TaskViewModelBuilder WithTitleNull()
    {
        _task.Title = null;
        return this;
    }

    public TaskViewModelBuilder WithTitleTooSmall()
    {
        _task.Title = "T";
        return this;
    }

    public TaskViewModelBuilder WithTitleTooLarge()
    {
        _task.Title = "Huge_enormous_and_invalid_title";
        return this;
    }

    public TaskViewModelBuilder WithDescriptionTooLarge()
    {
        _task.Description =
            "Considerable_large_great_huge_immense_enormous_extensive_colossal_massive_sizeable_and_invalid_description";
        return this;
    }

    public TaskViewModel Build()
    {
        return _task;
    }
}