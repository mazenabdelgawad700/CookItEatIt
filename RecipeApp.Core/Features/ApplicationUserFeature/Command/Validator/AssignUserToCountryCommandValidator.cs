using FluentValidation;
using RecipeApp.Core.Features.ApplicationUserFeature.Command.Model;

namespace RecipeApp.Core.Features.ApplicationUserFeature.Command.Validator
{
  public class AssignUserToCountryCommandValidator : AbstractValidator<AssignUserToCountryCommand>
  {
    public AssignUserToCountryCommandValidator()
    {
      RuleFor(x => x.UserId)
          .NotEmpty().WithMessage("User ID is required")
          .GreaterThan(0).WithMessage("Invalid User ID");

      RuleFor(x => x.CountryId)
          .NotEmpty().WithMessage("Country ID is required")
          .GreaterThan(0).WithMessage("Invalid Country ID");
    }
  }
}