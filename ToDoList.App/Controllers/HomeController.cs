using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ToDoList.App.Models;
using ToDoList.App.Repository.Interfaces;

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
			var tasks = await _taskRepository.GetAll();

			return View(tasks);
		}

		public IActionResult Completed()
		{
			return View();
		}

		public IActionResult Create()
		{
			return View();
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