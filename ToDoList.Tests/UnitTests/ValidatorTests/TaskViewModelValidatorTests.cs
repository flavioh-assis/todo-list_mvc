using FluentAssertions;
using FluentValidation.TestHelper;
using ToDoList.App.Validators;
using ToDoList.Tests.Builders;
using Xunit;

namespace ToDoList.Tests.UnitTests.ValidatorTests;

public class TaskViewModelValidatorTests
{
    private readonly TaskViewModelValidator _validator = new();

    private const string titleNullErrorMessage = "Campo 'Título' não pode ser vazio.";
    private const string titleTooSmallErrorMessage = "Campo 'Título' deve ter no mínimo 3 caracteres.";
    private const string titleTooBigErrorMessage = "Campo 'Título' deve ter no máximo 20 caracteres.";
    private const string descriptionTooLargeErrorMessage = "Campo 'Descrição' deve ter no máximo 100 caracteres.";

    [Fact]
    public void WhenModelIsValid_ShouldNotReturnErrors()
    {
        var model = new TaskViewModelBuilder()
            .NewValidTask()
            .Build();

        var result = _validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void WhenTitleIsNull_ShouldReturnErrorWithMessage()
    {
        var model = new TaskViewModelBuilder()
            .NewValidTask()
            .WithTitleNull()
            .Build();

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(m => m.Title)
            .WithErrorMessage(titleNullErrorMessage);
    }

    [Fact]
    public void WhenTitleLengthIsLessThanThree_ShouldReturnErrorWithMessage()
    {
        var model = new TaskViewModelBuilder()
            .NewValidTask()
            .WithTitleTooSmall()
            .Build();

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(m => m.Title)
            .WithErrorMessage(titleTooSmallErrorMessage);
    }

    [Fact]
    public void WhenTitleLengthMoreThanTwenty_ShouldReturnErrorWithMessage()
    {
        var model = new TaskViewModelBuilder()
            .NewValidTask()
            .WithTitleTooLarge()
            .Build();

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(m => m.Title)
            .WithErrorMessage(titleTooBigErrorMessage);
    }

    [Fact]
    public void WhenDescriptionLengthMoreThanOneHundred_ShouldReturnErrorWithMessage()
    {
        var model = new TaskViewModelBuilder()
            .NewValidTask()
            .WithDescriptionTooLarge()
            .Build();

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(m => m.Description)
            .WithErrorMessage(descriptionTooLargeErrorMessage);
    }

    [Fact]
    public void WhenTitleAndDescriptionAreInvalid_ShouldReturnTwoErrors()
    {
        var expectedErrorsCount = 2;
        var model = new TaskViewModelBuilder()
            .NewValidTask()
            .WithTitleNull()
            .WithDescriptionTooLarge()
            .Build();

        var result = _validator.TestValidate(model);

        result.Errors.Count.Should().Be(expectedErrorsCount);
    }
}