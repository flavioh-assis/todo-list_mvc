using Microsoft.AspNetCore.Mvc;
using ToDoList.App.Models;
using ToDoList.App.Services.Interfaces;
using ToDoList.App.Validators;
using ToDoList.App.ViewModels;

namespace ToDoList.App.Controllers
{
    public class TaskController : Controller
    {
        private readonly ILogger<TaskController> _logger;
        private readonly ITaskService _taskService;
        private readonly TaskViewModelValidator _validator;

        public TaskController(ILogger<TaskController> logger, ITaskService taskService)
        {
            _logger = logger;
            _taskService = taskService;
            _validator = new TaskViewModelValidator();
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
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "O id informado é inválido.";
                return RedirectToAction(nameof(Error));
            }

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
            var results = await _validator.ValidateAsync(model);

            if (!results.IsValid)
            {
                TempData["ErrorMessage"] = results.Errors[0].ToString();
                return RedirectToAction(nameof(Error));
            }

            await _taskService.Add(model);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "O id informado é inválido.";
                return RedirectToAction(nameof(Error));
            }

            var task = await _taskService.GetById(id);

            if (task is not null)
                return View(task);

            TempData["ErrorMessage"] = "Tarefa não existe.";
            return RedirectToAction(nameof(Error));
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromRoute] int id, TaskViewModel model)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "O id informado é inválido.";
                return RedirectToAction(nameof(Error));
            }

            var results = await _validator.ValidateAsync(model);

            if (!results.IsValid)
            {
                TempData["ErrorMessage"] = results.Errors;
                return RedirectToAction(nameof(Error));
            }

            await _taskService.Update(id, model);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "O id informado é inválido.";
                return RedirectToAction(nameof(Error));
            }

            await _taskService.Remove(id);

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var errorMessage = TempData["ErrorMessage"] as string;

            return View(new ErrorViewModel { Message = errorMessage });
        }
    }
}