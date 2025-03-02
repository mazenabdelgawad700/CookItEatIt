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
    public async Task<ReturnBase<bool>> UpdateCategoryAsync(Category category)
    {
      try
      {
        ReturnBase<Category> exsistingCategory = await _categoryRepository.GetCategoryById(category.Id);

        if (!exsistingCategory.Succeeded || exsistingCategory.Data is null)
          return ReturnBaseHandler.Failed<bool>(exsistingCategory.Message);

        ReturnBase<bool> updateCategoryResult = await _categoryRepository.UpdateAsync(category);

        if (updateCategoryResult.Succeeded)
          return ReturnBaseHandler.Updated<bool>("Category updated successfully");

        return ReturnBaseHandler.Failed<bool>(updateCategoryResult.Message);
      }
      catch (Exception ex)
      {
        return ReturnBaseHandler.Failed<bool>(ex.Message);
      }
    }
    public async Task<ReturnBase<bool>> DeleteCategoryAsync(int categoryId)
    {
      try
      {
        // Check if category exists
        var categoryResult = await _categoryRepository.GetCategoryById(categoryId);
        if (!categoryResult.Succeeded || categoryResult.Data is null)
          return ReturnBaseHandler.NotFound<bool>("Category not found");

        // Check if category has recipes
        if (categoryResult.Data.Recipes is not null && categoryResult.Data.Recipes.Count > 0)
          return ReturnBaseHandler.BadRequest<bool>("Cannot delete category as it is being used by recipes");

        // Delete category
        var deleteResult = await _categoryRepository.DeleteAsync(categoryId);
        if (deleteResult.Succeeded)
          return ReturnBaseHandler.Success(true, "Category deleted successfully");

        return ReturnBaseHandler.Failed<bool>(deleteResult.Message);
      }
      catch (Exception ex)
      {
        return ReturnBaseHandler.Failed<bool>(ex.Message);
      }
    }
  }
}