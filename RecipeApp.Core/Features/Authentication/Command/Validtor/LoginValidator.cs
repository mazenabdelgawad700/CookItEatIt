using FluentValidation;
using RecipeApp.Core.Features.Authentication.Command.Models;

namespace RecipeApp.Core.Features.Authentication.Command.Validtor
{
    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator()
        {
            ApplyValidationRules();
        }
        private void ApplyValidationRules()
        {
            RuleFor(e => e.Email)
                .NotNull()
                .WithMessage("Email Address Can not be null")
                .NotEmpty()
                .WithMessage("Email Can not be empty");

            RuleFor(e => e.Password)
                .NotNull()
                .WithMessage("Password Can not be null")
                .NotEmpty()
                .WithMessage("Password Can not be empty");
        }
    }
}