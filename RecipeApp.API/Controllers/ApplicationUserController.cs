﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeApp.API.Base;
using RecipeApp.Core.Features.ApplicationUserFeature.Command.Model;
using RecipeApp.Core.Features.ApplicationUserFeature.Query.Model;
using RecipeApp.Core.Features.ApplicationUserFeature.Query.Response;
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
  }
}