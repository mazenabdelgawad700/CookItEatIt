using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecipeApp.Domain.Entities.Identity;
using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Infrastructure.Context;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Implementation
{
    internal class ApplicationUserService : IApplicationUserService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _dbContext;
        private readonly IApplicationUserRepository _applicationUserRepository;

        public ApplicationUserService(UserManager<ApplicationUser> userManager, IApplicationUserRepository applicationUserRepository, AppDbContext dbContext)
        {
            _userManager = userManager;
            _applicationUserRepository = applicationUserRepository;
            _dbContext = dbContext;
        }

        public async Task<ReturnBase<ApplicationUser>> GetApplicationUserProfileByIdAsync(int userId)
        {
            try
            {
                ApplicationUser? user = await _userManager.Users
                                                          .Include(u => u.Country)
                                                          .FirstOrDefaultAsync(u => u.Id == userId);

                if (user is null)
                    return ReturnBaseHandler.Failed<ApplicationUser>("User Not Found");
                return ReturnBaseHandler.Success(user, "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<ApplicationUser>(ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> IsCountryValidAsync(int? countryId)
        {
            try
            {

                if (countryId is null)
                    return ReturnBaseHandler.Failed<bool>("Invalid Country");

                Country? country = await _dbContext.Country.Where(x => x.Id == countryId)
                                                        .FirstOrDefaultAsync();
                if (country is null)
                    return ReturnBaseHandler.Failed<bool>("Invalid Country");
                return ReturnBaseHandler.Success(true, "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
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
