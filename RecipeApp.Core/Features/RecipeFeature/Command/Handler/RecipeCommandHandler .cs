using AutoMapper;
using MediatR;
using RecipeApp.Core.Features.RecipeFeature.Command.Model;
using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.RecipeFeature.Command.Handler
{
    public class RecipeCommandHandler : IRequestHandler<CreateRecipeCommand, ReturnBase<int>>,
         IRequestHandler<AddRecipeImageCommand, ReturnBase<bool>>,
         IRequestHandler<UpdateRecipeCommand, ReturnBase<bool>>,
         IRequestHandler<UpdateRecipeImageCommand, ReturnBase<bool>>
    {
        private readonly IRecipeService _recipeService;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        internal static readonly string[] allowedFileExtensions = [".jpg", ".jpeg", ".png"];

        public RecipeCommandHandler(IRecipeService recipeService, IMapper mapper, IFileService fileService, IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
            _recipeService = recipeService;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<ReturnBase<int>> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var mappedResult = _mapper.Map<Recipe>(request);

                var createRecipeResult = await _recipeService.CreateRecipeAsync(mappedResult);
                if (createRecipeResult.Succeeded)
                    return ReturnBaseHandler.Created(createRecipeResult.Data, "Recipe Added Successfully");
                return ReturnBaseHandler.Failed<int>(createRecipeResult.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<int>(ex.Message);
            }
        }

        public async Task<ReturnBase<bool>> Handle(AddRecipeImageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ReturnBase<bool> addRecipeImageResult = await _recipeService.AddRecipeImageAsync(request.Id, request.ImageFile, allowedFileExtensions);

                if (!addRecipeImageResult.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(addRecipeImageResult.Message);

                return ReturnBaseHandler.Created(true, "Recipe Image Added Successfully");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }

        public async Task<ReturnBase<bool>> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Recipe mappedResult = _mapper.Map<Recipe>(request);

                ReturnBase<bool> updateRecipeResult = await _recipeService.UpdateRecipeAsync(mappedResult);

                if (!updateRecipeResult.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(updateRecipeResult.Message);

                return ReturnBaseHandler.Success(true, "Recipe Updated Successfully");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }

        public async Task<ReturnBase<bool>> Handle(UpdateRecipeImageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ReturnBase<bool> addRecipeImageResult = await _recipeService.UpdateRecipeImageAsync(request.Id, request.ImageFile, allowedFileExtensions);

                if (!addRecipeImageResult.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(addRecipeImageResult.Message);

                return ReturnBaseHandler.Updated<bool>(addRecipeImageResult.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
    }
}
