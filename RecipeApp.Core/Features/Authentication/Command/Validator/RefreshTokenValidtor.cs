using FluentValidation;
using RecipeApp.Core.Features.Authentication.Command.Models;

namespace RecipeApp.Core.Features.Authentication.Command.Validator
{
    public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenValidator()
        {
            ApplyValidationRules();
        }
        private void ApplyValidationRules()
        {
            RuleFor(e => e.AccessToken)
                .NotNull()
                .WithMessage("Access token can not be null")
                .NotEmpty()
                .WithMessage("Access token can not be empty");
        }
    }
}
