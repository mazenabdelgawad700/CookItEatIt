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

    public async Task<ReturnBase<bool>> IsCountryExistsAsync(int countryId)
    {
      try
      {
        var country = await _countryRepository.GetByIdAsync(countryId);
        if (!country.Succeeded || country.Data is null)
          return ReturnBaseHandler.NotFound<bool>("Country not found");

        return ReturnBaseHandler.Success(true, "Country exists");
      }
      catch (Exception ex)
      {
        return ReturnBaseHandler.Failed<bool>(ex.Message);
      }
    }
  }
}
