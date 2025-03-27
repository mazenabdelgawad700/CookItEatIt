using AutoMapper;
using RecipeApp.Core.Features.RecipeFeature.Queries.Response;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Core.Mapping.RecipeMapping
{
    public class GetSavedRecipesAsPaginatedMappingProfile : Profile
    {
        public GetSavedRecipesAsPaginatedMappingProfile()
        {
            CreateMap<SavedRecipe, GetSavedRecipesAsPaginatedResponse>()
                .ForMember(x => x.RecipeId, dest => dest.MapFrom(src => src.Recipe.Id))
                .ForMember(x => x.ImgURL, dest => dest.MapFrom(src => src.Recipe.ImgURL))
                .ForMember(x => x.RecipeName, dest => dest.MapFrom(src => src.Recipe.RecipeName));
        }
    }
}
