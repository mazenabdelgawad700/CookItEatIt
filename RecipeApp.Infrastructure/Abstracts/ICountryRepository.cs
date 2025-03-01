using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.InfrastructureBases;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Infrastructure.Abstracts
{
  public interface ICountryRepository : IGenericRepositoryAsync<Country>
  {
    Task<ReturnBase<IQueryable<Country>>> GetAllCountriesAsync(string? searchTerm);
  }
}
