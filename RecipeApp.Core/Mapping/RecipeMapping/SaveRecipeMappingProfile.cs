using AutoMapper;
using RecipeApp.Core.Features.RecipeFeature.Command.Model;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Core.Mapping.RecipeMapping
{
    public class SaveRecipeMappingProfile : Profile
    {
        public SaveRecipeMappingProfile()
        {
            CreateMap<Recipe, SaveRecipeCommand>()
                .ForMember(x => x.RecipeId, dest => dest.MapFrom(src => src.Id));
        }
    }
}