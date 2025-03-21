using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.InfrastructureBases;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Infrastructure.Abstracts
{
    public interface IRecipeLikeRepository : IGenericRepositoryAsync<RecipeLike>
    {
        public Task<ReturnBase<bool>> RemoveLikeFromRecipe(RecipeLike recipeLike);
    }
}
