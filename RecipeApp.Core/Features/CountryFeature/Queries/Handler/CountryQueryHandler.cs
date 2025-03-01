using AutoMapper;
using MediatR;
using RecipeApp.Core.Features.CountryFeature.Queries.Model;
using RecipeApp.Core.Features.CountryFeature.Queries.Response;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.CountryFeature.Queries.Handler
{
  public class CountryQueryHandler :
      IRequestHandler<GetAllCountriesQuery, ReturnBase<IQueryable<GetAllCountriesResponse>>>
  {
    private readonly ICountryService _countryService;
    private readonly IMapper _mapper;

    public CountryQueryHandler(ICountryService countryService, IMapper mapper)
    {
      _countryService = countryService;
      _mapper = mapper;
    }

    public async Task<ReturnBase<IQueryable<GetAllCountriesResponse>>> Handle(
        GetAllCountriesQuery request,
        CancellationToken cancellationToken)
    {
      try
      {
        var countries = await _countryService.GetAllCountriesAsync(request.SearchTerm);

        if (!countries.Succeeded)
          return ReturnBaseHandler.Failed<IQueryable<GetAllCountriesResponse>>(countries.Message);

        var mappedCountries = _mapper.ProjectTo<GetAllCountriesResponse>(countries.Data);

        return ReturnBaseHandler.Success(mappedCountries, "Countries retrieved successfully");
      }
      catch (Exception ex)
      {
        return ReturnBaseHandler.Failed<IQueryable<GetAllCountriesResponse>>(ex.Message);
      }
    }
  }
}
