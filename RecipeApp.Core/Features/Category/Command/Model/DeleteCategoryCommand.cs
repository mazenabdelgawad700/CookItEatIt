using MediatR;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.Category.Command.Model
{
  public record DeleteCategoryCommand(int Id) : IRequest<ReturnBase<bool>>;
}