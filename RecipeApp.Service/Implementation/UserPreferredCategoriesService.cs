using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Implementation
{
    public class UserPreferredCategoriesService : IUserPreferredCategoriesService
    {
        private readonly IUserPreferredCategoriesRepository _userPreferredCategoriesRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;

        public UserPreferredCategoriesService(
            IUserPreferredCategoriesRepository userPreferredCategoriesRepository,
            IApplicationUserRepository applicationUserRepository)
        {
            _userPreferredCategoriesRepository = userPreferredCategoriesRepository;
            _applicationUserRepository = applicationUserRepository;
        }

        public async Task<ReturnBase<bool>> SaveUserPreferredCategoriesAsync(int userId, List<int> categoryIds)
        {
            try
            {
                // Check if user exists
                var user = await _applicationUserRepository.GetByIdAsync(userId);
                if (!user.Succeeded || user.Data == null)
                    return ReturnBaseHandler.NotFound<bool>("User not found");

                // Validate categories
                var categoriesValidResult = await _userPreferredCategoriesRepository.AreCategoriesValidAsync(categoryIds);
                if (!categoriesValidResult.Succeeded)
                    return categoriesValidResult;

                // Save preferred categories
                return await _userPreferredCategoriesRepository.SaveUserPreferredCategoriesAsync(userId, categoryIds);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
    }
}
