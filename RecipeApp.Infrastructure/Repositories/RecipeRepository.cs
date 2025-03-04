using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Infrastructure.Context;
using RecipeApp.Infrastructure.InfrastructureBases;

namespace RecipeApp.Infrastructure.Repositories
{
  public class RecipeRepository : GenericRepositoryAsync<Recipe>, IRecipeRepository
  {
    public RecipeRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
  }
}
