using Microsoft.EntityFrameworkCore;
using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Infrastructure.Context;
using RecipeApp.Infrastructure.InfrastructureBases;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Infrastructure.Repositories
{
    internal class RecipeLikeRepository : GenericRepositoryAsync<RecipeLike>, IRecipeLikeRepository
    {
        private readonly DbSet<RecipeLike> _dbSet;
        private readonly AppDbContext _dbContext;

        public RecipeLikeRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<RecipeLike>();
        }

        public async Task<ReturnBase<bool>> RemoveLikeFromRecipe(RecipeLike recipeLike)
        {
            try
            {
                _dbSet.Remove(recipeLike);
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
