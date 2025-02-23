using RecipeApp.Domain.Entities.Identity;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Abstraction
{
    public interface ISendPasswordChangeNotificationEmailService
    {
        Task<ReturnBase<bool>> SendPasswordChangeNotificationAsync(ApplicationUser user);
    }
}
