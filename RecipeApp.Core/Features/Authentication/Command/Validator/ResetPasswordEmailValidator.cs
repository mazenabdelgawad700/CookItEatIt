using FluentValidation;
using RecipeApp.Core.Features.Authentication.Command.Models;
using RecipeApp.Service.Abstraction;

namespace RecipeApp.Core.Features.Authentication.Command.Validator
{
    public class ResetPasswordEmailValidator : AbstractValidator<ResetPasswordEmailCommand>
    {
        private readonly IAuthenticationService _authenticationService;

        public ResetPasswordEmailValidator(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
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