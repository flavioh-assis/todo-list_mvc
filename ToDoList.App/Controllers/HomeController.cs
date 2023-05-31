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

		public async Task<IActionResult> Index()
		{
			var allTasks = await _taskRepository.GetAll();
			var pendingTasks = allTasks.Select(x => x).Where(x => x.CompletedAt == null).ToList();

			return View(pendingTasks);
		}

		public async Task<IActionResult> Completed()
		{
			var allTasks = await _taskRepository.GetAll();
			var completedTasks = allTasks.Select(x => x).Where(x => x.CompletedAt != null).ToList();

			return View(completedTasks);
		}

		[HttpPost]
		public async Task<IActionResult> Complete([FromRoute] int id)
		{
			var task = await _taskRepository.GetById(id);
			task.CompletedAt = DateTime.UtcNow;

			await _taskRepository.Update(task);

			return RedirectToAction(nameof(Completed));
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(TaskViewModel model)
		{
			var newTask = new TaskModel { Title = model.Title, Description = model.Description };

			await _taskRepository.Create(newTask);

			return RedirectToAction("Index", "Home");
		}

		public async Task<IActionResult> Edit([FromRoute] int id)
		{
			var task = await _taskRepository.GetById(id);

			return View(task);
		}

		[HttpPost]
		public async Task<IActionResult> Update([FromRoute] int id, TaskViewModel model)
		{
			var task = await _taskRepository.GetById(id);
			task.Title = model.Title;
			task.Description = model.Description;

			await _taskRepository.Update(task);

			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public async Task<IActionResult> Delete([FromRoute] int id)
		{
			var task = await _taskRepository.GetById(id);

			await _taskRepository.Delete(task);

			return RedirectToAction(nameof(Index));
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}