using ToDoList.App.Models;
using ToDoList.App.Repository.Interfaces;
using ToDoList.App.Services.Interfaces;
using ToDoList.App.ViewModels;

namespace ToDoList.App.Services
{
    public class TaskService : ITaskService
    {
        private readonly IRepositoryBase<TaskModel> _taskRepository;

        public TaskService(IRepositoryBase<TaskModel> taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskModel>> GetAll()
        {
            return await _taskRepository.GetAll();
        }

        public async Task<List<TaskModel>> GetAllPending()
        {
            var allTasks = await this.GetAll();
            var pendingTasks = allTasks.Select(x => x)
                .Where(x => x.CompletedAt == null)
                .ToList();

            return pendingTasks;
        }

        public async Task<List<TaskModel>> GetAllCompleted()
        {
            var allTasks = await this.GetAll();
            var completedTasks = allTasks.Select(x => x)
                .Where(x => x.CompletedAt != null)
                .ToList();

            return completedTasks;
        }

        public async Task<TaskModel> GetById(int id)
        {
            return await _taskRepository.GetById(id);
        }

        public async Task CompleteTask(int id)
        {
            var task = await this.GetById(id);
            task.CompletedAt = DateTime.UtcNow;

            await _taskRepository.Update(task);
        }

        public async Task Add(TaskViewModel model)
        {
            var newTask = new TaskModel
            {
                Title = model.Title,
                Description = model.Description
            };

            await _taskRepository.Create(newTask);
        }

        public async Task Update(int id, TaskViewModel model)
        {
            var task = await this.GetById(id);

            task.Title = model.Title;
            task.Description = model.Description;

            await _taskRepository.Update(task);
        }

        public async Task Remove(int id)
        {
            var task = await this.GetById(id);

            await _taskRepository.Delete(task);
        }
    }
}