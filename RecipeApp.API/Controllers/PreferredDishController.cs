﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeApp.API.Base;
using RecipeApp.Core.Features.PreferredDishFeature.Command.Model;
using RecipeApp.Shared.Bases;

namespace RecipeApp.API.Controllers
{
  [Route("api/preferred-dish/[action]")]
  public class PreferredDishController : AppControllerBase
  {
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> CreateAsync([FromForm] AddPreferredDishCommand command)
    {
      ReturnBase<bool> response = await Mediator.Send(command);
      return NewResult(response);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdateAsync([FromForm] UpdatePreferredDishCommand command)
    {
      ReturnBase<bool> response = await Mediator.Send(command);
      return NewResult(response);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteAsync([FromBody] DeletePreferredDishCommand command)
    {
      ReturnBase<bool> response = await Mediator.Send(command);
      return NewResult(response);
    }
  }
}
