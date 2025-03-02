using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.InfrastructureBases;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Infrastructure.Abstracts
{
  public interface IUserPreferencesRepository : IGenericRepositoryAsync<UserPreferences>
  {
    Task<ReturnBase<UserPreferences>> GetUserPreferencesByUserIdAsync(int userId);
    Task<ReturnBase<bool>> SaveUserPreferencesAsync(UserPreferences userPreferences);
  }
}
