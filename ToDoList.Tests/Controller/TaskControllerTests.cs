using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ToDoList.App.Controllers;
using ToDoList.App.Models;
using ToDoList.App.Services.Interfaces;
using Xunit;

namespace ToDoList.Test.Controller;

public class TaskControllerTests
{
    private readonly TaskController _taskController;
    private readonly ITaskService _taskService;

    public TaskControllerTests()
    {
        var logger = A.Fake<ILogger<TaskController>>();
        _taskService = A.Fake<ITaskService>();

        _taskController = new TaskController(logger, _taskService);
    }

    [Fact]
    public async void Index_ShouldReturnViewResult()
    {
        var pendingTasks = A.Fake<List<TaskModel>>();
        A.CallTo(() => _taskService.GetAllPending()).Returns(pendingTasks);

        var result = await _taskController.Index();

        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public async void Index_ShouldReturnPendingTasks()
    {
        var pendingTasks = A.Fake<List<TaskModel>>();
        A.CallTo(() => _taskService.GetAllPending()).Returns(pendingTasks);

        var result = await _taskController.Index();

        result.As<ViewResult>().ViewData.Model.Should().Be(pendingTasks);
    }
}