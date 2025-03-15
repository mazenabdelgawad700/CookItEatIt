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
        private readonly AppDbContext _dbContext;
        public RecipeRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<Recipe>();
        }

        public async Task<ReturnBase<Recipe>> AddRecipeAsync(Recipe recipe)
        {
            try
            {
                var addRecipeResult = await _dbSet.AddAsync(recipe);

                if (addRecipeResult is null)
                    return ReturnBaseHandler.Failed<Recipe>("Failed to add recipe");

                await _dbContext.SaveChangesAsync();
                return ReturnBaseHandler.Success(addRecipeResult.Entity, "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<Recipe>(ex.Message);
            }
        }

        public async Task<ReturnBase<Recipe>> GetRecipeByIdToDeleteAsync(int recipeId)
        {
            try
            {
                Recipe? recipe = await _dbSet
                                            .Where(x => x.Id == recipeId)
                                            .Include(x => x.Ingredients)
                                            .Include(x => x.Instructions)
                                            .Include(x => x.Likes)
                                            .Include(x => x.RecipeCategories)
                                            .ThenInclude(rc => rc.Category)
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

        public async Task<ReturnBase<Recipe>> GetRecipeByIdAsync(int recipeId)
        {
            try
            {
                Recipe? recipe = await _dbSet
                                            .Where(x => x.Id == recipeId)
                                            .Include(x => x.Ingredients)
                                            .Include(x => x.Instructions)
                                            .Include(x => x.RecipeCategories)
                                            .ThenInclude(rc => rc.Category)
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

        public async Task<ReturnBase<Recipe>> GetRecipeByIdAsNoTrackingAsync(int recipeId)
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

        public ReturnBase<IQueryable<Recipe>> GetRecipesForUserAsync(int userId)
        {
            try
            {
                var recipes = _dbSet
                                .Where(x => x.UserId == userId)
                                .AsNoTracking();

                if (recipes is not null)
                    return ReturnBaseHandler.Success(recipes, "");

                return ReturnBaseHandler.Failed<IQueryable<Recipe>>("Can not get recipes, check user id");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<IQueryable<Recipe>>(ex.Message);
            }
        }
    }
}
