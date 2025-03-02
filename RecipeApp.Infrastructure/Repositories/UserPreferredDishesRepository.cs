using Microsoft.EntityFrameworkCore;
using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Infrastructure.Context;
using RecipeApp.Infrastructure.InfrastructureBases;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Infrastructure.Repositories
{
    public class UserPreferredDishesRepository : GenericRepositoryAsync<UserPreferredDishes>, IUserPreferredDishesRepository
    {
        private readonly AppDbContext _dbContext;

        public UserPreferredDishesRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ReturnBase<bool>> ArePreferredDishesValidAsync(List<int> preferredDishIds)
        {
            try
            {
                var existingDishesCount = await _dbContext.PreferredDish
                    .Where(d => preferredDishIds.Contains(d.Id))
                    .CountAsync();

                if (existingDishesCount != preferredDishIds.Count)
                    return ReturnBaseHandler.BadRequest<bool>("One or more preferred dishes are invalid");

                return ReturnBaseHandler.Success(true, "All preferred dishes are valid");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }

        public async Task<ReturnBase<bool>> SaveUserPreferredDishesAsync(int userId, List<int> preferredDishIds)
        {
            try
            {
                // Remove existing preferences
                var existingPreferences = await _dbContext.UserPreferredDishes
                    .Where(upd => upd.UserId == userId)
                    .ToListAsync();

                if (existingPreferences.Any())
                {
                    _dbContext.UserPreferredDishes.RemoveRange(existingPreferences);
                }

                // Add new preferences
                foreach (var dishId in preferredDishIds)
                {
                    await _dbContext.UserPreferredDishes.AddAsync(new UserPreferredDishes
                    {
                        UserId = userId,
                        PreferredDishId = dishId
                    });
                }

                await _dbContext.SaveChangesAsync();
                return ReturnBaseHandler.Success(true, "User preferred dishes saved successfully");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
    }
}