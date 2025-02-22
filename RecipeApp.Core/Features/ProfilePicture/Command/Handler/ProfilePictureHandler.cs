using AutoMapper;
using MediatR;
using RecipeApp.Core.Features.ApplicationUser.Command.Model;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.ApplicationUser.Command.Handler
{
    internal class ProfilePictureHandler :

        IRequestHandler<AddProfilePictureCommand, ReturnBase<bool>>,
        IRequestHandler<DeleteProfilePictureCommand, ReturnBase<bool>>,
        IRequestHandler<UpdateProfilePictureCommand, ReturnBase<bool>>
    {
        private readonly IProfilePictureService _profilePictureService;
        public ProfilePictureHandler(IProfilePictureService profilePictureService, IMapper mapper, IConfirmEmailSerivce confirmEmailSerivce)
        {
            _profilePictureService = profilePictureService;

        }

        public async Task<ReturnBase<bool>> Handle(AddProfilePictureCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ReturnBase<bool> saveProfilePictureResult = await _profilePictureService.AddProfilePictureAsync(request.Id, request.ProfilePicture);

                if (saveProfilePictureResult.Succeeded)
                {
                    return ReturnBaseHandler.Success(true, saveProfilePictureResult.Message);
                }

                switch (saveProfilePictureResult.Message)
                {
                    case "ImageIsNull":
                        return ReturnBaseHandler.Failed<bool>("No File Uploaded");
                    case "InvalidFileType":
                        return ReturnBaseHandler.Failed<bool>("Only Imgaes are allowed");
                    case "NotAllowedFileSize":
                        return ReturnBaseHandler.Failed<bool>("File size should not exceed 1 MB");
                    case "InvalidUserId":
                        return ReturnBaseHandler.Failed<bool>("User is not found");
                    default:
                        return ReturnBaseHandler.Failed<bool>(saveProfilePictureResult.Message);
                }
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> Handle(DeleteProfilePictureCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ReturnBase<bool> deleteProfilePictureResult = await _profilePictureService.DeleteProfilePictureAsync(request.Id, request.PictureName);

                if (deleteProfilePictureResult.Succeeded)
                {
                    return ReturnBaseHandler.Deleted<bool>(deleteProfilePictureResult.Message);
                }

                switch (deleteProfilePictureResult.Message)
                {
                    case "PictureNameIsRequired":
                        return ReturnBaseHandler.Failed<bool>("Picture name is required");
                    case "InvalidUserId":
                        return ReturnBaseHandler.Failed<bool>("User is not found");
                    default:
                        return ReturnBaseHandler.Failed<bool>(deleteProfilePictureResult.Message);
                }
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> Handle(UpdateProfilePictureCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ReturnBase<bool> updateProfilePictureResult = await _profilePictureService.UpdateProfilePictureAsync(request.Id, request.ProfilePicture);

                if (updateProfilePictureResult.Succeeded)
                {
                    return ReturnBaseHandler.Success(true, updateProfilePictureResult.Message);
                }

                switch (updateProfilePictureResult.Message)
                {
                    case "ImageIsNull":
                        return ReturnBaseHandler.Failed<bool>("No File Uploaded");
                    case "InvalidFileType":
                        return ReturnBaseHandler.Failed<bool>("Only Imgaes are allowed");
                    case "NotAllowedFileSize":
                        return ReturnBaseHandler.Failed<bool>("File size should not exceed 1 MB");
                    case "InvalidUserId":
                        return ReturnBaseHandler.Failed<bool>("User is not found");
                    default:
                        return ReturnBaseHandler.Failed<bool>(updateProfilePictureResult.Message);
                }
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
    }
}