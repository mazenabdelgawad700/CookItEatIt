using Microsoft.AspNetCore.Mvc;
using RecipeApp.API.Base;
using RecipeApp.Core.Features.ApplicationUser.Command.Model;
using RecipeApp.Shared.Bases;

namespace RecipeApp.API.Controllers
{
    [Route("api/applicationuser/[action]")]
    public class ApplicationUserController : AppControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddApplicationUserCommand command)
        {
            ReturnBase<string> response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}