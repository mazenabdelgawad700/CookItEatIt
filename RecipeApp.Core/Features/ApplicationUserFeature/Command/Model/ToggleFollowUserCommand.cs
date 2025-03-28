using MediatR;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.ApplicationUserFeature.Command.Model
{
    public class ToggleFollowUserCommand : IRequest<ReturnBase<bool>>
    {
        public int FollowerId { get; set; }
        public int FollowingId { get; set; }
    }
}