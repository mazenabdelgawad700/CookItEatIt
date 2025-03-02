using MediatR;
using RecipeApp.Core.Features.UserPreferredCategoryFeature.Command.Model;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.UserPreferredCategoryFeature.Command.Handler
{
  public class UserPreferredCategoriesHandler :
      IRequestHandler<SaveUserPreferredCategoriesCommand, ReturnBase<bool>>
  {
    private readonly IUserPreferredCategoriesService _userPreferredCategoriesService;

    public UserPreferredCategoriesHandler(IUserPreferredCategoriesService userPreferredCategoriesService)
    {
      _userPreferredCategoriesService = userPreferredCategoriesService;
    }

    public async Task<ReturnBase<bool>> Handle(
        SaveUserPreferredCategoriesCommand request,
        CancellationToken cancellationToken)
    {
      try
      {
        return await _userPreferredCategoriesService.SaveUserPreferredCategoriesAsync(
            request.UserId,
            request.CategoryIds);
      }
      catch (Exception ex)
      {
        return ReturnBaseHandler.Failed<bool>(ex.Message);
      }
    }
  }
}