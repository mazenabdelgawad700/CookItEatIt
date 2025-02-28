using FluentValidation;
using RecipeApp.Core.Features.PreferredDishFeature.Command.Model;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.PreferredDishFeature.Command.Validator
{
    public class UpdatePreferredDishValidator : AbstractValidator<UpdatePreferredDishCommand>
    {

        private readonly IPreferredDishService _preferredDishService;

        public UpdatePreferredDishValidator(IPreferredDishService preferredDishService)
        {
            _preferredDishService = preferredDishService;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(e => e.Id)
                .NotNull()
                .WithMessage("Id can not be null.")
                .NotEmpty()
                .WithMessage("Id is required.");

            //RuleFor(e => e.DishName)
            //    .NotNull()
            //    .WithMessage("Dish name can not be null.")
            //    .NotEmpty()
            //    .WithMessage("Dish name is required.");
        }
        private void ApplyCustomValidationRules()
        {
            RuleFor(e => e.DishName)
                .MustAsync(async (key, CancellationToken) =>
                {
                    ReturnBase<bool> result = await _preferredDishService.IsPreferredDishExistAsync(key);
                    return !result.Data;
                })
                .WithMessage("Dish Already Exist");
        }
    }
}