using Microsoft.AspNetCore.Mvc;
using RecipeApp.API.Base;
using RecipeApp.Core.Features.Authentication.Command.Models;
using RecipeApp.Shared.Bases;

namespace RecipeApp.API.Controllers
{
    [Route("api/authentication/[action]")]
    public class AuthenticationController : AppControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] AddApplicationUserCommand command)
        {
            ReturnBase<string> response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> LogIn([FromBody] LoginCommand command)
        {
            ReturnBase<string> response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailCommand command)
        {
            ReturnBase<bool> response = await Mediator.Send(command);
            return NewResult(response);
        }

    }
}