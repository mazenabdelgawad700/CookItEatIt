using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RecipeApp.Domain.Entities.Identity;
using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Implementation
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IFileService _fileService;
        private readonly UserManager<ApplicationUser> _userManager;

        public RecipeService(
            IRecipeRepository recipeRepository,
            IFileService fileService, UserManager<ApplicationUser> userManager
            , IApplicationUserRepository applicationUserRepository)
        {
            _recipeRepository = recipeRepository;
            _applicationUserRepository = applicationUserRepository;
            _fileService = fileService;
            _userManager = userManager;
        }

        public async Task<ReturnBase<bool>> AddRecipeImageAsync(int recipeId, IFormFile imageFile, string[] allowedExtensions)
        {
            try
            {
                var getRecipeResult = await _recipeRepository.GetByIdAsync(recipeId);

                if (!getRecipeResult.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(getRecipeResult.Message);

                var saveImageResult = await _fileService.SaveFileAsync(imageFile, allowedExtensions);

                if (!saveImageResult.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(saveImageResult.Message);

                getRecipeResult.Data.ImgURL = saveImageResult.Data;
                await _recipeRepository.SaveChangesAsync();

                return ReturnBaseHandler.Created(true, "Recipe Image Added Successfully");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
        public async Task<ReturnBase<int>> CreateRecipeAsync(Recipe recipe)
        {
            var transaction = await _recipeRepository.BeginTransactionAsync();
            try
            {
                ReturnBase<Recipe> addRecipeResult = await _recipeRepository.AddRecipeAsync(recipe);

                if (!addRecipeResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return ReturnBaseHandler.Failed<int>(addRecipeResult.Message);
                }

                if (addRecipeResult.Succeeded)
                {
                    ApplicationUser? recipeOwner = await _userManager.FindByIdAsync(recipe.UserId.ToString());

                    if (recipeOwner is null)
                    {
                        await transaction.RollbackAsync();
                        return ReturnBaseHandler.Failed<int>("User not found");
                    }

                    recipeOwner.RecipesCount++;
                    await _applicationUserRepository.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return ReturnBaseHandler.Success(addRecipeResult.Data.Id, "");
                }

                await transaction.RollbackAsync();
                return ReturnBaseHandler.Failed<int>(addRecipeResult.Message);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ReturnBaseHandler.Failed<int>(ex.Message);
            }
        }

        public async Task<ReturnBase<Recipe>> GetRecipeByIdAsync(int recipeId)
        {
            try
            {
                var getRecipeResult = await _recipeRepository.GetRecipeById(recipeId);

                if (!getRecipeResult.Succeeded)
                    return ReturnBaseHandler.Failed<Recipe>(getRecipeResult.Message);

                return ReturnBaseHandler.Success(getRecipeResult.Data, "Recipe Found Successfully");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<Recipe>(ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> UpdateRecipeAsync(Recipe recipe)
        {
            try
            {
                ReturnBase<Recipe> getRecipeResult = await _recipeRepository.GetRecipeByIdAsNoTracking(recipe.Id);

                if (!getRecipeResult.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(getRecipeResult.Message);

                if (string.IsNullOrEmpty(recipe.RecipeName) || string.IsNullOrEmpty(recipe.Description) || recipe.ServesCount == 0 || recipe.CookTimeMinutes == 0 || recipe.Ingredients.Count == 0 || recipe.Instructions.Count == 0)
                    return ReturnBaseHandler.Failed<bool>("Invalid input");

                // Check if ingredients and instructions are valid 

                recipe.ImgURL = getRecipeResult.Data.ImgURL;

                ReturnBase<bool> updateRecipeResult = await _recipeRepository.UpdateAsync(recipe);

                if (!updateRecipeResult.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(updateRecipeResult.Message);


                recipe.UpdatedAt = DateTime.UtcNow;
                await _recipeRepository.SaveChangesAsync();

                return ReturnBaseHandler.Success(true, "Recipe Updated Successfully");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }

        public async Task<ReturnBase<bool>> UpdateRecipeImageAsync(int recipeId, IFormFile imageFile, string[] allowedExtensions)
        {
            var transaction = await _recipeRepository.BeginTransactionAsync();
            try
            {
                var getRecipeResult = await _recipeRepository.GetByIdAsync(recipeId);

                if (!getRecipeResult.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(getRecipeResult.Message);

                if (getRecipeResult.Data.ImgURL is not null)
                {
                    int startIndex = getRecipeResult.Data.ImgURL.LastIndexOf('\\');
                    string pictureName = getRecipeResult.Data.ImgURL.Substring(startIndex + 1);
                    ReturnBase<bool> deletePictureResult = _fileService.DeleteFile(pictureName);

                    if (!deletePictureResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return ReturnBaseHandler.Failed<bool>(deletePictureResult.Message);
                    }
                }

                var saveImageResult = await _fileService.SaveFileAsync(imageFile, allowedExtensions);

                if (!saveImageResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return ReturnBaseHandler.Failed<bool>(saveImageResult.Message);
                }

                getRecipeResult.Data.ImgURL = saveImageResult.Data;
                getRecipeResult.Data.UpdatedAt = DateTime.UtcNow;
                await _recipeRepository.SaveChangesAsync();
                await transaction.CommitAsync();

                return ReturnBaseHandler.Updated<bool>("Recipe Image Updated Successfully");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
    }
}