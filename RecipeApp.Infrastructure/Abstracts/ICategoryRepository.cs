using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.InfrastructureBases;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Infrastructure.Abstracts
{
    public interface ICategoryRepository : IGenericRepositoryAsync<Category>
    {
        Task<ReturnBase<bool>> IsCategoryExistAsync(string categoryName);
        ReturnBase<IQueryable<Category>> GetAllCategories();
    }
}
