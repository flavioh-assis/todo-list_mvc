using ToDoList.App.Models;
using ToDoList.App.ViewModels;

namespace ToDoList.App.Services.Interfaces
{
	public interface ITaskService
	{
		Task<IEnumerable<TaskModel>> GetAll();
		Task<List<TaskModel>> GetAllPending();
		Task<List<TaskModel>> GetAllCompleted();
		Task<TaskModel> GetById(int id);
		Task CompleteTask(int id);
		Task Add(TaskViewModel model);
		Task Update(int id, TaskViewModel model);
		Task Remove(int id);
	}
}