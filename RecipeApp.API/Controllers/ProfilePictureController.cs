using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeApp.API.Base;
using RecipeApp.Core.Features.ApplicationUser.Command.Model;
using RecipeApp.Shared.Bases;
using System.Net;

namespace RecipeApp.API.Controllers
{
    [Route("api/profilepicture/[action]")]
    public class ProfilePictureController : AppControllerBase
    {
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Upload([FromForm] AddProfilePictureCommand command)
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete([FromBody] DeleteProfilePictureCommand command)
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update([FromForm] UpdateProfilePictureCommand command)
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
    }
}