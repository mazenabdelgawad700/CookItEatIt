using MediatR;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.Authentication.Command.Models
{
    public class RefreshTokenCommand : IRequest<ReturnBase<string>>
    {
        public string AccessToken { get; set; } = null!;
    }
}