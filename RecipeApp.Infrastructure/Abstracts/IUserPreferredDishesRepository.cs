using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.InfrastructureBases;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Infrastructure.Abstracts
{
  public interface IUserPreferredDishesRepository : IGenericRepositoryAsync<UserPreferredDishes>
  {
    Task<ReturnBase<bool>> SaveUserPreferredDishesAsync(int userId, List<int> preferredDishIds);
    Task<ReturnBase<bool>> ArePreferredDishesValidAsync(List<int> preferredDishIds);
  }
}