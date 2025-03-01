using MediatR;
using RecipeApp.Core.Features.CountryFeature.Queries.Response;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.CountryFeature.Queries.Model
{
  public record GetAllCountriesQuery(string? SearchTerm = null) : IRequest<ReturnBase<IQueryable<GetAllCountriesResponse>>>;
}