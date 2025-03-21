using MediatR;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.RecipeFeature.Command.Model
{
    public class ToggleRecipeLikeCommand : IRequest<ReturnBase<bool>>
    {
        public int RecipeId { get; set; }
        public int UserId { get; set; }
    }
}
