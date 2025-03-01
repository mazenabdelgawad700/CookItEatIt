using MediatR;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.ApplicationUserFeature.Command.Model
{
  public record AssignUserToCountryCommand : IRequest<ReturnBase<bool>>
  {
    public int UserId { get; set; }
    public int CountryId { get; set; }
  }
}