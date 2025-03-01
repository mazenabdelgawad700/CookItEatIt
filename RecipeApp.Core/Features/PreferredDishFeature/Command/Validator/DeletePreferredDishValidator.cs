using FluentValidation;
using RecipeApp.Core.Features.PreferredDishFeature.Command.Model;

namespace RecipeApp.Core.Features.PreferredDishFeature.Command.Validator
{
  public class DeletePreferredDishValidator : AbstractValidator<DeletePreferredDishCommand>
  {


    public DeletePreferredDishValidator()
    {
      ApplyValidationRules();
    }

    private void ApplyValidationRules()
    {
      RuleFor(e => e.Id)
          .NotNull()
          .WithMessage("Id can not be null.")
          .NotEmpty()
          .WithMessage("Id is required.");
    }
  }
}