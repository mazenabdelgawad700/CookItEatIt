using AutoMapper;
using RecipeApp.Core.Features.RecipeFeature.Command.Model;
using RecipeApp.Core.Features.RecipeFeature.Queries.Model;
using RecipeApp.Core.Features.RecipeFeature.Queries.Response;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Core.Mapping.RecipeMapping
{
    public class GetRecipeByIdMappingProfile : Profile
    {
        public GetRecipeByIdMappingProfile()
        {
            CreateMap<GetRecipeByIdQuery, Recipe>().ReverseMap();

            CreateMap<CreateIngredientDto, Ingredient>();
            CreateMap<CreateInstructionDto, Instruction>();
            CreateMap<CategoryDto, Category>();
            //CreateMap<LikeDto, RecipeLike>();

            CreateMap<Recipe, GetRecipeByIdResponse>()
                .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients))
                .ForMember(dest => dest.Instructions, opt => opt.MapFrom(src => src.Instructions));

            CreateMap<Ingredient, CreateIngredientDto>();
            CreateMap<Instruction, CreateInstructionDto>();
            CreateMap<Category, CategoryDto>();
            //CreateMap<RecipeLike, LikeDto>();
        }
    }
}
