using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ToDoList.App.Models;
using ToDoList.App.Repository.Interfaces;
using ToDoList.App.ViewModels;

namespace ToDoList.App.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IRepositoryBase<TaskModel> _taskRepository;

		public HomeController(ILogger<HomeController> logger, IRepositoryBase<TaskModel> taskRepository)
		{
			_logger = logger;
			_taskRepository = taskRepository;
		}

		public async Task<IActionResult> IndexAsync()
		{
			var allTasks = await _taskRepository.GetAll();
			var pendingTasks = allTasks.Select(x => x).Where(x => x.CompletedAt == null).ToList();

			return View(pendingTasks);
		}

		public async Task<IActionResult> CompletedAsync()
		{
			var allTasks = await _taskRepository.GetAll();
			var completedTasks = allTasks.Select(x => x).Where(x => x.CompletedAt != null).ToList();

			return View(completedTasks);
		}

		[HttpPost]
		public async Task<IActionResult> CompleteAsync(CompleteTaskViewModel model)
		{
			var task = await _taskRepository.GetById(model.Id);
			task.CompletedAt = DateTime.UtcNow;

			await _taskRepository.Update(task);

			return RedirectToAction("Completed", "Home");
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateAsync(TaskViewModel model)
		{
			var newTask = new TaskModel { Title = model.Title, Description = model.Description };

			await _taskRepository.Create(newTask);

			return RedirectToAction("Index", "Home");
		}

		public IActionResult Edit()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}