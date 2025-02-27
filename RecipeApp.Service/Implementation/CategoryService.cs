using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Implementation
{
    internal class CategoryService : ICategoryService
    {

        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ReturnBase<bool>> AddCategoryAsync(Category category)
        {
            try
            {
                ReturnBase<bool> addCategoryResult = await _categoryRepository.AddAsync(category);

                if (addCategoryResult.Succeeded)
                    return ReturnBaseHandler.Created(true, "Category added successfully");

                return ReturnBaseHandler.Failed<bool>(addCategoryResult.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
        public ReturnBase<IQueryable<Category>> GetAllCategories()
        {
            try
            {
                ReturnBase<IQueryable<Category>> getCategoriesResult = _categoryRepository.GetAllCategories();

                if (getCategoriesResult is null || getCategoriesResult.Data is null)
                    return ReturnBaseHandler.Failed<IQueryable<Category>>();

                return ReturnBaseHandler.Success(getCategoriesResult.Data, "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<IQueryable<Category>>(ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> IsCategoryExistAsync(string categoryName)
        {
            try
            {
                ReturnBase<bool> getCategoryResult = await _categoryRepository.IsCategoryExistAsync(categoryName);

                if (getCategoryResult.Succeeded)
                    return ReturnBaseHandler.BadRequest<bool>(getCategoryResult.Message);

                return ReturnBaseHandler.Success(false, getCategoryResult.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
    }
}