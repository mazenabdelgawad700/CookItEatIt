using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.InfrastructureBases;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Infrastructure.Abstracts
{
    public interface IUserFollowerRepository : IGenericRepositoryAsync<UserFollower>
    {
        Task<ReturnBase<bool>> RemoveUserFollowAsync(UserFollower userFollower);
    }
}
