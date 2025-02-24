using RecipeApp.Domain.Entities.Identity;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Abstraction
{
    public interface IConfirmEmailService
    {
        Task<ReturnBase<bool>> ConfirmEmailAsync(int userId, string token);
        Task<ReturnBase<bool>> SendConfirmationEmailAsync(ApplicationUser user);
    }
}