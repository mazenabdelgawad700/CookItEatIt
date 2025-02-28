using MediatR;
using Microsoft.AspNetCore.Http;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.PreferredDishFeature.Command.Model
{
    public class AddPreferredDishCommand : IRequest<ReturnBase<bool>>
    {
        public string DishName { get; set; } = null!;
        public IFormFile DishImage { get; set; } = null!;
    }
}