using RecipeApp.Domain.Entities.Models;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Abstraction
{
  public interface IUserPreferencesService
  {
    Task<ReturnBase<bool>> SaveUserPreferencesAsync(UserPreferences userPreferences);
    Task<ReturnBase<UserPreferences>> GetUserPreferencesByUserIdAsync(int userId);
  }
}
