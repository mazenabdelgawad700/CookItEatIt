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

        public async Task<ReturnBase<Recipe>> GetRecipeById(int recipeId)
        {
            try
            {
                Recipe? recipe = await _dbSet
                                            .Where(x => x.Id == recipeId)
                                            .Include(x => x.Ingredients)
                                            .Include(x => x.Instructions)
                                            .FirstOrDefaultAsync();

                if (recipe is null)
                {
                    return ReturnBaseHandler.NotFound<Recipe>("Recipe not found");
                }
                return ReturnBaseHandler.Success(recipe, "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<Recipe>(ex.Message);
            }
        }

        public async Task<ReturnBase<Recipe>> GetRecipeByIdAsNoTracking(int recipeId)
        {
            try
            {
                Recipe? recipe = await _dbSet.Where(x => x.Id == recipeId).AsNoTracking().FirstOrDefaultAsync();

                if (recipe is null)
                {
                    return ReturnBaseHandler.NotFound<Recipe>("Recipe not found");
                }
                return ReturnBaseHandler.Success(recipe, "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<Recipe>(ex.Message);
            }
        }
    }
}
