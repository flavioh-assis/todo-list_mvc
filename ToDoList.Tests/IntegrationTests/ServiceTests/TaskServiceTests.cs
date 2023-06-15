using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
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
    private readonly TaskModelBuilder _taskBuilder;

    private readonly TaskModel _task1Pending;
    private readonly TaskModel _task2Completed;

    public TaskServiceTests()
    {
        _taskBuilder = new TaskModelBuilder();

        _dbContext = new TaskContextFactory()
            .CreateDbContext(Array.Empty<string>());
        _dbContext.Database.EnsureCreated();

        var taskRepository = new TaskRepository(_dbContext);
        _taskService = new TaskService(taskRepository);

        _task1Pending = _taskBuilder.Pending().Build();
        _task2Completed = _taskBuilder.Completed().Build();

        _dbContext.Add(_task1Pending);
        _dbContext.Add(_task2Completed);
        _dbContext.SaveChanges();
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

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}