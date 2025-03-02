using AutoMapper;
using MediatR;
using RecipeApp.Core.Features.UserPreferencesFeature.Command.Model;
using RecipeApp.Domain.Entities.Models;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.UserPreferencesFeature.Command.Handler
{
  public class UserPreferencesHandler :
      IRequestHandler<SaveUserPreferencesCommand, ReturnBase<bool>>
  {
    private readonly IUserPreferencesService _userPreferencesService;
    private readonly IMapper _mapper;

    public UserPreferencesHandler(IUserPreferencesService userPreferencesService, IMapper mapper)
    {
      _userPreferencesService = userPreferencesService;
      _mapper = mapper;
    }

    public async Task<ReturnBase<bool>> Handle(
        SaveUserPreferencesCommand request,
        CancellationToken cancellationToken)
    {
      try
      {
        var userPreferences = _mapper.Map<UserPreferences>(request);
        return await _userPreferencesService.SaveUserPreferencesAsync(userPreferences);
      }
      catch (Exception ex)
      {
        return ReturnBaseHandler.Failed<bool>(ex.Message);
      }
    }
  }
}