using MediatR;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.Authentication.Command.Models
{
    public class ChangePasswordCommand : IRequest<ReturnBase<bool>>
    {
        public int Id { get; set; }
        public string NewPassword { get; set; } = null!;
        public string CurrentPassword { get; set; } = null!;
    }
}