using ToDoList.Data.Context;
using ToDoList.Models;
using ToDoList.Repository.Base;
using ToDoList.Repository.Interfaces;

namespace ToDoList.Repository
{
	public class TaskRepository : RepositoryBase<TaskModel>, ITaskRepository
	{
		public TaskRepository(TaskContext context) : base(context) { }
	}
}