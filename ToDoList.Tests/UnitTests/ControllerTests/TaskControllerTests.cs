using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using ToDoList.App.Controllers;
using ToDoList.App.Models;
using ToDoList.App.Services.Interfaces;
using ToDoList.App.ViewModels;
using Xunit;

namespace ToDoList.Tests.UnitTests.ControllerTests;

public class TaskControllerTests
{
    private readonly TaskController _taskController;
    private readonly ITaskService _taskService;
    private readonly ITempDataDictionary _fakeTempData = A.Fake<ITempDataDictionary>();

    public TaskControllerTests()
    {
        var logger = A.Fake<ILogger<TaskController>>();
        _taskService = A.Fake<ITaskService>();

        _taskController = new TaskController(logger, _taskService);
        _taskController.TempData = _fakeTempData;
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

    [Fact]
    public async void Completed_ShouldReturnViewResult()
    {
        var completedTasks = A.Fake<List<TaskModel>>();
        A.CallTo(() => _taskService.GetAllCompleted()).Returns(completedTasks);

        var result = await _taskController.Completed();

        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public async void Completed_ShouldReturnCompletedTasks()
    {
        var completedTasks = A.Fake<List<TaskModel>>();
        A.CallTo(() => _taskService.GetAllCompleted()).Returns(completedTasks);

        var result = await _taskController.Completed();

        result.As<ViewResult>().ViewData.Model.Should().Be(completedTasks);
    }

    [Fact]
    public void Create_ShouldReturnViewResult()
    {
        var result = _taskController.Create();

        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public async void Edit_WhenIdValid_ShouldReturnViewResult()
    {
        const int taskId = 1;
        var task = A.Fake<TaskModel>();
        A.CallTo(() => _taskService.GetById(taskId)).Returns(task);

        var result = await _taskController.Edit(taskId);

        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public async void Edit_WhenIdValid_ShouldReturnTask()
    {
        const int taskId = 1;
        var task = A.Fake<TaskModel>();
        A.CallTo(() => _taskService.GetById(taskId)).Returns(task);

        var result = await _taskController.Edit(taskId);

        result.As<ViewResult>().ViewData.Model.Should().Be(task);
    }

    [Fact]
    public async void Edit_WhenIdLessThanOne_ShouldRedirectToErrorPage()
    {
        const int invalidTaskId = 0;

        var result = await _taskController.Edit(invalidTaskId);

        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ActionName.Should().Be("Error");
    }

    [Fact]
    public async void Edit_WhenTaskNull_ShouldRedirectToErrorPage()
    {
        const int nonExistentTaskId = 999;

        A.CallTo(() => _taskService.GetById(nonExistentTaskId)).Returns(null as TaskModel);

        var result = await _taskController.Edit(nonExistentTaskId);

        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ActionName.Should().Be("Error");
    }

    [Fact]
    public async void Complete_WhenIdValid_ShouldRedirectToActionCompleted()
    {
        const int taskId = 1;
        const string expectedActionName = "Completed";
        A.CallTo(() => _taskService.CompleteTask(taskId)).DoesNothing();

        var result = await _taskController.Complete(taskId);

        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ActionName.Should().Be(expectedActionName);
    }

    [Fact]
    public async void Complete_WhenIdLessThanOne_ShouldRedirectToActionError()
    {
        const int invalidTaskId = 0;
        const string expectedActionName = "Error";

        var result = await _taskController.Complete(invalidTaskId);

        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ActionName.Should().Be(expectedActionName);
    }

    [Fact]
    public async void Create_WhenModelValid_ShouldRedirectToActionIndex()
    {
        var validModel = A.Fake<TaskViewModel>();
        validModel.Title = "Task title";
        validModel.Description = "Task description";

        const string expectedActionName = "Index";
        A.CallTo(() => _taskService.Add(validModel)).DoesNothing();

        var result = await _taskController.Create(validModel);

        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ActionName.Should().Be(expectedActionName);
    }

    [Fact]
    public async void Create_WhenModelInvalid_ShouldReturnViewResult()
    {
        var invalidModel = A.Fake<TaskViewModel>();
        invalidModel.Title = "a";
        invalidModel.Description = null;

        var result = await _taskController.Create(invalidModel);

        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public async void Create_WhenModelInvalid_ShouldReturnModel()
    {
        var invalidModel = A.Fake<TaskViewModel>();
        invalidModel.Title = "a";
        invalidModel.Description = null;

        var result = await _taskController.Create(invalidModel);

        result.As<ViewResult>().ViewData.Model.Should().Be(invalidModel);
    }

    [Fact]
    public async void Update_WhenIdValid_ShouldRedirectToActionIndex()
    {
        const int taskId = 1;
        var model = A.Fake<TaskViewModel>();
        model.Title = "Task title";
        model.Description = "Task description";

        const string expectedActionName = "Index";
        A.CallTo(() => _taskService.Update(taskId, model)).DoesNothing();

        var result = await _taskController.Update(taskId, model);

        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ActionName.Should().Be(expectedActionName);
    }

    [Fact]
    public async void Update_WhenIdInvalid_ShouldRedirectToActionError()
    {
        const int invalidTaskId = 0;
        var model = A.Fake<TaskViewModel>();
        const string expectedActionName = "Error";
        A.CallTo(() => _taskService.Update(invalidTaskId, model)).DoesNothing();

        var result = await _taskController.Update(invalidTaskId, model);

        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ActionName.Should().Be(expectedActionName);
    }

    [Fact]
    public async void Update_WhenModelInvalid_ShouldRedirectToActionError()
    {
        const int taskId = 1;
        var invalidModel = A.Fake<TaskViewModel>();
        const string expectedActionName = "Error";
        A.CallTo(() => _taskService.Update(taskId, invalidModel)).DoesNothing();

        var result = await _taskController.Update(taskId, invalidModel);

        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ActionName.Should().Be(expectedActionName);
    }

    [Fact]
    public async void Delete_WhenIdValid_ShouldRedirectToActionIndex()
    {
        const int taskId = 1;
        A.CallTo(() => _taskService.Remove(taskId)).DoesNothing();
        const string expectedActionName = "Index";

        var result = await _taskController.Delete(taskId);

        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ActionName.Should().Be(expectedActionName);
    }

    [Fact]
    public async void Delete_WhenIdInvalid_ShouldRedirectToActionError()
    {
        const int invalidTaskId = 0;
        const string expectedActionName = "Error";

        _taskController.TempData = _fakeTempData;
        var result = await _taskController.Delete(invalidTaskId);

        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ActionName.Should().Be(expectedActionName);
    }
}