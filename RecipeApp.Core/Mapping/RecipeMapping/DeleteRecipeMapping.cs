using AutoMapper;
using RecipeApp.Core.Features.RecipeFeature.Command.Model;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Core.Mapping.RecipeMapping
{
    public class DeleteRecipeMapping : Profile
    {
        public DeleteRecipeMapping()
        {
            CreateMap<DeleteRecipeCommand, Recipe>();
        }
    }
}
