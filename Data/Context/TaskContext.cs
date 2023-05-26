using Microsoft.EntityFrameworkCore;
using ToDoList.Data.Configuration;
using ToDoList.Models;

namespace ToDoList.Data.Context
{
	public class TaskContext : DbContext
	{
		public DbSet<TaskModel> Tasks { get; set; }

		public TaskContext(DbContextOptions<TaskContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new TaskConfiguration());
			base.OnModelCreating(modelBuilder);
		}
	}
}
