using FluentValidation;
using RecipeApp.Core.Features.ApplicationUser.Command.Model;

namespace RecipeApp.Core.Features.ApplicationUser.Command.Validator
{
    public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailValidator()
        {
            ApplyValidionRules();
        }

        public void ApplyValidionRules()
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