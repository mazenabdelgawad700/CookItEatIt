using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecipeApp.Domain.Entities.Identity;
using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Infrastructure.Context;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;
using RecipeApp.Shared.SharedResponse;

namespace RecipeApp.Service.Implementation
{
    internal class ApplicationUserService : IApplicationUserService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _dbContext;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IUserFollowerRepository _userFollowerRepository;
        private readonly IUserPreferencesService _userPreferencesService;
        private readonly IUserPreferencesRepository _userPreferencesRepository;

        public ApplicationUserService(UserManager<ApplicationUser> userManager, IApplicationUserRepository applicationUserRepository, AppDbContext dbContext, IUserFollowerRepository userFollowerRepository, IUserPreferencesService userPreferencesService, IUserPreferencesRepository userPreferencesRepository)
        {
            _userManager = userManager;
            _applicationUserRepository = applicationUserRepository;
            _dbContext = dbContext;
            _userFollowerRepository = userFollowerRepository;
            _userPreferencesService = userPreferencesService;
            _userPreferencesRepository = userPreferencesRepository;
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
        public async Task<ReturnBase<GetUserSettingsResponse>> GetApplicationUserSettingsAsync(int userId)
        {
            try
            {
                var getUserResult = await GetApplicationUserProfileByIdAsync(userId);
                if (!getUserResult.Succeeded)
                    return ReturnBaseHandler.Failed<GetUserSettingsResponse>(getUserResult.Message);

                var getUserPreferencesResult = await _userPreferencesService.GetUserPreferencesByUserIdAsync(userId);

                if (!getUserPreferencesResult.Succeeded)
                    return ReturnBaseHandler.Failed<GetUserSettingsResponse>(getUserPreferencesResult.Message);

                var response = new GetUserSettingsResponse()
                {
                    PreferredTheme = getUserResult.Data.PreferredTheme,
                    AcceptNewDishNotification = getUserPreferencesResult.Data.AcceptNewDishNotification
                };

                return ReturnBaseHandler.Success(response);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<GetUserSettingsResponse>(ex.Message);
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
        public async Task<ReturnBase<bool>> ToggleUserFollowAsync(int followerId, int followingId)
        {
            var transaction = await _applicationUserRepository.BeginTransactionAsync();
            try
            {
                ReturnBase<ApplicationUser> follower = await _applicationUserRepository.GetByIdAsync(followerId);

                if (!follower.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(follower.Message);

                ReturnBase<ApplicationUser> following = await _applicationUserRepository.GetByIdAsync(followingId);

                if (!following.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(follower.Message);

                if (followerId == followingId)
                    return ReturnBaseHandler.Failed<bool>("You can't follow yourself");

                var isFollowing = await _userFollowerRepository.GetTableNoTracking().Data.FirstOrDefaultAsync(f => f.FollowingId == followingId && f.FollowerId == followerId);

                if (isFollowing is not null)
                {
                    var followerToRemove = new UserFollower()
                    {
                        FollowerId = followerId,
                        FollowingId = followingId
                    };
                    var removeFollowingResult = await _userFollowerRepository.RemoveUserFollowAsync(followerToRemove);

                    if (!removeFollowingResult.Succeeded)
                        return ReturnBaseHandler.Failed<bool>(removeFollowingResult.Message);

                    follower.Data.FollowingCount--;
                    following.Data.FollowersCount--;

                }
                else
                {
                    var followerToAdd = new UserFollower()
                    {
                        FollowerId = followerId,
                        FollowingId = followingId,
                        FollowedAt = DateTime.UtcNow
                    };
                    var addFollowingResult = await _userFollowerRepository.AddAsync(followerToAdd);

                    if (!addFollowingResult.Succeeded)
                        return ReturnBaseHandler.Failed<bool>(addFollowingResult.Message);

                    follower.Data.FollowingCount++;
                    following.Data.FollowersCount++;
                }

                await _applicationUserRepository.UpdateAsync(follower.Data);
                await _applicationUserRepository.UpdateAsync(following.Data);

                var verifyChefResult = await VerifyChefAsync(following.Data);

                if (!verifyChefResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return ReturnBaseHandler.Failed<bool>(verifyChefResult.Message);
                }

                await transaction.CommitAsync();
                return ReturnBaseHandler.Success(true, "");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
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
        public async Task<ReturnBase<bool>> UpdateApplicationUserSettingsAsync(UpdateApplicationUserSettingsCommandShared newUserSettings)
        {
            var transaction = await _applicationUserRepository.BeginTransactionAsync();
            try
            {
                var getUserResult = await GetApplicationUserProfileByIdAsync(newUserSettings.Id);
                if (!getUserResult.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(getUserResult.Message);

                var getUserPreferencesResult = await _userPreferencesService.GetUserPreferencesByUserIdAsync(newUserSettings.Id);

                if (!getUserPreferencesResult.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(getUserPreferencesResult.Message);

                getUserResult.Data.PreferredTheme = newUserSettings.PreferredTheme;

                getUserPreferencesResult.Data.AcceptNewDishNotification = newUserSettings.AcceptNewDishNotification;

                var updateUserPreferredTheme = await _applicationUserRepository.UpdateAsync(getUserResult.Data);

                if (!updateUserPreferredTheme.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return ReturnBaseHandler.Failed<bool>(updateUserPreferredTheme.Message);
                }

                var updateUserPreferences = await _userPreferencesRepository.UpdateAsync(getUserPreferencesResult.Data);
                if (!updateUserPreferences.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return ReturnBaseHandler.Failed<bool>(updateUserPreferences.Message);
                }

                await transaction.CommitAsync();
                return ReturnBaseHandler.Success(true);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }

        public Task<ReturnBase<GetUserSettingsResponse>> UpdateApplicationUserSettingsAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public ReturnBase<IQueryable<ApplicationUser>> VerifiedChefList()
        {
            try
            {
                var verifiedchefs = _applicationUserRepository.GetTableNoTracking().Data.Where(x => x.IsVerifiedChef);

                if (verifiedchefs is null)
                    return ReturnBaseHandler.Failed<IQueryable<ApplicationUser>>();

                if (!verifiedchefs.Any())
                    return ReturnBaseHandler.Success(verifiedchefs, "No Verified Chef Found");

                return ReturnBaseHandler.Success(verifiedchefs);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<IQueryable<ApplicationUser>>(ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> VerifyChefAsync(ApplicationUser user)
        {
            try
            {

                if (user is null)
                    return ReturnBaseHandler.Failed<bool>("User Not Found");

                if (!user.IsVerifiedChef)
                {
                    if (user.EmailConfirmed && user.FollowersCount >= 10000 && user.RecipesCount >= 200)
                    {
                        user.IsVerifiedChef = true;
                        var updateUserResult = await _applicationUserRepository.UpdateAsync(user);

                        if (!updateUserResult.Succeeded)
                            return ReturnBaseHandler.Failed<bool>(updateUserResult.Message);
                    }
                }
                return ReturnBaseHandler.Success(true);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
    }
}
