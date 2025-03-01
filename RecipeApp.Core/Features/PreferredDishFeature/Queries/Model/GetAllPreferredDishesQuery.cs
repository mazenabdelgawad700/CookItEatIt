using MediatR;
using RecipeApp.Core.Features.PreferredDishFeature.Queries.Response;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.PreferredDishFeature.Queries.Model
{
  public record GetAllPreferredDishesQuery() : IRequest<ReturnBase<IQueryable<GetAllPreferredDishesResponse>>>;
}