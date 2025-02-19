using Microsoft.AspNetCore.Identity;
using RecipeApp.Domain.Entities.Identity;
using RecipeApp.Infrastructure.Context;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Implementaion
{
    internal class ApplicationUserService : IApplicationUserService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _dbContext;

        public ApplicationUserService(UserManager<ApplicationUser> userManager, AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<ReturnBase<string>> AddApplicationUserAsync(ApplicationUser appUser, string password)
        {
            try
            {
                IdentityResult createUserResult = await _userManager.CreateAsync(appUser, password);

                if (createUserResult.Succeeded)
                {
                    return ReturnBaseHandler.Created("", "");
                }
                return ReturnBaseHandler.Failed<string>("An error occurred while creating user.");

            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<string>($"{ex.Message}");
            }
        }

        public async Task<ReturnBase<bool>> IsEmailAlreadyRegisteredAsync(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return ReturnBaseHandler.BadRequest<bool>("Email is required.");
                }

                ApplicationUser? user = await _userManager.FindByEmailAsync(email);
                if (user is not null)
                {
                    return ReturnBaseHandler.Success(true, "Email is already registered.");
                }
                return ReturnBaseHandler.Success(false, "Email is available.");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>($"An error occurred: {ex.Message}");
            }
        }
    }
}