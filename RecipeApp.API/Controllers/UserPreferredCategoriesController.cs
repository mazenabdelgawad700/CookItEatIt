using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeApp.API.Base;
using RecipeApp.Core.Features.UserPreferredCategoryFeature.Command.Model;
using RecipeApp.Shared.Bases;

namespace RecipeApp.API.Controllers
{
  [Route("api/user-preferred-categories/[action]")]
  public class UserPreferredCategoriesController : AppControllerBase
  {
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> SaveAsync([FromBody] SaveUserPreferredCategoriesCommand command)
    {
      string? userIdFromToken = User.FindFirst("UserId")?.Value;

      if (userIdFromToken is null)
        return Unauthorized(ReturnBaseHandler.Failed<bool>("Invalid Token"));

      if (command.UserId.ToString() != userIdFromToken)
        return Unauthorized(ReturnBaseHandler.Failed<bool>("You are not allowed to perform this action"));

      ReturnBase<bool> response = await Mediator.Send(command);
      return NewResult(response);
    }
  }
}
