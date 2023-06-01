using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ToDoList.App.Models;
using ToDoList.App.Services.Interfaces;
using ToDoList.App.ViewModels;

namespace ToDoList.App.Controllers
{
	public class TaskController : Controller
	{
		private readonly ILogger<TaskController> _logger;
		private readonly ITaskService _taskService;

		public TaskController(ILogger<TaskController> logger, ITaskService taskService)
		{
			_logger = logger;
			_taskService = taskService;
		}

		public async Task<IActionResult> Index()
		{
			var pendingTasks = await _taskService.GetAllPending();

			return View(pendingTasks);
		}

		public async Task<IActionResult> Completed()
		{
			var completedTasks = await _taskService.GetAllCompleted();

			return View(completedTasks);
		}

		[HttpPost]
		public async Task<IActionResult> Complete([FromRoute] int id)
		{
			await _taskService.CompleteTask(id);

			return RedirectToAction(nameof(Completed));
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(TaskViewModel model)
		{
			await _taskService.Add(model);

			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Edit([FromRoute] int id)
		{
			var task = await _taskService.GetById(id);

			return View(task);
		}

		[HttpPost]
		public async Task<IActionResult> Update([FromRoute] int id, TaskViewModel model)
		{
			await _taskService.Update(id, model);

			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public async Task<IActionResult> Delete([FromRoute] int id)
		{
			await _taskService.Remove(id);

			return RedirectToAction(nameof(Index));
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}