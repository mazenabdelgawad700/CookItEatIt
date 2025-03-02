using FluentValidation;
using RecipeApp.Core.Features.UserPreferencesFeature.Command.Model;

namespace RecipeApp.Core.Features.UserPreferencesFeature.Command.Validator
{
  public class SaveUserPreferencesCommandValidator : AbstractValidator<SaveUserPreferencesCommand>
  {
    public SaveUserPreferencesCommandValidator()
    {
      RuleFor(x => x.UserId)
          .NotEmpty().WithMessage("User ID is required")
          .GreaterThan(0).WithMessage("Invalid User ID");

      RuleFor(x => x.DefaultHungryHeads)
          .GreaterThan(0).WithMessage("DefaultHungryHeads must be greater than 0");
    }
  }
}