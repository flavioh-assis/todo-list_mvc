using System.ComponentModel.DataAnnotations;

namespace ToDoList.App.ViewModels
{
	public class TaskViewModel
    {
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Display(Name = "Descrição")]
        public string Description { get; set; }
    }
}