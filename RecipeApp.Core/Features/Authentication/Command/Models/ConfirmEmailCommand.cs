using MediatR;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.Authentication.Command.Models
{
    public class ConfirmEmailCommand : IRequest<ReturnBase<bool>>
    {
        public int UserId { get; set; }
        public string Token { get; set; } = null!;
    }
}