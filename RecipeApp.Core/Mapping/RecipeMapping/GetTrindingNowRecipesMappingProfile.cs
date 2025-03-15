using AutoMapper;
using RecipeApp.Core.Features.RecipeFeature.Queries.Response;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Core.Mapping.RecipeMapping
{
    public class GetTrindingNowRecipesMappingProfile : Profile
    {
        public GetTrindingNowRecipesMappingProfile()
        {
            CreateMap<Recipe, GetTrindingNowRecipesResponse>()
                .ForMember(x => x.RecipeId, dest => dest.MapFrom(src => src.Id));
        }
    }
}
