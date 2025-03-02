using MediatR;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.UserPreferredCategoryFeature.Command.Model
{
  public record SaveUserPreferredCategoriesCommand : IRequest<ReturnBase<bool>>
  {
    public int UserId { get; set; }
    public List<int> CategoryIds { get; set; } = new List<int>();
  }
}