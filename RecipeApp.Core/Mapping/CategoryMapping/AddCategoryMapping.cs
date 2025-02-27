using AutoMapper;
using RecipeApp.Core.Features.Category.Command.Model;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Core.Mapping.CategoryMapping
{
    public class AddCategoryMapping : Profile
    {
        public AddCategoryMapping()
        {
            CreateMap<Category, AddCategoryCommand>().ReverseMap();
        }
    }
}