using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly IRecipeCategoryRepository _recipeCategoryRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IInstructionRepository _instructionRepository;
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IRecipeLikeRepository _recipeLikeRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IFileService _fileService;
        private readonly UserManager<ApplicationUser> _userManager;

        public RecipeService(
            IRecipeRepository recipeRepository,
            IFileService fileService, UserManager<ApplicationUser> userManager
            , IApplicationUserRepository applicationUserRepository, IRecipeCategoryRepository recipeCategoryRepository, ICategoryRepository categoryRepository, IInstructionRepository instructionRepository, IIngredientRepository ingredientRepository, IRecipeLikeRepository recipeLikeRepository)
        {
            _recipeRepository = recipeRepository;
            _applicationUserRepository = applicationUserRepository;
            _fileService = fileService;
            _userManager = userManager;
            _recipeCategoryRepository = recipeCategoryRepository;
            _categoryRepository = categoryRepository;
            _instructionRepository = instructionRepository;
            _ingredientRepository = ingredientRepository;
            _recipeLikeRepository = recipeLikeRepository;
        }

        public async Task<ReturnBase<bool>> AddRecipeCategoriesAsync(int recipeId, List<int> categoryIds)
        {
            try
            {
                var getRecipeResult = await _recipeRepository.GetByIdAsync(recipeId);

                if (!getRecipeResult.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(getRecipeResult.Message);

                var validateCategoriesResult = await _categoryRepository.GetTableNoTracking()
                    .Data.Where(c => categoryIds.Contains(c.Id))
                         .CountAsync();

                if (validateCategoriesResult != categoryIds.Count)
                    return ReturnBaseHandler.Failed<bool>("One or more categories are invalid");


                var addRecipeCategoryResult = await _recipeCategoryRepository
                    .AddRangeAsync(categoryIds.Select(c => new RecipeCategory
                    {
                        RecipeId = recipeId,
                        CategoryId = c
                    }).ToList());

                if (!addRecipeCategoryResult.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(addRecipeCategoryResult.Message);

                return ReturnBaseHandler.Success(true, "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
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
        public async Task<ReturnBase<bool>> DeleteRecipeAsync(Recipe recipe)
        {
            var transaction = await _recipeRepository.BeginTransactionAsync();
            try
            {
                ReturnBase<Recipe>? getRecipeResult = await _recipeRepository.GetRecipeById(recipe.Id);

                if (!getRecipeResult.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(getRecipeResult.Message);

                ReturnBase<ApplicationUser>? getUserResult = await _applicationUserRepository.GetByIdAsync(recipe.UserId);

                if (!getUserResult.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(getUserResult.Message);

                if (getRecipeResult.Data.Instructions.Count > 0)
                {
                    ReturnBase<bool> deleteRecipeInstructionsResult = await _instructionRepository.DeleteRangeAsync(getRecipeResult.Data.Instructions);

                    if (!deleteRecipeInstructionsResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return ReturnBaseHandler.Failed<bool>(deleteRecipeInstructionsResult.Message);
                    }
                }

                if (getRecipeResult.Data.Ingredients.Count > 0)
                {
                    ReturnBase<bool> deleteRecipeIngredientsResult = await _ingredientRepository.DeleteRangeAsync(getRecipeResult.Data.Ingredients);

                    if (!deleteRecipeIngredientsResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return ReturnBaseHandler.Failed<bool>(deleteRecipeIngredientsResult.Message);
                    }
                }

                if (getRecipeResult.Data.RecipeCategories.Count > 0)
                {
                    ReturnBase<bool> deleteRecipeCategoriesResult = await _recipeCategoryRepository.DeleteRangeAsync(getRecipeResult.Data.RecipeCategories);

                    if (!deleteRecipeCategoriesResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return ReturnBaseHandler.Failed<bool>(deleteRecipeCategoriesResult.Message);
                    }
                }

                if (getRecipeResult.Data.LikesCount > 0)
                {
                    ReturnBase<bool> deleteRecipeLikesResult = await _recipeLikeRepository.DeleteRangeAsync(getRecipeResult.Data.Likes);

                    if (!deleteRecipeLikesResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return ReturnBaseHandler.Failed<bool>(deleteRecipeLikesResult.Message);
                    }
                }


                ReturnBase<bool> deleteRecipeResult = await _recipeRepository.DeleteAsync(recipe.Id);

                if (!deleteRecipeResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return ReturnBaseHandler.Failed<bool>(deleteRecipeResult.Message);
                }

                getUserResult.Data.RecipesCount--;
                ReturnBase<bool> updateUserRecipesCountResult = await _applicationUserRepository.UpdateAsync(getUserResult.Data);

                if (!updateUserRecipesCountResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return ReturnBaseHandler.Failed<bool>(updateUserRecipesCountResult.Message);
                }

                await transaction.CommitAsync();

                // Delete the image from the server if and only if everything went well.
                // The reason why it is here after the commit is that we can not rollback the image if anything goes wrong ^_^
                if (!string.IsNullOrEmpty(recipe.ImgURL))
                {
                    int startIndex = recipe.ImgURL.LastIndexOf('\\');
                    string pictureName = recipe.ImgURL[(startIndex + 1)..];

                    ReturnBase<bool> deletePictureResult = _fileService.DeleteFile(pictureName);

                    if (!deletePictureResult.Succeeded)
                        return ReturnBaseHandler.Failed<bool>(deletePictureResult.Message);
                }

                return ReturnBaseHandler.Success(true, "Recipe Deleted Successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
        public async Task<ReturnBase<Recipe>> GetRecipeByIdAsync(int recipeId)
        {
            try
            {
                var getRecipeResult = await _recipeRepository.GetRecipeById(recipeId);

                if (!getRecipeResult.Succeeded)
                    return ReturnBaseHandler.Failed<Recipe>(getRecipeResult.Message);

                return ReturnBaseHandler.Success(getRecipeResult.Data);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<Recipe>(ex.Message);
            }
        }
        public ReturnBase<IQueryable<Recipe>> GetRecipesForUser(int userId)
        {
            try
            {
                var getRecipesResult = _recipeRepository.GetRecipesForUser(userId);

                if (getRecipesResult.Succeeded)
                    return ReturnBaseHandler.Success(getRecipesResult.Data, "");
                return ReturnBaseHandler.Failed<IQueryable<Recipe>>(getRecipesResult.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<IQueryable<Recipe>>(ex.Message);
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
        public async Task<ReturnBase<bool>> UpdateRecipeCategoriesAsync(int recipeId, List<int> categoryIds)
        {
            try
            {
                var getRecipeResult = await _recipeRepository.GetByIdAsync(recipeId);

                if (!getRecipeResult.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(getRecipeResult.Message);

                var validateCategoriesResult = await _categoryRepository.GetTableNoTracking()
                    .Data.Where(c => categoryIds.Contains(c.Id))
                         .CountAsync();

                if (validateCategoriesResult != categoryIds.Count)
                    return ReturnBaseHandler.Failed<bool>("One or more categories are invalid");

                var existingRecipeCategories = await _recipeCategoryRepository.GetTableNoTracking()
                    .Data.Where(rc => rc.RecipeId == recipeId)
                    .ToListAsync();

                if (existingRecipeCategories.Any())
                {
                    var deleteResult = await _recipeCategoryRepository.DeleteRangeAsync(existingRecipeCategories);
                    if (!deleteResult.Succeeded)
                        return ReturnBaseHandler.Failed<bool>(deleteResult.Message);
                }

                var newRecipeCategories = categoryIds.Select(c => new RecipeCategory
                {
                    RecipeId = recipeId,
                    CategoryId = c
                }).ToList();

                var addRecipeCategoryResult = await _recipeCategoryRepository.AddRangeAsync(newRecipeCategories);

                if (!addRecipeCategoryResult.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(addRecipeCategoryResult.Message);

                return ReturnBaseHandler.Success(true, "");
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