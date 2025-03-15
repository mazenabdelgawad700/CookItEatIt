using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeApp.API.Base;
using RecipeApp.Core.Features.RecipeFeature.Command.Model;
using RecipeApp.Core.Features.RecipeFeature.Queries.Model;
using RecipeApp.Shared.Bases;

namespace RecipeApp.API.Controllers
{
    [Route("api/recipe/[action]")]
    public class RecipeController : AppControllerBase
    {
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAsync([FromBody] CreateRecipeCommand command)
        {
            string? userIdFromToken = User.FindFirst("UserId")?.Value;

            if (userIdFromToken is null)
                return Unauthorized(ReturnBaseHandler.Failed<bool>("Invalid Token"));

            if (command.UserId.ToString() != userIdFromToken)
                return Unauthorized(ReturnBaseHandler.Failed<bool>("You are not allowed to perform this action"));

            ReturnBase<int> response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddImageAsync([FromForm] AddRecipeImageCommand command)
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
        public async Task<IActionResult> GetByIdAsync([FromBody] GetRecipeByIdQuery query)
        {
            var response = await Mediator.Send(query);
            return NewResult(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateRecipeCommand command)
        {
            string? userIdFromToken = User.FindFirst("UserId")?.Value;

            if (userIdFromToken is null)
                return Unauthorized(ReturnBaseHandler.Failed<bool>("Invalid Token"));

            if (command.UserId.ToString() != userIdFromToken)
                return Unauthorized(ReturnBaseHandler.Failed<bool>("You are not allowed to perform this action"));

            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateImageAsync([FromForm] UpdateRecipeImageCommand command)
        {
            string? userIdFromToken = User.FindFirst("UserId")?.Value;

            if (userIdFromToken is null)
                return Unauthorized(ReturnBaseHandler.Failed<bool>("Invalid Token"));

            if (command.UserId.ToString() != userIdFromToken)
                return Unauthorized(ReturnBaseHandler.Failed<bool>("You are not allowed to perform this action"));

            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteAsync([FromForm] DeleteRecipeCommand command)
        {
            string? userIdFromToken = User.FindFirst("UserId")?.Value;

            if (userIdFromToken is null)
                return Unauthorized(ReturnBaseHandler.Failed<bool>("Invalid Token"));

            if (command.UserId.ToString() != userIdFromToken)
                return Unauthorized(ReturnBaseHandler.Failed<bool>("You are not allowed to perform this action"));

            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetRecipesForUserAsync([FromBody] GetRecipesForUserQuery query)
        {
            string? userIdFromToken = User.FindFirst("UserId")?.Value;

            if (userIdFromToken is null)
                return Unauthorized(ReturnBaseHandler.Failed<bool>("Invalid Token"));

            if (query.UserId.ToString() != userIdFromToken)
                return Unauthorized(ReturnBaseHandler.Failed<bool>("You are not allowed to perform this action"));

            var response = await Mediator.Send(query);
            return NewResult(response);
        }
    }
}