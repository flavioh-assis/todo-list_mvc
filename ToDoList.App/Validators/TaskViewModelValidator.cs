using FluentValidation;
using ToDoList.App.ViewModels;

namespace ToDoList.App.Validators;

public class TaskViewModelValidator : AbstractValidator<TaskViewModel>
{
    public TaskViewModelValidator()
    {
        RuleFor(x => x.Title)
            .NotNull()
            .WithMessage("Campo 'Título' não pode ser nulo.")
            .MinimumLength(3)
            .WithMessage("Campo 'Título' deve ter no mínimo 3 caracteres.")
            .MaximumLength(20)
            .WithMessage("Campo 'Título' deve ter no máximo 20 caracteres.");

        RuleFor(x => x.Description)
            .MaximumLength(100)
            .WithMessage("Campo 'Descrição' deve ter no máximo 100 caracteres.");
    }
}