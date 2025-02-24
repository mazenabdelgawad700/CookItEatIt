using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RecipeApp.API.Base;
using RecipeApp.Core.Features.Authentication.Command.Models;
using RecipeApp.Shared.Bases;
using System.Net;

namespace RecipeApp.API.Controllers
{
    [Route("api/authentication/[action]")]
    public class AuthenticationController : AppControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
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

        [HttpPost]
        [Authorize]
        [EnableRateLimiting("PasswordChangePolicy")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            string? userIdFromToken = User.FindFirst("UserId")?.Value;

            ReturnBase<bool> res;

            if (userIdFromToken is null)
            {
                res = new()
                {
                    Data = false,
                    Succeeded = false,
                    Message = "Invalid Token",
                    StatusCode = HttpStatusCode.Unauthorized,
                    Errors = []
                };
                return Unauthorized(res);
            }

            if (command.Id.ToString() != userIdFromToken)
            {
                res = new()
                {
                    Data = false,
                    Succeeded = false,
                    Message = "You are not allowed",
                    StatusCode = HttpStatusCode.Forbidden,
                    Errors = []
                };
                return NewResult(res);
            }

            ReturnBase<bool> response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailCommand command)
        {
            ReturnBase<bool> response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            ReturnBase<string> response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordEmail([FromBody] ResetPasswordEmailCommand command)
        {
            ReturnBase<bool> response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            ReturnBase<bool> response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}