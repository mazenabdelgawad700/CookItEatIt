using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Abstraction
{
  public interface IUserPreferredDishesService
  {
    Task<ReturnBase<bool>> SaveUserPreferredDishesAsync(int userId, List<int> preferredDishIds);
  }
}