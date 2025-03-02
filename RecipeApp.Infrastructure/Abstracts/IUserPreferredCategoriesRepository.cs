using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.InfrastructureBases;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Infrastructure.Abstracts
{
  public interface IUserPreferredCategoriesRepository : IGenericRepositoryAsync<UserPreferredCategory>
  {
    Task<ReturnBase<bool>> SaveUserPreferredCategoriesAsync(int userId, List<int> categoryIds);
    Task<ReturnBase<bool>> AreCategoriesValidAsync(List<int> categoryIds);
  }
}
