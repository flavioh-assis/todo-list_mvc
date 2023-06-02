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
}