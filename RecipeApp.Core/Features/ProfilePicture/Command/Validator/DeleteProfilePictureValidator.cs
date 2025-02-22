using FluentValidation;
using RecipeApp.Core.Features.ApplicationUser.Command.Model;

namespace RecipeApp.Core.Features.ApplicationUser.Command.Validator
{
    internal class DeleteProfilePictureValidator : AbstractValidator<DeleteProfilePictureCommand>
    {
        public DeleteProfilePictureValidator()
        {
            ApplyValidionRules();
        }

        public void ApplyValidionRules()
        {
            RuleFor(e => e.PictureName)
                .NotNull()
                .WithMessage("Profile Name can not be NULL")
                .NotEmpty()
                .WithMessage("Profile Name can not be empty");

            RuleFor(e => e.Id)
                .NotNull()
                .WithMessage("Id can not be NULL")
                .NotEmpty()
                .WithMessage("Id can not be empty");
        }
    }
}