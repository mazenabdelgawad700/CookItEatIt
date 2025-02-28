using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Implementation
{
    internal class PreferredDishService : IPreferredDishService
    {
        private readonly IPreferredDishRepository _preferredDishRepository;

        public PreferredDishService(IPreferredDishRepository preferredDishRepository)
        {
            _preferredDishRepository = preferredDishRepository;
        }

        public async Task<ReturnBase<bool>> AddPreferredDishAsync(PreferredDish preferredDish)
        {
            try
            {
                ReturnBase<bool> exsitingDishResult = await
                    _preferredDishRepository.AddAsync(preferredDish);

                if (exsitingDishResult.Succeeded)
                    return ReturnBaseHandler.Created(true, exsitingDishResult.Message);

                return ReturnBaseHandler.BadRequest<bool>(exsitingDishResult.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> IsPreferredDishExistAsync(string dishName)
        {
            try
            {
                ReturnBase<bool> exsitingDishResult = await
                    _preferredDishRepository.IsPreferredDishExistAsync(dishName);

                if (exsitingDishResult.Data)
                    return ReturnBaseHandler.Success(true, exsitingDishResult.Message);

                return ReturnBaseHandler.Success(false, "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
    }
}
