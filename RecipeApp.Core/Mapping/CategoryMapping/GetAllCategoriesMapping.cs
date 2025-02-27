using AutoMapper;
using RecipeApp.Core.Features.Category.Queries.Response;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Core.Mapping.CategoryMapping
{
    public class GetAllCategoriesMapping : Profile
    {
        public GetAllCategoriesMapping()
        {
            CreateMap<Category, GetAllCategoriesResponse>();
        }
    }
}