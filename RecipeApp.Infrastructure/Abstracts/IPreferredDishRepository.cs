using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.InfrastructureBases;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Infrastructure.Abstracts
{
    public interface IPreferredDishRepository : IGenericRepositoryAsync<PreferredDish>
    {
        Task<ReturnBase<bool>> IsPreferredDishExistAsync(string dishName);
        //ReturnBase<IQueryable<PreferredDish>> GetAllPreferredDishs();
        //Task<ReturnBase<PreferredDish>> GetPreferredDishById(int preferredDishId);
    }
}
