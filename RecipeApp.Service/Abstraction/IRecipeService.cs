using Microsoft.AspNetCore.Http;
using RecipeApp.Domain.Entities.Models;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Abstraction
{
    public interface IRecipeService
    {
        Task<ReturnBase<int>> CreateRecipeAsync(Recipe recipe);
        Task<ReturnBase<Recipe>> GetRecipeByIdAsync(int recipeId);
        Task<ReturnBase<bool>> AddRecipeImageAsync(int recipeId, IFormFile imageFile, string[] allowedExtensions);
    }
}
