using MediatR;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.ApplicationUser.Command.Model
{
    public class DeleteProfilePictureCommand : IRequest<ReturnBase<bool>>
    {
        public int Id { get; set; }
        public string PictureName { get; set; } = null!;
    }
}