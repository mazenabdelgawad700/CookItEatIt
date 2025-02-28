using AutoMapper;
using RecipeApp.Core.Features.PreferredDishFeature.Command.Model;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Core.Mapping.PreferredDishMapping
{
    public class AddPreferredDishMapping : Profile
    {
        public AddPreferredDishMapping()
        {
            CreateMap<PreferredDish, AddPreferredDishCommand>().ReverseMap();
        }
    }
}
