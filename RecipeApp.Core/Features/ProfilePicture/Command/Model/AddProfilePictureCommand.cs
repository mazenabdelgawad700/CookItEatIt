using MediatR;
using Microsoft.AspNetCore.Http;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.ApplicationUser.Command.Model
{
    public class AddProfilePictureCommand : IRequest<ReturnBase<bool>>
    {
        public int Id { get; set; }
        public IFormFile ProfilePicture { get; set; } = null!;
    }
}