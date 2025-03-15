using MediatR;
using RecipeApp.Core.Features.RecipeFeature.Queries.Response;
using RecipeApp.Core.Wrappers;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.RecipeFeature.Queries.Model
{
    public class GetTrindingNowRecipesAsPaginatedQuery : IRequest<ReturnBase<PaginatedResult<GetTrindingNowRecipesResponse>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
