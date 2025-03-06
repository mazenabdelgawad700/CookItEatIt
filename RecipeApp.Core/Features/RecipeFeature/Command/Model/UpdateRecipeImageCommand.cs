using MediatR;
using Microsoft.AspNetCore.Http;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.RecipeFeature.Command.Model
{
    public class UpdateRecipeImageCommand : IRequest<ReturnBase<bool>>
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
