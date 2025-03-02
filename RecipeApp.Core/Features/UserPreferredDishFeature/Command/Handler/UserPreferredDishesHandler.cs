using MediatR;
using RecipeApp.Core.Features.UserPreferredDishFeature.Command.Model;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.UserPreferredDishFeature.Command.Handler
{
  public class UserPreferredDishesHandler :
      IRequestHandler<SaveUserPreferredDishesCommand, ReturnBase<bool>>
  {
    private readonly IUserPreferredDishesService _userPreferredDishesService;

    public UserPreferredDishesHandler(IUserPreferredDishesService userPreferredDishesService)
    {
      _userPreferredDishesService = userPreferredDishesService;
    }

    public async Task<ReturnBase<bool>> Handle(
        SaveUserPreferredDishesCommand request,
        CancellationToken cancellationToken)
    {
      try
      {
        return await _userPreferredDishesService.SaveUserPreferredDishesAsync(
            request.UserId,
            request.PreferredDishIds);
      }
      catch (Exception ex)
      {
        return ReturnBaseHandler.Failed<bool>(ex.Message);
      }
    }
  }
}