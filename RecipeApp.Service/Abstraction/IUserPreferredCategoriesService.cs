using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Abstraction
{
  public interface IUserPreferredCategoriesService
  {
    Task<ReturnBase<bool>> SaveUserPreferredCategoriesAsync(int userId, List<int> categoryIds);
  }
}
