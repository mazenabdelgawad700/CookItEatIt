using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.InfrastructureBases;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Infrastructure.Abstracts
{
    public interface ISavedRecipeRepository : IGenericRepositoryAsync<SavedRecipe>
    {
        public Task<ReturnBase<bool>> RemoveSavedRecipeAsync(SavedRecipe savedRecipe);
    }
}