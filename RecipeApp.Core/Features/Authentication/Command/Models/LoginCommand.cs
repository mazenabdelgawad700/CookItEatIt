using MediatR;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.Authentication.Command.Models
{
    public class LoginCommand : IRequest<ReturnBase<string>>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}