using Microsoft.AspNetCore.Http;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Abstraction
{
    public interface IProfilePictureService
    {

        Task<ReturnBase<bool>> AddProfilePictureAsync(int userId, IFormFile imageFile);
        Task<ReturnBase<bool>> UpdateProfilePictureAsync(int userId, IFormFile imageFile);
        Task<ReturnBase<bool>> DeleteProfilePictureAsync(int userId, string pictureName);
    }
}