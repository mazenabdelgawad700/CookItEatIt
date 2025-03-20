using MediatR;
using RecipeApp.Core.Features.RecipeFeature.Queries.Response;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.RecipeFeature.Queries.Model
{
    public class SearchRecipeQuery : IRequest<ReturnBase<IQueryable<SearchRecipeResponse>>>
    {
        public string? SearchQuery { get; set; }
    }
}