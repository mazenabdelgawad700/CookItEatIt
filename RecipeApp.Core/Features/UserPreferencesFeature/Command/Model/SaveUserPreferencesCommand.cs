using MediatR;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.UserPreferencesFeature.Command.Model
{
  public record SaveUserPreferencesCommand : IRequest<ReturnBase<bool>>
  {
    public int UserId { get; set; }
    public bool IsVegetarian { get; set; }
    public bool AcceptNewDishNotification { get; set; }
    public int DefaultHungryHeads { get; set; }
  }
}