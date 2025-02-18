using FluentValidation;
using RecipeApp.Core.Features.ApplicationUser.Command.Model;

namespace RecipeApp.Core.Features.ApplicationUser.Command.Validator
{
    public class AddApplicationUserValidator : AbstractValidator<AddApplicationUserCommand>
    {
        public AddApplicationUserValidator()
        {
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email address is required")
                .NotNull()
                .WithMessage("Email address can not be null")
                .Matches("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$")
                .WithMessage("Email address is not valid");

            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("User Name is required")
                .NotNull()
                .WithMessage("User Name can not be null");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8);
        }
        private void ApplyCustomValidationRules()
        {
            // Check if email is already registered
        }
    }
}