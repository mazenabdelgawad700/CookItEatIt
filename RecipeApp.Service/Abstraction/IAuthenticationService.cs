using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Abstraction
{
    public interface IAuthenticationService
    {
        Task<ReturnBase<string>> LoginInAsync(string email, string password);
    }
}