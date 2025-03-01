using AutoMapper;
using RecipeApp.Core.Features.PreferredDishFeature.Queries.Response;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Core.Mapping
{
  public class PreferredDishMappingProfile : Profile
  {
    public PreferredDishMappingProfile()
    {
      CreateMap<PreferredDish, GetAllPreferredDishesResponse>();
    }
  }
}