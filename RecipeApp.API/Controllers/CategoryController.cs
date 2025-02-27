using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeApp.API.Base;
using RecipeApp.Core.Features.Category.Command.Model;
using RecipeApp.Core.Features.Category.Queries.Model;
using RecipeApp.Core.Features.Category.Queries.Response;
using RecipeApp.Shared.Bases;

namespace RecipeApp.API.Controllers
{
    [Route("api/category/[action]")]
    public class CategoryController : AppControllerBase
    {
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AddCategoryAsync([FromBody] AddCategoryCommand command)
        {
            ReturnBase<bool> response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateCategoryAsync([FromBody] UpdateCategoryCommand command)
        {
            ReturnBase<bool> response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetAllCategoriesAsync([FromBody] GetAllCategoriesQuery query)
        {
            ReturnBase<IQueryable<GetAllCategoriesResponse>> response = await Mediator.Send(query);
            return NewResult(response);
        }
    }
}
