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
        Task<ReturnBase<bool>> IsUserNameAlreadyExistAsync(string userName);
        Task<ReturnBase<string>> RefreshTokenAsync(string accessToken);
        Task<ReturnBase<bool>> SendResetPasswordEmailAsync(string email);
        Task<ReturnBase<bool>> ResetPasswordAsync(string resetPasswordToken, string newPassword, string email);
    }
}