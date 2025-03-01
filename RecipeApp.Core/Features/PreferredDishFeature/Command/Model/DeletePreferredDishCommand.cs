using MediatR;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.PreferredDishFeature.Command.Model
{
  public record DeletePreferredDishCommand(int Id) : IRequest<ReturnBase<bool>>;
}