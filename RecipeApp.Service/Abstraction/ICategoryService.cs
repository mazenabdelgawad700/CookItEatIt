using RecipeApp.Domain.Entities.Models;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Abstraction
{
    public interface ICategoryService
    {
        Task<ReturnBase<bool>> AddCategoryAsync(Category category);
        Task<ReturnBase<bool>> UpdateCategoryAsync(Category category);
        Task<ReturnBase<bool>> IsCategoryExistAsync(string categoryName);
        ReturnBase<IQueryable<Category>> GetAllCategories();
    }
}