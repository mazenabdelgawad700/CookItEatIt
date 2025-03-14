using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Infrastructure.Context;
using RecipeApp.Infrastructure.InfrastructureBases;

namespace RecipeApp.Infrastructure.Repositories
{
    internal class RecipeCategoryRepository : GenericRepositoryAsync<RecipeCategory>, IRecipeCategoryRepository
    {
        public RecipeCategoryRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
