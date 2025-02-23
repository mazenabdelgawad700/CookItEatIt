using RecipeApp.Domain.Entities.Identity;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Abstraction
{
    public interface IAuthenticationService
    {
        Task<ReturnBase<string>> RegisterAsync(ApplicationUser appUser, string password);
        Task<ReturnBase<string>> LoginInAsync(string email, string password, string ipAddress);
        Task<ReturnBase<bool>> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<ReturnBase<bool>> IsEmailAlreadyRegisteredAsync(string email);
    }
}