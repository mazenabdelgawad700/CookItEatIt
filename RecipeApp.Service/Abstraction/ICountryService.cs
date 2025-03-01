using RecipeApp.Domain.Entities.Models;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Abstraction
{
  public interface ICountryService
  {
    Task<ReturnBase<IQueryable<Country>>> GetAllCountriesAsync(string? searchTerm);
  }
}
