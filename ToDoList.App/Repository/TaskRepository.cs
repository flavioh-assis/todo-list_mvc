using ToDoList.App.Data.Context;
using ToDoList.App.Models;
using ToDoList.App.Repository.Base;
using ToDoList.App.Repository.Interfaces;

namespace ToDoList.App.Repository
{
	public class TaskRepository : RepositoryBase<TaskModel>, ITaskRepository
	{
		public TaskRepository(TaskContext context) : base(context) { }
	}
}