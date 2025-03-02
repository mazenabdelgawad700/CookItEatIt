using FluentValidation;
using RecipeApp.Core.Features.UserPreferredDishFeature.Command.Model;

namespace RecipeApp.Core.Features.UserPreferredDishFeature.Command.Validator
{
  public class SaveUserPreferredDishesCommandValidator : AbstractValidator<SaveUserPreferredDishesCommand>
  {
    public SaveUserPreferredDishesCommandValidator()
    {
      RuleFor(x => x.UserId)
          .NotEmpty().WithMessage("User ID is required")
          .GreaterThan(0).WithMessage("Invalid User ID");

      RuleFor(x => x.PreferredDishIds)
          .NotNull().WithMessage("Preferred dish IDs list is required");

      RuleForEach(x => x.PreferredDishIds)
          .GreaterThan(0).WithMessage("Invalid preferred dish ID");
    }
  }
}