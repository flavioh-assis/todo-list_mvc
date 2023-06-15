using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ToDoList.App.Data.Context;
using ToDoList.App.Models;
using ToDoList.App.Repository;
using ToDoList.App.Services;
using ToDoList.Tests.Builders;
using ToDoList.Tests.Factories;
using Xunit;

namespace ToDoList.Tests.IntegrationTests.ServiceTests;

public class TaskServiceTests : IDisposable
{
    private readonly TaskContext _dbContext;
    private readonly TaskService _taskService;
    private readonly TaskViewModelBuilder _taskViewModelBuilder;

    private readonly TaskModel _task1Pending;
    private readonly TaskModel _task2Completed;
    private readonly int _totalTask;

    public TaskServiceTests()
    {
        var taskBuilder = new TaskModelBuilder();
        _taskViewModelBuilder = new TaskViewModelBuilder();

        _dbContext = new TaskContextFactory()
            .CreateDbContext(Array.Empty<string>());
        _dbContext.Database.EnsureCreated();

        var taskRepository = new TaskRepository(_dbContext);
        _taskService = new TaskService(taskRepository);

        _task1Pending = taskBuilder.Pending().Build();
        _task2Completed = taskBuilder.Completed().Build();

        _dbContext.Add(_task1Pending);
        _dbContext.Add(_task2Completed);
        _dbContext.SaveChanges();

        _totalTask = 2;
    }

    [Fact]
    public async void GetAll_ShouldReturnListTaskModel()
    {
        var expectedResultType = typeof(List<TaskModel>);

        var result = await _taskService.GetAll();

        result.Should().BeOfType(expectedResultType);
    }

    [Fact]
    public async void GetAll_ShouldReturnAllTasks()
    {
        var result = await _taskService.GetAll();

        result.Should().Contain(_task1Pending);
        result.Should().Contain(_task2Completed);
    }

    [Fact]
    public async void GetAllPending_ShouldReturnListTaskModel()
    {
        var expectedResultType = typeof(List<TaskModel>);

        var result = await _taskService.GetAllPending();

        result.Should().BeOfType(expectedResultType);
    }

    [Fact]
    public async void GetAllPending_ShouldReturnAllPendingTasks()
    {
        var result = await _taskService.GetAllPending();

        result.Should().NotContain(_task2Completed);
        result.Should().Contain(_task1Pending);
    }

    [Fact]
    public async void GetAllCompleted_ShouldReturnListTaskModel()
    {
        var expectedResultType = typeof(List<TaskModel>);

        var result = await _taskService.GetAllCompleted();

        result.Should().BeOfType(expectedResultType);
    }

    [Fact]
    public async void GetAllCompleted_ShouldReturnCompletedTasksOnly()
    {
        var result = await _taskService.GetAllCompleted();

        result.Should().NotContain(_task1Pending);
        result.Should().Contain(_task2Completed);
    }

    [Fact]
    public async void GetById_ShouldReturnTaskModel()
    {
        var taskId = _task1Pending.Id;
        var expectedResultType = typeof(TaskModel);

        var result = await _taskService.GetById(taskId);

        result.Should().BeOfType(expectedResultType);
    }

    [Fact]
    public async void GetById_ShouldReturnTaskModelWithId()
    {
        var taskId = _task1Pending.Id;

        var result = await _taskService.GetById(taskId);

        result.Should().BeEquivalentTo(_task1Pending);
    }

    [Fact]
    public async void CompleteTask_ShouldUpdateCompletedAtAttribute()
    {
        var taskId = _task1Pending.Id;
        _task1Pending.CompletedAt.Should().BeNull();

        await _taskService.CompleteTask(taskId);

        _task1Pending.CompletedAt.Should().NotBeNull();
    }

    [Fact]
    public async void CompleteTask_ShouldSetCompletedAtAttribute_ToCurrentUtcDateTime()
    {
        var taskId = _task1Pending.Id;
        var expectedDate = DateTime.UtcNow;
        var timeSpan = new TimeSpan(0, 0, 5);

        await _taskService.CompleteTask(taskId);

        _task1Pending.CompletedAt.Should().BeCloseTo(expectedDate, timeSpan);
    }

    [Fact]
    public async void Add_ShouldInsertNewTask()
    {
        var newTaskId = _totalTask + 1;
        var expectedCreatedAt = DateTime.UtcNow;
        var fiveSecondsTimeSpan = new TimeSpan(0, 0, 5);
        var taskViewModel = _taskViewModelBuilder
            .WithTitle("New Title")
            .WithDescription("New description")
            .Build();

        await _taskService.Add(taskViewModel);

        var insertedTask = await _dbContext.Tasks.FindAsync(newTaskId);
        insertedTask.Title.Should().Be(taskViewModel.Title);
        insertedTask.Description.Should().Be(taskViewModel.Description);
        insertedTask.CreatedAt.Should().BeCloseTo(expectedCreatedAt, fiveSecondsTimeSpan);
        insertedTask.CompletedAt.Should().BeNull();
    }

    [Fact]
    public async void Update_ShouldUpdateTitle()
    {
        var taskId = _task1Pending.Id;
        var newTitle = "New Title";
        var currentDescription = _task1Pending.Description;
        var taskViewModel = _taskViewModelBuilder
            .WithTitle(newTitle)
            .WithDescription(currentDescription)
            .Build();

        await _taskService.Update(taskId, taskViewModel);

        var updatedTask = await _dbContext.Tasks.FindAsync(taskId);
        updatedTask.Title.Should().Be(newTitle);
        updatedTask.Description.Should().Be(currentDescription);
    }

    [Fact]
    public async void Update_ShouldUpdateDescription()
    {
        var taskId = _task1Pending.Id;
        var newDescription = "New description";
        var currentTitle = _task1Pending.Title;
        var taskViewModel = _taskViewModelBuilder
            .WithTitle(currentTitle)
            .WithDescription(newDescription)
            .Build();

        await _taskService.Update(taskId, taskViewModel);

        var updatedTask = await _dbContext.Tasks.FindAsync(taskId);
        updatedTask.Title.Should().Be(currentTitle);
        updatedTask.Description.Should().Be(newDescription);
    }

    [Fact]
    public async void Remove_ShouldRemoveTask()
    {
        var taskId = _task1Pending.Id;

        await _taskService.Remove(taskId);

        var allTasks = await _dbContext.Tasks.ToListAsync();
        allTasks.Should().NotContain(_task1Pending);
        allTasks.Should().Contain(_task2Completed);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}