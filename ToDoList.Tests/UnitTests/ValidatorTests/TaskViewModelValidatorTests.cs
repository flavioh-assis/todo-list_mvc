using FakeItEasy;
using FluentValidation.TestHelper;
using ToDoList.App.Validators;
using ToDoList.App.ViewModels;
using Xunit;

namespace ToDoList.Tests.UnitTests.ValidatorTests;

public class TaskViewModelValidatorTests
{
    private readonly TaskViewModelValidator _validator;

    private const string titleNullErrorMessage = "Campo 'Título' não pode ser nulo.";
    private const string titleLessLengthThreeErrorMessage = "Campo 'Título' deve ter no mínimo 3 caracteres.";
    private const string descriptionNullErrorMessage = "Campo 'Descrição' não pode ser nulo.";

    public TaskViewModelValidatorTests()
    {
        _validator = new TaskViewModelValidator();
    }

    [Fact]
    public void WhenModelValid_ShouldNotReturnAnyErrors()
    {
        var model = A.Fake<TaskViewModel>();
        model.Title = "Title";
        model.Description = "Description";

        var results = _validator.TestValidate(model);

        results.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void WhenTitleNull_ShouldReturnErrorWithMessage()
    {
        var model = A.Fake<TaskViewModel>();
        model.Title = null;
        model.Description = "Description";

        var results = _validator.TestValidate(model);

        results.ShouldHaveValidationErrorFor(m => m.Title)
            .WithErrorMessage(titleNullErrorMessage);
    }

    [Fact]
    public void WhenTitleLessThanLengthThree_ShouldReturnErrorWithMessage()
    {
        var model = A.Fake<TaskViewModel>();
        model.Title = "Ti";
        model.Description = "Description";

        var results = _validator.TestValidate(model);

        results.ShouldHaveValidationErrorFor(m => m.Title)
            .WithErrorMessage(titleLessLengthThreeErrorMessage);
    }

    [Fact]
    public void WhenDescriptionNull_ShouldReturnErrorWithMessage()
    {
        var model = A.Fake<TaskViewModel>();
        model.Title = "Title";
        model.Description = null;

        var results = _validator.TestValidate(model);

        results.ShouldHaveValidationErrorFor(m => m.Description)
            .WithErrorMessage(descriptionNullErrorMessage);
    }

    [Fact]
    public void WhenTitleAndDescriptionNull_ShouldReturnErrorsWithMessages()
    {
        var model = A.Fake<TaskViewModel>();
        model.Title = null;
        model.Description = null;

        var results = _validator.TestValidate(model);

        results.ShouldHaveValidationErrorFor(m => m.Title)
            .WithErrorMessage(titleNullErrorMessage);
        results.ShouldHaveValidationErrorFor(m => m.Description)
            .WithErrorMessage(descriptionNullErrorMessage);
    }
}