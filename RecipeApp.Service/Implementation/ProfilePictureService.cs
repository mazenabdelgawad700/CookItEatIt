using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RecipeApp.Domain.Entities.Identity;
using RecipeApp.Infrastructure.Context;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Implementation
{
    internal class ProfilePictureService : IProfilePictureService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _dbContext;
        private readonly IConfirmEmailService _confirmEmailSerivce;
        private readonly IFileService _fileService;

        public ProfilePictureService(UserManager<ApplicationUser> userManager, AppDbContext dbContext, IConfirmEmailService confirmEmailSerivce, IFileService fileService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _confirmEmailSerivce = confirmEmailSerivce;
            _fileService = fileService;
        }


        public async Task<ReturnBase<bool>> AddProfilePictureAsync(int userId, IFormFile imageFile)
        {
            try
            {
                if (imageFile is null)
                    return ReturnBaseHandler.Failed<bool>("ImageIsNull");

                if (!imageFile.ContentType.StartsWith("image/"))
                    return ReturnBaseHandler.Failed<bool>("InvalidFileType");

                if (imageFile?.Length > 1 * 1024 * 1024)
                    return ReturnBaseHandler.Failed<bool>("NotAllowedFileSize");

                ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());
                if (user is null)
                    return ReturnBaseHandler.Failed<bool>("InvalidUserId");

                string[] allowedFileExtentions = [".jpg", ".jpeg", ".png"];
                ReturnBase<string> savePictureResult = await _fileService.SaveFileAsync(imageFile, allowedFileExtentions);

                if (savePictureResult.Succeeded)
                {
                    user.ProfilePictureURL = savePictureResult.Data;
                    await _dbContext.SaveChangesAsync();
                    return ReturnBaseHandler.Success(true, savePictureResult.Message);
                }

                return ReturnBaseHandler.Failed<bool>(savePictureResult.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> DeleteProfilePictureAsync(int userId, string pictureName)
        {
            try
            {
                if (pictureName is null)
                    return ReturnBaseHandler.Failed<bool>("PictureNameIsRequired");

                ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());
                if (user is null)
                    return ReturnBaseHandler.Failed<bool>("InvalidUserId");

                ReturnBase<bool> deletePictureResult = _fileService.DeleteFile(pictureName);

                if (deletePictureResult.Succeeded)
                {
                    user.ProfilePictureURL = null;
                    await _dbContext.SaveChangesAsync();
                    return ReturnBaseHandler.Success(true, deletePictureResult.Message);
                }

                return ReturnBaseHandler.Failed<bool>(deletePictureResult.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> UpdateProfilePictureAsync(int userId, IFormFile imageFile)
        {
            try
            {
                if (imageFile is null)
                    return ReturnBaseHandler.Failed<bool>("ImageIsNull");

                if (!imageFile.ContentType.StartsWith("image/"))
                    return ReturnBaseHandler.Failed<bool>("InvalidFileType");

                if (imageFile?.Length > 1 * 1024 * 1024)
                    return ReturnBaseHandler.Failed<bool>("NotAllowedFileSize");

                ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());
                if (user is null)
                    return ReturnBaseHandler.Failed<bool>("InvalidUserId");

                if (user.ProfilePictureURL is not null)
                {
                    int startIndex = user.ProfilePictureURL.LastIndexOf('\\');
                    string pictureName = user.ProfilePictureURL.Substring(startIndex + 1);

                    ReturnBase<bool> deletePictureResult = _fileService.DeleteFile(pictureName);

                    if (!deletePictureResult.Succeeded)
                        return ReturnBaseHandler.Failed<bool>(deletePictureResult.Message);
                }

                string[] allowedFileExtentions = [".jpg", ".jpeg", ".png"];
                ReturnBase<string> updatePictureResult = await _fileService.SaveFileAsync(imageFile, allowedFileExtentions);

                if (updatePictureResult.Succeeded)
                {
                    user.ProfilePictureURL = updatePictureResult.Data;
                    await _dbContext.SaveChangesAsync();
                    return ReturnBaseHandler.Success(true, "Profile Picture Updated Successfully");
                }

                return ReturnBaseHandler.Failed<bool>(updatePictureResult.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
    }
}