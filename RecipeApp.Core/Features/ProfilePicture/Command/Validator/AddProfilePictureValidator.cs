using FluentValidation;
using RecipeApp.Core.Features.ApplicationUser.Command.Model;

namespace RecipeApp.Core.Features.ApplicationUser.Command.Validator
{
    internal class AddProfilePictureValidator : AbstractValidator<AddProfilePictureCommand>
    {
        public AddProfilePictureValidator()
        {
            ApplyValidionRules();
        }

        public void ApplyValidionRules()
        {
            RuleFor(e => e.ProfilePicture)
                .NotNull()
                .WithMessage("Profile Picture can not be NULL")
                .NotEmpty()
                .WithMessage("Profile Picture can not be empty");

            RuleFor(e => e.Id)
                .NotNull()
                .WithMessage("Id can not be NULL")
                .NotEmpty()
                .WithMessage("Id can not be empty");
        }
    }
}