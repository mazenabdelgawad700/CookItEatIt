using MediatR;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.Authentication.Command.Models
{
    public class ResetPasswordEmailCommand : IRequest<ReturnBase<bool>>
    {
        public string Email { get; set; } = null!;
    }
}