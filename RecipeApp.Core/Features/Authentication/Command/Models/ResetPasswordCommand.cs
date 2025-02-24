using MediatR;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.Authentication.Command.Models
{
    public class ResetPasswordCommand : IRequest<ReturnBase<bool>>
    {
        public string ResetPasswordToken { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}