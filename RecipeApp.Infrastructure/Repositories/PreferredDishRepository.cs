using Microsoft.EntityFrameworkCore;
using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Infrastructure.Context;
using RecipeApp.Infrastructure.InfrastructureBases;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Infrastructure.Repositories
{
    internal class PreferredDishRepository : GenericRepositoryAsync<PreferredDish>, IPreferredDishRepository
    {
        private readonly DbSet<PreferredDish> _dbSet;
        public PreferredDishRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbSet = dbContext.Set<PreferredDish>();
        }

        public async Task<ReturnBase<bool>> IsPreferredDishExistAsync(string dishName)
        {
            try
            {
                PreferredDish? exsitingDish = await _dbSet.FirstOrDefaultAsync(x => x.DishName == dishName);
                if (exsitingDish is not null)
                    return ReturnBaseHandler.Success(true, "Dish already exist");

                return ReturnBaseHandler.Success(false, "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
    }
}