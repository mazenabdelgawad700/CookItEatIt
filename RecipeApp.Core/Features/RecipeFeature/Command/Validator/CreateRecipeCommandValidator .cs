using FluentValidation;
using RecipeApp.Core.Features.RecipeFeature.Command.Model;

namespace RecipeApp.Core.Features.RecipeFeature.Command.Validator
{
    public class CreateRecipeCommandValidator : AbstractValidator<CreateRecipeCommand>
    {

        public CreateRecipeCommandValidator()
        {
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.RecipeName)
                .NotEmpty().WithMessage("Recipe name is required")
                .MaximumLength(100).WithMessage("Recipe name cannot exceed 100 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

            RuleFor(x => x.Ingredients)
                .NotEmpty().WithMessage("At least one ingredient is required")
                .Must(ingredients => ingredients.All(i => !string.IsNullOrWhiteSpace(i.Name)))
                .WithMessage("Ingredient names cannot be empty");

            RuleFor(x => x.Instructions)
                .NotEmpty().WithMessage("At least one instruction is required")
                .Must(instructions => instructions.All(i => !string.IsNullOrWhiteSpace(i.Description)))
                .WithMessage("Instruction descriptions cannot be empty");

            RuleFor(x => x.ServesCount)
                .GreaterThan(0).WithMessage("Serves count must be greater than 0");

            RuleFor(x => x.CookTimeMinutes)
                .GreaterThanOrEqualTo(0).WithMessage("Cook time cannot be negative");

            //RuleFor(x => x.PreferredDishId)
            //    .MustAsync(async (id, cancellation) =>
            //        await _preferredDishRepository.ExistsAsync(id))
            //    .WithMessage("Invalid Preferred Dish");
        }
    }
}