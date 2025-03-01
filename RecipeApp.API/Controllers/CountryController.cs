using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecipeApp.API.Base;
using RecipeApp.Core.Features.CountryFeature.Queries.Model;
using RecipeApp.Core.Features.CountryFeature.Queries.Response;
using RecipeApp.Shared.Bases;

namespace RecipeApp.API.Controllers
{
  [Route("api/country/[action]")]
  public class CountryController : AppControllerBase
  {
    [HttpPost]
    public async Task<IActionResult> GetAllAsync([FromBody] GetAllCountriesQuery query)
    {
      ReturnBase<IQueryable<GetAllCountriesResponse>> response = await Mediator.Send(query);
      return NewResult(response);
    }
  }
}
