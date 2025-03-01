using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Implementation
{
  public class CountryService : ICountryService
  {
    private readonly ICountryRepository _countryRepository;

    public CountryService(ICountryRepository countryRepository)
    {
      _countryRepository = countryRepository;
    }

    public async Task<ReturnBase<IQueryable<Country>>> GetAllCountriesAsync(string? searchTerm)
    {
      try
      {
        return await _countryRepository.GetAllCountriesAsync(searchTerm);
      }
      catch (Exception ex)
      {
        return ReturnBaseHandler.Failed<IQueryable<Country>>(ex.Message);
      }
    }
  }
}
