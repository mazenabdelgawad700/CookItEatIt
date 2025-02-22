using RecipeApp.Domain.Entities.Identity;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Abstraction
{
    public interface IConfirmEmailSerivce
    {
        Task<ReturnBase<bool>> ConfirmEmailAsync(int userId, string token);
        Task<ReturnBase<bool>> SendConfirmationEmailAsync(ApplicationUser user);
    }
}