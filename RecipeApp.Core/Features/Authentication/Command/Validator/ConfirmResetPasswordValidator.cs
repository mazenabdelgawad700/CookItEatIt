using FluentValidation;
using RecipeApp.Core.Features.Authentication.Command.Models;
using RecipeApp.Service.Abstraction;

namespace RecipeApp.Core.Features.Authentication.Command.Validator
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
    {
        private readonly IAuthenticationService _authenticationService;

        public ResetPasswordValidator(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(e => e.ResetPasswordToken)
                .NotNull()
                .WithMessage("Invalid Token")
                .NotEmpty()
                .WithMessage("Invalid Token");

            RuleFor(e => e.NewPassword)
                .NotNull()
                .WithMessage("Password can not be null")
                .NotEmpty()
                .WithMessage("Password can not be empty")
                .Matches("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[\\[\\]{}!@#$%^&*()]).{8,}$")
                .WithMessage(
                "Password must contain at least 8 characters, one uppercase letter, one lowercase letter, one number and one special character"
                );

            RuleFor(x => x.Email)
               .NotEmpty()
               .WithMessage("Email address is required")
               .NotNull()
               .WithMessage("Email address can not be null")
               .Matches("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$")
               .WithMessage("Email address is not valid");
        }

        private void ApplyCustomValidationRules()
        {
            RuleFor(x => x.Email).MustAsync(async (key, CancellationToken) =>
            {
                var result = await _authenticationService.IsEmailAlreadyRegisteredAsync(key);
                return result.Data;
            }).WithMessage("There is no user with this email");
        }
    }
}