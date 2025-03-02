using FluentValidation;
using RecipeApp.Core.Features.UserPreferredCategoryFeature.Command.Model;

namespace RecipeApp.Core.Features.UserPreferredCategoryFeature.Command.Validator
{
  public class SaveUserPreferredCategoriesCommandValidator : AbstractValidator<SaveUserPreferredCategoriesCommand>
  {
    public SaveUserPreferredCategoriesCommandValidator()
    {
      RuleFor(x => x.UserId)
          .NotEmpty().WithMessage("User ID is required")
          .GreaterThan(0).WithMessage("Invalid User ID");

      RuleFor(x => x.CategoryIds)
          .NotNull().WithMessage("Category IDs list is required");

      RuleForEach(x => x.CategoryIds)
          .GreaterThan(0).WithMessage("Invalid category ID");
    }
  }
}