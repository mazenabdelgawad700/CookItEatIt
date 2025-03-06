using AutoMapper;
using RecipeApp.Core.Features.RecipeFeature.Command.Model;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Core.Mapping.RecipeMapping
{
    public class UpdateRecipeMappingProfile : Profile
    {
        public UpdateRecipeMappingProfile()
        {
            CreateMap<CreateIngredientDto, Ingredient>();
            CreateMap<CreateInstructionDto, Instruction>();

            CreateMap<UpdateRecipeCommand, Recipe>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RecipeId));
        }
    }
}
