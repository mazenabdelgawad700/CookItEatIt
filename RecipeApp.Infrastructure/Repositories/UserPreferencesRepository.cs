using Microsoft.EntityFrameworkCore;
using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Infrastructure.Context;
using RecipeApp.Infrastructure.InfrastructureBases;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Infrastructure.Repositories
{
    internal class UserPreferencesRepository : GenericRepositoryAsync<UserPreferences>, IUserPreferencesRepository
    {
        private readonly AppDbContext _dbContext;

        public UserPreferencesRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ReturnBase<UserPreferences>> GetUserPreferencesByUserIdAsync(int userId)
        {
            try
            {
                var preferences = await _dbContext.UserPreferences
                    .FirstOrDefaultAsync(p => p.UserId == userId);

                if (preferences == null)
                    return ReturnBaseHandler.NotFound<UserPreferences>("User preferences not found");

                return ReturnBaseHandler.Success(preferences, "User preferences retrieved successfully");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<UserPreferences>(ex.Message);
            }
        }

        public async Task<ReturnBase<bool>> SaveUserPreferencesAsync(UserPreferences userPreferences)
        {
            try
            {
                var existingPreferences = await _dbContext.UserPreferences
                    .FirstOrDefaultAsync(p => p.UserId == userPreferences.UserId);

                if (existingPreferences == null)
                {
                    // Create new preferences
                    await _dbContext.UserPreferences.AddAsync(userPreferences);
                }
                else
                {
                    // Update existing preferences
                    existingPreferences.IsVegetarian = userPreferences.IsVegetarian;
                    existingPreferences.AcceptNewDishNotification = userPreferences.AcceptNewDishNotification;
                    existingPreferences.DefaultHungryHeads = userPreferences.DefaultHungryHeads;
                    _dbContext.UserPreferences.Update(existingPreferences);
                }

                await _dbContext.SaveChangesAsync();
                return ReturnBaseHandler.Success(true, "User preferences saved successfully");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
    }
}
