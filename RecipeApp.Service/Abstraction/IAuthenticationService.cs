using RecipeApp.Domain.Entities.Identity;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Abstraction
{
    public interface IAuthenticationService
    {
        Task<ReturnBase<string>> LoginInAsync(string email, string password);
        Task<ReturnBase<bool>> IsEmailAlreadyRegisteredAsync(string email);
        Task<ReturnBase<string>> RegisterAsync(ApplicationUser appUser, string password);
    }
}