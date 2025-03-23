using MediatR;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.RecipeFeature.Command.Model
{
    public class SaveRecipeCommand : IRequest<ReturnBase<bool>>
    {
        public int UserId { get; set; }
        public int RecipeId { get; set; }
    }
}