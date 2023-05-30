using System.ComponentModel.DataAnnotations;

namespace ToDoList.App.ViewModels
{
	public class CompleteTaskViewModel
	{
		[Required]
		[Display(Name = "Id")]
		public int Id { get; set; }
	}
}