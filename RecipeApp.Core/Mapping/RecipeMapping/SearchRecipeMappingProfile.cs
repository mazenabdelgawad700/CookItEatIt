using AutoMapper;
using RecipeApp.Core.Features.RecipeFeature.Queries.Response;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Core.Mapping.RecipeMapping
{
    public class SearchRecipeMappingProfile : Profile
    {
        public SearchRecipeMappingProfile()
        {
            CreateMap<Recipe, SearchRecipeResponse>();
        }
    }
}
