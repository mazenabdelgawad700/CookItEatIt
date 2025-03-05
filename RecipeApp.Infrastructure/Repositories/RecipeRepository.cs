using Microsoft.EntityFrameworkCore;
using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Infrastructure.Context;
using RecipeApp.Infrastructure.InfrastructureBases;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Infrastructure.Repositories
{
    public class RecipeRepository : GenericRepositoryAsync<Recipe>, IRecipeRepository
    {
        private readonly DbSet<Recipe> _dbSet;
        public RecipeRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbSet = dbContext.Set<Recipe>();
        }

        public async Task<ReturnBase<Recipe>> AddRecipeAsync(Recipe recipe)
        {
            try
            {
                var addRecipeResult = await _dbSet.AddAsync(recipe);

                if (addRecipeResult is null)
                {
                    return ReturnBaseHandler.NotFound<Recipe>("Failed to add recipe");
                }
                return ReturnBaseHandler.Success(addRecipeResult.Entity, "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<Recipe>(ex.Message);
            }
        }

        public async Task<ReturnBase<int>> GetRecipeIdByName(string name)
        {
            try
            {
                Recipe? recipe = await _dbSet.Where(x => x.RecipeName.ToLower() == name.ToLower()).FirstOrDefaultAsync();

                if (recipe is null)
                {
                    return ReturnBaseHandler.NotFound<int>("Recipe not found");
                }
                return ReturnBaseHandler.Success(recipe.Id, "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<int>(ex.Message);
            }
        }
    }
}
