using AutoMapper;
using RecipeApp.Core.Features.RecipeFeature.Queries.Response;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Core.Mapping.RecipeMapping
{
    public class GetAllRecipesMappingProfile : Profile
    {
        public GetAllRecipesMappingProfile()
        {
            CreateMap<Recipe, GetAllRecipesResponse>()
                .ForMember(x => x.AuthorName, dest => dest.MapFrom(src => src.User.UserName))
                .ForMember(x => x.RecipeId, dest => dest.MapFrom(src => src.Id));
        }
    }
}
