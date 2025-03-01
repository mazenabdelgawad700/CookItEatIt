using Microsoft.EntityFrameworkCore;
using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Infrastructure.Context;
using RecipeApp.Infrastructure.InfrastructureBases;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Infrastructure.Repositories
{
  public class CountryRepository : GenericRepositoryAsync<Country>, ICountryRepository
  {
    private readonly DbSet<Country> _countries;

    public CountryRepository(AppDbContext dbContext) : base(dbContext)
    {
      _countries = dbContext.Set<Country>();
    }

    public async Task<ReturnBase<IQueryable<Country>>> GetAllCountriesAsync(string? searchTerm)
    {
      try
      {
        var query = _countries.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
          query = query.Where(c => c.Name.Contains(searchTerm));

        return await Task.FromResult(ReturnBaseHandler.Success(query, "Countries retrieved successfully"));
      }
      catch (Exception ex)
      {
        return await Task.FromResult(ReturnBaseHandler.Failed<IQueryable<Country>>(ex.Message));
      }
    }
  }
}
