using Microsoft.AspNetCore.Identity;
using RecipeApp.Domain.Entities.Identity;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Implementaion
{
    internal class AuthenticationService : IAuthenticationService
    {

        private readonly UserManager<ApplicationUser> _userManager;


        public AuthenticationService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ReturnBase<string>> LoginInAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return ReturnBaseHandler.Failed<string>("Please, enter your credentials");

            ApplicationUser? user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return ReturnBaseHandler.Failed<string>("Invalid email or password");

            bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, password);

            if (!isPasswordCorrect)
                return ReturnBaseHandler.Failed<string>("Invalid email or password");

            var token = "fasdfsfsd";

            return ReturnBaseHandler.Success(token, "");
        }
    }
}