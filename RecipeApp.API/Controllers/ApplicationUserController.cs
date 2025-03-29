using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeApp.API.Base;
using RecipeApp.Core.Features.ApplicationUserFeature.Command.Model;
using RecipeApp.Core.Features.ApplicationUserFeature.Query.Model;
using RecipeApp.Core.Features.ApplicationUserFeature.Query.Response;
using RecipeApp.Core.Features.ApplicationUserSettingsFeature.Query.Model;
using RecipeApp.Core.Features.VerifiedChefFeature.Query.Model;
using RecipeApp.Shared.Bases;
using System.Net;

namespace RecipeApp.API.Controllers
{
    [Route("api/applicationuser/[action]")]
    public class ApplicationUserController : AppControllerBase
    {
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetUserProfileByIdAsync([FromBody] GetApplicationUserProfileByIdQuery query)
        {
            ReturnBase<GetApplicationUserProfileByIdResponse> response = await Mediator.Send(query);
            return NewResult(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetUserSettingsAsync([FromBody] GetUserSettingsQuery query)
        {
            string? userIdFromToken = User.FindFirst("UserId")?.Value;

            if (userIdFromToken is null)
                return Unauthorized(ReturnBaseHandler.Failed<bool>("Invalid Token"));

            if (query.Id.ToString() != userIdFromToken)
                return Unauthorized(ReturnBaseHandler.Failed<bool>("You are not allowed to perform this action"));

            var response = await Mediator.Send(query);
            return NewResult(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateUserProfileAsync([FromBody] UpdateApplicationUserCommand command)
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
        public async Task<IActionResult> AssignToCountryAsync([FromBody] AssignUserToCountryCommand command)
        {
            string? userIdFromToken = User.FindFirst("UserId")?.Value;

            if (userIdFromToken is null)
                return Unauthorized(ReturnBaseHandler.Failed<bool>("Invalid Token"));

            if (command.UserId.ToString() != userIdFromToken)
                return Unauthorized(ReturnBaseHandler.Failed<bool>("You are not allowed to perform this action"));

            ReturnBase<bool> response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ToggleFollowingAsync([FromBody] ToggleFollowUserCommand command)
        {
            string? userIdFromToken = User.FindFirst("UserId")?.Value;

            if (userIdFromToken is null)
                return Unauthorized(ReturnBaseHandler.Failed<bool>("Invalid Token"));

            if (command.FollowerId.ToString() != userIdFromToken)
                return Unauthorized(ReturnBaseHandler.Failed<bool>("You are not allowed to perform this action"));

            ReturnBase<bool> response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetVerifiedChefsAsync([FromBody] VerifiedChefAsPaginatedQuery query)
        {
            var response = await Mediator.Send(query);
            return NewResult(response);
        }
    }
}