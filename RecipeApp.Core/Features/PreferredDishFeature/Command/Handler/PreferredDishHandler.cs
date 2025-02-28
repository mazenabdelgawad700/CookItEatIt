using AutoMapper;
using MediatR;
using RecipeApp.Core.Features.PreferredDishFeature.Command.Model;
using RecipeApp.Domain.Entities.Models;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.PreferredDishFeature.Command.Handler
{
    internal class PreferredDishHandler : IRequestHandler<AddPreferredDishCommand, ReturnBase<bool>>
    {
        private readonly IPreferredDishService _preferredDishService;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        internal static readonly string[] allowedFileExtensions = new string[] { ".jpg", ".jpeg", ".png" };

        public PreferredDishHandler(IPreferredDishService preferredDishService, IMapper mapper, IFileService fileService)
        {
            _preferredDishService = preferredDishService;
            _mapper = mapper;
            _fileService = fileService;
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
    }
}
