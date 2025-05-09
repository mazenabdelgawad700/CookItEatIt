﻿using FluentValidation;
using RecipeApp.Core.Features.Category.Command.Model;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.Category.Command.Validator
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        private readonly ICategoryService _categoryService;

        public UpdateCategoryValidator(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .WithMessage("Id can not be Null")
                .NotEmpty()
                .WithMessage("Id is required.");

            RuleFor(x => x.Name)
                .NotNull()
                .WithMessage("Name can not be Null")
                .NotEmpty()
                .WithMessage("Name is required.");
        }
        private void ApplyCustomValidationRules()
        {
            RuleFor(e => e.Name)
                .MustAsync(async (key, CancellationToken) =>
                {
                    ReturnBase<bool> result = await _categoryService.IsCategoryExistAsync(key);
                    return result.Succeeded;
                })
                .WithMessage("Category already exist");
        }

    }
}
