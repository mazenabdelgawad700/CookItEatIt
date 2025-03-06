using AutoMapper;
using RecipeApp.Core.Features.RecipeFeature.Queries.Response;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Core.Mapping.RecipeMapping
{
    public class GetRecipesForUserMappingProfile : Profile
    {
        public GetRecipesForUserMappingProfile()
        {
            CreateMap<Recipe, GetRecipesForUserResponse>();
        }
    }
}
