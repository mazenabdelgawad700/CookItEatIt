using Microsoft.AspNetCore.Http;
using RecipeApp.Domain.Entities.Models;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Abstraction
{
    public interface IRecipeService
    {
        Task<ReturnBase<int>> CreateRecipeAsync(Recipe recipe);
        Task<ReturnBase<bool>> UpdateRecipeAsync(Recipe recipe);
        Task<ReturnBase<bool>> DeleteRecipeAsync(Recipe recipe);
        Task<ReturnBase<bool>> IsCurrentUserLikedTheRecipeAsync(int userId, int recipeId);
        Task<ReturnBase<bool>> AddRecipeCategoriesAsync(int recipeId, List<int> categoryIds);
        Task<ReturnBase<bool>> UpdateRecipeCategoriesAsync(int recipeId, List<int> categoryIds);
        Task<ReturnBase<bool>> UpdateRecipeImageAsync(int recipeId, IFormFile imageFile, string[] allowedExtensions);
        Task<ReturnBase<Recipe>> GetRecipeByIdAsync(int recipeId);
        Task<ReturnBase<bool>> AddRecipeImageAsync(int recipeId, IFormFile imageFile, string[] allowedExtensions);
        /* ****************** Not Async ****************** */
        ReturnBase<IQueryable<Recipe>> GetRecipesForUserAsync(int userId);
        ReturnBase<IQueryable<Recipe>> GetAllRecipes(int? filter = null);
        ReturnBase<IQueryable<Recipe>> GetTrindingNowRecipes();
    }
}
