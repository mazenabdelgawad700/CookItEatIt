using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Infrastructure.Context;
using RecipeApp.Infrastructure.InfrastructureBases;

namespace RecipeApp.Infrastructure.Repositories
{
    internal class RecipeLikeRepository : GenericRepositoryAsync<RecipeLike>, IRecipeLikeRepository
    {
        public RecipeLikeRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
