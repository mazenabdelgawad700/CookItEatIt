using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Implementation
{
  public class UserPreferredDishesService : IUserPreferredDishesService
  {
    private readonly IUserPreferredDishesRepository _userPreferredDishesRepository;
    private readonly IApplicationUserRepository _applicationUserRepository;

    public UserPreferredDishesService(
        IUserPreferredDishesRepository userPreferredDishesRepository,
        IApplicationUserRepository applicationUserRepository)
    {
      _userPreferredDishesRepository = userPreferredDishesRepository;
      _applicationUserRepository = applicationUserRepository;
    }

    public async Task<ReturnBase<bool>> SaveUserPreferredDishesAsync(int userId, List<int> preferredDishIds)
    {
      try
      {
        // Check if user exists
        var user = await _applicationUserRepository.GetByIdAsync(userId);
        if (!user.Succeeded || user.Data == null)
          return ReturnBaseHandler.NotFound<bool>("User not found");

        // Validate preferred dishes
        var dishesValidResult = await _userPreferredDishesRepository.ArePreferredDishesValidAsync(preferredDishIds);
        if (!dishesValidResult.Succeeded)
          return dishesValidResult;

        // Save preferred dishes
        return await _userPreferredDishesRepository.SaveUserPreferredDishesAsync(userId, preferredDishIds);
      }
      catch (Exception ex)
      {
        return ReturnBaseHandler.Failed<bool>(ex.Message);
      }
    }
  }
}