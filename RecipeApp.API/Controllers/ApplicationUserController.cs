using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeApp.API.Base;
using RecipeApp.Core.Features.ApplicationUserFeature.Query.Model;
using RecipeApp.Core.Features.ApplicationUserFeature.Query.Response;
using RecipeApp.Shared.Bases;

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
    }
}