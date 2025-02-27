using Microsoft.EntityFrameworkCore;
using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Infrastructure.Context;
using RecipeApp.Infrastructure.InfrastructureBases;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Infrastructure.Repositories
{
    internal class CategoryRepository : GenericRepositoryAsync<Category>, ICategoryRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<Category> _dbSet;

        public CategoryRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<Category>();
        }

        public ReturnBase<IQueryable<Category>> GetAllCategories()
        {
            try
            {
                ReturnBase<IQueryable<Category>> getCategoriesResult = GetTableNoTracking();

                if (getCategoriesResult is null || getCategoriesResult.Data is null)
                    return ReturnBaseHandler.Failed<IQueryable<Category>>();

                return ReturnBaseHandler.Success(getCategoriesResult.Data, "");

            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<IQueryable<Category>>(ex.Message);
            }
        }

        public async Task<ReturnBase<Category>> GetCategoryById(int categoryId)
        {
            try
            {
                Category? category = await _dbSet.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == categoryId);

                if (category is null)
                    return ReturnBaseHandler.BadRequest<Category>("Category is not exist");

                return ReturnBaseHandler.Success(category, "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<Category>(ex.Message);
            }
        }

        public async Task<ReturnBase<bool>> IsCategoryExistAsync(string categoryName)
        {
            try
            {
                Category? category = await _dbSet.Where
                    (x => x.Name.ToLower() == categoryName.ToLower())
                    .FirstOrDefaultAsync();

                if (category is not null)
                    return ReturnBaseHandler.Success(true, "Category already exist");

                return ReturnBaseHandler.BadRequest<bool>();
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
    }
}
