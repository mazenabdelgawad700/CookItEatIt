using FluentValidation;
using RecipeApp.Core.Features.Authentication.Command.Models;

namespace RecipeApp.Core.Features.Authentication.Command.Validtor
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordValidator()
        {
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(e => e.NewPassword)
                .NotNull()
                .WithMessage("Password can not be null")
                .NotEmpty()
                .WithMessage("Password can not be empty")
                .Matches("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[\\[\\]{}!@#$%^&*()]).{8,}$")
                .WithMessage(
                "Password must contain at least 8 characters, one uppercase letter, one lowercase letter, one number and one special character"
                )
                .Must((model, key, cancellationToken) => model.CurrentPassword != key)
                .WithMessage(
                "This is password has been used before. Please, enter a new strong password"
                );

            RuleFor(e => e.CurrentPassword)
                .NotNull()
                .WithMessage("Password can not be null")
                .NotEmpty()
                .WithMessage("Password can not be empty");

            RuleFor(e => e.Id)
                .NotNull()
                .WithMessage("Id can not be null")
                .NotEmpty()
                .WithMessage("Id can not be empty");
        }
    }
}