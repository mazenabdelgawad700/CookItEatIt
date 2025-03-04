using Microsoft.AspNetCore.Components;
using RecipeApp.API.Base;

namespace RecipeApp.API.Controllers
{
  [Route("api/recipe/[action]")]
  public class RecipeController : AppControllerBase
  {
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateAsync([FromBody] CreateRecipeCommand command)
    {
      ReturnBase<bool> response = await Mediator.Send(command);
      return NewResult(response);
    }
  }
}