using Microsoft.AspNetCore.Identity;
using RecipeApp.Domain.Entities.Identity;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Implementation
{
    internal class ApplicationUserService : IApplicationUserService
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ReturnBase<ApplicationUser>> GetApplicationUserProfileByIdAsync(int userId)
        {
            try
            {
                ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());

                if (user is null)
                    return ReturnBaseHandler.Failed<ApplicationUser>("User Not Found");
                return ReturnBaseHandler.Success(user, "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<ApplicationUser>(ex.Message);
            }
        }
    }
}
