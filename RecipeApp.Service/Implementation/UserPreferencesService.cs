using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Implementation
{
    internal class UserPreferencesService : IUserPreferencesService
    {
        private readonly IUserPreferencesRepository _userPreferencesRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;

        public UserPreferencesService(IUserPreferencesRepository userPreferencesRepository,
                                     IApplicationUserRepository applicationUserRepository)
        {
            _userPreferencesRepository = userPreferencesRepository;
            _applicationUserRepository = applicationUserRepository;
        }

        public async Task<ReturnBase<UserPreferences>> GetUserPreferencesByUserIdAsync(int userId)
        {
            try
            {
                return await _userPreferencesRepository.GetUserPreferencesByUserIdAsync(userId);
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
                // Check if user exists
                var user = await _applicationUserRepository.GetByIdAsync(userPreferences.UserId);
                if (!user.Succeeded || user.Data == null)
                    return ReturnBaseHandler.NotFound<bool>("User not found");

                return await _userPreferencesRepository.SaveUserPreferencesAsync(userPreferences);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
    }
}
