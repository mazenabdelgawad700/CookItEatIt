using AutoMapper;
using MediatR;
using RecipeApp.Core.Features.PreferredDishFeature.Command.Model;
using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.PreferredDishFeature.Command.Handler
{
    internal class PreferredDishHandler : IRequestHandler<AddPreferredDishCommand, ReturnBase<bool>>
        , IRequestHandler<UpdatePreferredDishCommand, ReturnBase<bool>>
    {
        private readonly IPreferredDishService _preferredDishService;
        private readonly IPreferredDishRepository _preferredDishRepository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        internal static readonly string[] allowedFileExtensions = [".jpg", ".jpeg", ".png"];

        public PreferredDishHandler(IPreferredDishService preferredDishService, IMapper mapper, IFileService fileService, IPreferredDishRepository preferredDishRepository)
        {
            _preferredDishService = preferredDishService;
            _mapper = mapper;
            _fileService = fileService;
            _preferredDishRepository = preferredDishRepository;
        }

        public async Task<ReturnBase<bool>> Handle(AddPreferredDishCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ReturnBase<string> addImage = await _fileService.SaveFileAsync(request.DishImage, allowedFileExtensions);

                if (!addImage.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(addImage.Message);

                PreferredDish mappedResult = _mapper.Map<PreferredDish>(request);

                mappedResult.ImageUrl = addImage.Data;

                var addPreferredDishResult = await _preferredDishService.AddPreferredDishAsync(mappedResult);

                if (addPreferredDishResult.Succeeded)
                    return ReturnBaseHandler.Created(true, "Preferred Dish Added Successfully");

                return ReturnBaseHandler.Failed<bool>(addPreferredDishResult.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> Handle(UpdatePreferredDishCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ReturnBase<PreferredDish> getPreferredDishResult = await _preferredDishRepository.GetPreferredDishByIdAsNoTracking(request.Id);

                if (getPreferredDishResult.Data.Recipes is not null && getPreferredDishResult.Data.Recipes.Count > 0)
                    return ReturnBaseHandler.BadRequest<bool>("Preferred Dish is in use, can't update");

                if (!getPreferredDishResult.Succeeded)
                    return ReturnBaseHandler.BadRequest<bool>(getPreferredDishResult.Message);

                PreferredDish mappedResult = _mapper.Map<PreferredDish>(request);

                if (request.DishName is null || request.DishName.Equals("null", StringComparison.CurrentCultureIgnoreCase))
                    mappedResult.DishName = getPreferredDishResult.Data.DishName;

                if (request.DishImage is null)
                {
                    mappedResult.ImageUrl = getPreferredDishResult.Data.ImageUrl;
                }
                else
                {
                    // Delete old image
                    int startIndex = getPreferredDishResult.Data.ImageUrl.LastIndexOf('\\');
                    string pictureName = getPreferredDishResult.Data.ImageUrl.Substring(startIndex + 1);
                    var deleteOldImageResult = _fileService.DeleteFile(pictureName);
                    if (!deleteOldImageResult.Succeeded)
                        return ReturnBaseHandler.Failed<bool>(deleteOldImageResult.Message);

                    // Add new image
                    ReturnBase<string> addImageResult = await _fileService.SaveFileAsync(request.DishImage, allowedFileExtensions);

                    if (!addImageResult.Succeeded)
                        return ReturnBaseHandler.Failed<bool>(deleteOldImageResult.Message);

                    mappedResult.ImageUrl = addImageResult.Data;
                }

                var updatePreferredDishResult = await _preferredDishService.UpdatePreferredDishAsync(mappedResult);

                if (updatePreferredDishResult.Succeeded)
                    return ReturnBaseHandler.Updated<bool>("Preferred Dish Updated Successfully");

                return ReturnBaseHandler.Failed<bool>(updatePreferredDishResult.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
    }
}
