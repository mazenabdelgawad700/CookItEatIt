using Microsoft.EntityFrameworkCore;
using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Infrastructure.Context;
using RecipeApp.Infrastructure.InfrastructureBases;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Infrastructure.Repositories
{
    internal class SavedRecipeRepository : GenericRepositoryAsync<SavedRecipe>, ISavedRecipeRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<SavedRecipe> _dbSet;
        public SavedRecipeRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<SavedRecipe>();
        }

        public async Task<ReturnBase<bool>> RemoveSavedRecipeAsync(SavedRecipe savedRecipe)
        {
            try
            {
                _dbSet.Remove(savedRecipe);
                await _dbContext.SaveChangesAsync();
                return ReturnBaseHandler.Success(true);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
    }
}
