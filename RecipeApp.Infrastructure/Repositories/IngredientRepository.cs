using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Infrastructure.Context;
using RecipeApp.Infrastructure.InfrastructureBases;

namespace RecipeApp.Infrastructure.Repositories
{
    internal class IngredientRepository : GenericRepositoryAsync<Ingredient>, IIngredientRepository
    {
        public IngredientRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}