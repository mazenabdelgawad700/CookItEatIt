using RecipeApp.Domain.Entities.Identity;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Abstraction
{
    public interface IApplicationUserService
    {
        Task<ReturnBase<bool>> IsEmailAlreadyRegisteredAsync(string email);
        Task<ReturnBase<string>> AddApplicationUserAsync(ApplicationUser appUser, string password);
    }
}