using AutoMapper;
using RecipeApp.Core.Features.RecipeFeature.Command.Model;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Core.Mapping.RecipeMapping
{
    public class RecipeMappingProfile : Profile
    {
        public RecipeMappingProfile()
        {
            CreateMap<CreateRecipeCommand, Recipe>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.ImgURL, opt => opt.Ignore());

            CreateMap<CreateIngredientDto, Ingredient>()
                .ForMember(dest => dest.RecipeId, opt => opt.Ignore());

            CreateMap<CreateInstructionDto, Instruction>()
                .ForMember(dest => dest.RecipeId, opt => opt.Ignore());
        }
    }
}