using FluentValidation;
using RecipeApp.Core.Features.Authentication.Command.Models;
using RecipeApp.Service.Abstraction;

namespace RecipeApp.Core.Features.Authentication.Command.Validtor
{
    public class RegisterValidator : AbstractValidator<RegisterCommand>
    {

        private readonly IAuthenticationService _authenticationService;

        public RegisterValidator(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.UserName)
                .NotNull()
                .WithMessage("UserName can not be null")
                .NotEmpty()
                .WithMessage("UserName is required");

            RuleFor(x => x.Email)
                .NotNull()
                .WithMessage("Email address can not be null")
                .NotEmpty()
                .WithMessage("Email address is required")
                .Matches("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$")
                .WithMessage("Email address is not valid");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required")
                .Matches("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[\\[\\]{}!@#$%^&*()]).{8,}$")
                .WithMessage(
                "Password must contain at least 8 characters, one uppercase letter, one lowercase letter, one number and one special character"
                );
        }
        private void ApplyCustomValidationRules()
        {
            RuleFor(x => x.Email).MustAsync(async (key, CancellationToken) =>
            {
                var result = await _authenticationService.IsEmailAlreadyRegisteredAsync(key);
                return !result.Succeeded;
            }).WithMessage("Email is already used");
        }
    }
}