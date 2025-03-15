using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.InfrastructureBases;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Infrastructure.Abstracts
{
    public interface IRecipeRepository : IGenericRepositoryAsync<Recipe>
    {
        Task<ReturnBase<Recipe>> GetRecipeByIdToDeleteAsync(int recipeId);
        Task<ReturnBase<Recipe>> GetRecipeByIdAsync(int recipeId);
        ReturnBase<IQueryable<Recipe>> GetRecipesForUserAsync(int userId);
        Task<ReturnBase<Recipe>> GetRecipeByIdAsNoTrackingAsync(int recipeId);
        Task<ReturnBase<Recipe>> AddRecipeAsync(Recipe recipe);
    }
}
