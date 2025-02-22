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
        private readonly IConfirmEmailSerivce _confirmEmailSerivce;

        public ApplicationUserService(UserManager<ApplicationUser> userManager, AppDbContext dbContext, IConfirmEmailSerivce confirmEmailSerivce)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _confirmEmailSerivce = confirmEmailSerivce;
        }

        public async Task<ReturnBase<string>> AddApplicationUserAsync(ApplicationUser appUser, string password)
        {
            try
            {
                IdentityResult createUserResult = await _userManager.CreateAsync(appUser, password);

                if (createUserResult.Succeeded)
                {
                    ReturnBase<bool> sendConfirmationEmailResult = await _confirmEmailSerivce.SendConfirmationEmailAsync(appUser);
                    if (sendConfirmationEmailResult.Data)
                    {
                        return ReturnBaseHandler.Created("", $"Confirmation Email has been sent to {appUser.Email} Please, confirm your email");
                    }
                    return ReturnBaseHandler.Created("", "We could not send a confirmation email to you, please log in to confirm your email!");
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