using FluentValidation;
using RecipeApp.Core.Features.ApplicationUser.Command.Model;
using RecipeApp.Service.Abstraction;

namespace RecipeApp.Core.Features.ApplicationUser.Command.Validator
{
    public class AddApplicationUserValidator : AbstractValidator<AddApplicationUserCommand>
    {

        private readonly IApplicationUserService _applicationUserService;

        public AddApplicationUserValidator(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
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
                var result = await _applicationUserService.IsEmailAlreadyRegisteredAsync(key);
                return !result.Data;
            }).WithMessage("Email is already used");
        }
    }
}