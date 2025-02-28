using MediatR;
using Microsoft.AspNetCore.Http;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.PreferredDishFeature.Command.Model
{
    public class UpdatePreferredDishCommand : IRequest<ReturnBase<bool>>
    {
        public int Id { get; set; }
        public string? DishName { get; set; }
        public IFormFile? DishImage { get; set; }
    }
}
