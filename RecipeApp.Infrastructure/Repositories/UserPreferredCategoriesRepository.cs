using Microsoft.EntityFrameworkCore;
using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Infrastructure.Context;
using RecipeApp.Infrastructure.InfrastructureBases;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Infrastructure.Repositories
{
  public class UserPreferredCategoriesRepository : GenericRepositoryAsync<UserPreferredCategory>, IUserPreferredCategoriesRepository
  {
    private readonly AppDbContext _dbContext;

    public UserPreferredCategoriesRepository(AppDbContext dbContext) : base(dbContext)
    {
      _dbContext = dbContext;
    }

    public async Task<ReturnBase<bool>> AreCategoriesValidAsync(List<int> categoryIds)
    {
      try
      {
        var existingCategoriesCount = await _dbContext.Category
            .Where(c => categoryIds.Contains(c.Id))
            .CountAsync();

        if (existingCategoriesCount != categoryIds.Count)
          return ReturnBaseHandler.BadRequest<bool>("One or more categories are invalid");

        return ReturnBaseHandler.Success(true, "All categories are valid");
      }
      catch (Exception ex)
      {
        return ReturnBaseHandler.Failed<bool>(ex.Message);
      }
    }

    public async Task<ReturnBase<bool>> SaveUserPreferredCategoriesAsync(int userId, List<int> categoryIds)
    {
      try
      {
        // Remove existing preferences
        var existingPreferences = await _dbContext.UserPreferredCategory
            .Where(upc => upc.UserId == userId)
            .ToListAsync();

        if (existingPreferences.Any())
        {
          _dbContext.UserPreferredCategory.RemoveRange(existingPreferences);
        }

        // Add new preferences
        foreach (var categoryId in categoryIds)
        {
          await _dbContext.UserPreferredCategory.AddAsync(new UserPreferredCategory
          {
            UserId = userId,
            CategoryId = categoryId
          });
        }

        await _dbContext.SaveChangesAsync();
        return ReturnBaseHandler.Success(true, "User preferred categories saved successfully");
      }
      catch (Exception ex)
      {
        return ReturnBaseHandler.Failed<bool>(ex.Message);
      }
    }
  }
}
