using MediatR;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.UserPreferredDishFeature.Command.Model
{
  public record SaveUserPreferredDishesCommand : IRequest<ReturnBase<bool>>
  {
    public int UserId { get; set; }
    public List<int> PreferredDishIds { get; set; } = new List<int>();
  }
}