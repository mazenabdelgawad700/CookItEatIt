using Microsoft.AspNetCore.Identity;
using RecipeApp.Domain.Entities.Identity;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Implementaion
{
    internal class ConfirmEmailSerivce : IConfirmEmailSerivce
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ConfirmEmailSerivce(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ReturnBase<bool>> ConfirmEmailAsync(int userId, string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return ReturnBaseHandler.Failed<bool>("Invalid Token");
                }
                ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());

                if (user is null)
                    return ReturnBaseHandler.Failed<bool>("Invalid User Id");

                IdentityResult confirmEmailResult = await _userManager.ConfirmEmailAsync(user, token);

                if (confirmEmailResult.Succeeded)
                    return ReturnBaseHandler.Success(true, "Email confirmed successfully");

                return ReturnBaseHandler.Failed<bool>("Failed to confirm email address, please try again");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
    }
}