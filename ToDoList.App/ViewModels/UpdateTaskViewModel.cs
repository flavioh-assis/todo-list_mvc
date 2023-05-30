using System.ComponentModel.DataAnnotations;

namespace ToDoList.App.ViewModels
{
	public class UpdateTaskViewModel
	{
		[Required]
		[Display(Name = "Id")]
		public int Id { get; set; }

		[Required]
		[Display(Name = "Título")]
		public string Title { get; set; }

		[Required]
		[Display(Name = "Descrição")]
		public string Description { get; set; }
	}
}