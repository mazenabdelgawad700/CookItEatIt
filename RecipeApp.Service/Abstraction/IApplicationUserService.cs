using RecipeApp.Domain.Entities.Identity;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Abstraction
{
    public interface IApplicationUserService
    {
        Task<ReturnBase<ApplicationUser>> GetApplicationUserProfileByIdAsync(int userId);
        Task<ReturnBase<bool>> UpdateApplicationUserAsync(ApplicationUser user);
        Task<ReturnBase<bool>> IsCountryValidAsync(int? countryId);
        Task<ReturnBase<bool>> ToggleUserFollowAsync(int followerId, int followingId);
    }
}
