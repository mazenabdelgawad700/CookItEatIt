using AutoMapper;
using RecipeApp.Core.Features.CountryFeature.Queries.Response;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Core.Mapping.CountryMapping
{
  public class GetAllCountriesMappingProfile : Profile
  {
    public GetAllCountriesMappingProfile()
    {
      CreateMap<Country, GetAllCountriesResponse>();
    }
  }
}