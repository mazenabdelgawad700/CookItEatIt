using MediatR;
using Microsoft.AspNetCore.Http;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.ProfilePicture.Command.Model
{
    public class UpdateProfilePictureCommand : IRequest<ReturnBase<bool>>
    {
        public int Id { get; set; }
        public IFormFile ProfilePicture { get; set; } = null!;
    }
}