using Microsoft.AspNetCore.Identity;
using RecipeApp.Domain.Entities.Identity;
using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Implementation
{
    internal class ApplicationUserService : IApplicationUserService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationUserRepository _applicationUserRepository;

        public ApplicationUserService(UserManager<ApplicationUser> userManager, IApplicationUserRepository applicationUserRepository)
        {
            _userManager = userManager;
            _applicationUserRepository = applicationUserRepository;
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

        public async Task<ReturnBase<bool>> UpdateApplicationUserAsync(ApplicationUser user)
        {
            try
            {
                ReturnBase<bool> updateResult = await _applicationUserRepository.UpdateAsync(user);

                if (updateResult.Succeeded)
                    return ReturnBaseHandler.Success(true, "");

                return ReturnBaseHandler.Failed<bool>(updateResult.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
    }
}
