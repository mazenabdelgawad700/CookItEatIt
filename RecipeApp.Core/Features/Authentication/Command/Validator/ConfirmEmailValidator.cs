using FluentValidation;
using RecipeApp.Core.Features.Authentication.Command.Models;

namespace RecipeApp.Core.Features.Authentication.Command.Validtor
{
    public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailValidator()
        {
            ApplyValidationRules();
        }

        public void ApplyValidationRules()
        {
            RuleFor(e => e.UserId)
                .NotNull()
                .WithMessage("User id can not be NULL")
                .NotEmpty()
                .WithMessage("User id can not be empty");

            RuleFor(e => e.Token)
                .NotNull()
                .WithMessage("Invalid Token")
                .NotEmpty()
                .WithMessage("Invalid Token");
        }
    }
}