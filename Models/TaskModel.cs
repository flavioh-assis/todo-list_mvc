﻿namespace ToDoList.Models
{
	public class TaskModel : Entity
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? CompletedAt { get; set; }
	}
}
