﻿using RecipeApp.Domain.Entities.Models;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Abstraction
{
  public interface IPreferredDishService
  {
    Task<ReturnBase<bool>> IsPreferredDishExistAsync(string dishName);
    Task<ReturnBase<bool>> AddPreferredDishAsync(PreferredDish preferredDish);
    Task<ReturnBase<bool>> UpdatePreferredDishAsync(PreferredDish preferredDish);
    Task<ReturnBase<bool>> DeletePreferredDishAsync(int id);
  }
}
