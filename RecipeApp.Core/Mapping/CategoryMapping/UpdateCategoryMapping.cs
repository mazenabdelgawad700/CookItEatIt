using AutoMapper;
using RecipeApp.Core.Features.Category.Command.Model;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Core.Mapping.CategoryMapping
{
    public class UpdateCategoryMapping : Profile
    {
        public UpdateCategoryMapping()
        {
            CreateMap<Category, UpdateCategoryCommand>().ReverseMap();
        }
    }
}
