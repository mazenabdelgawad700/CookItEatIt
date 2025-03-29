using RecipeApp.Domain.Entities.Identity;
using RecipeApp.Shared.Bases;
using RecipeApp.Shared.SharedResponse;

namespace RecipeApp.Service.Abstraction
{
    public interface IApplicationUserService
    {
        Task<ReturnBase<ApplicationUser>> GetApplicationUserProfileByIdAsync(int userId);
        Task<ReturnBase<GetUserSettingsResponse>> GetApplicationUserSettingsAsync(int userId);
        Task<ReturnBase<bool>> UpdateApplicationUserSettingsAsync(UpdateApplicationUserSettingsCommandShared newUserSettings);
        Task<ReturnBase<bool>> UpdateApplicationUserAsync(ApplicationUser user);
        Task<ReturnBase<bool>> IsCountryValidAsync(int? countryId);
        Task<ReturnBase<bool>> ToggleUserFollowAsync(int followerId, int followingId);
        Task<ReturnBase<bool>> VerifyChefAsync(ApplicationUser user);
        ReturnBase<IQueryable<ApplicationUser>> VerifiedChefList();
    }
}
