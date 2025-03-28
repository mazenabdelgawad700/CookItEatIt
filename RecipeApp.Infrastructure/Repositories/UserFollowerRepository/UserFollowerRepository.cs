using Microsoft.EntityFrameworkCore;
using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Infrastructure.Context;
using RecipeApp.Infrastructure.InfrastructureBases;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Infrastructure.Repositories.UserFollowerRepository
{
    internal class UserFollowerRepository : GenericRepositoryAsync<UserFollower>, IUserFollowerRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<UserFollower> _dbSet;
        public UserFollowerRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<UserFollower>();
        }

        public async Task<ReturnBase<bool>> RemoveUserFollowAsync(UserFollower userFollower)
        {
            try
            {
                var removeUserFollow = _dbSet.Remove(userFollower);
                await _dbContext.SaveChangesAsync();
                return ReturnBaseHandler.Success(true, "User Unfollowed");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
    }
}
