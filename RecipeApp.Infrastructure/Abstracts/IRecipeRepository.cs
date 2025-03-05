using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.InfrastructureBases;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Infrastructure.Abstracts
{
    public interface IRecipeRepository : IGenericRepositoryAsync<Recipe>
    {
        Task<ReturnBase<Recipe>> GetRecipeById(int recipeId);
        Task<ReturnBase<Recipe>> AddRecipeAsync(Recipe recipe);
    }
}
