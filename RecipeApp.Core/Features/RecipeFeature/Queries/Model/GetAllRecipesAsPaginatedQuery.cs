using MediatR;
using RecipeApp.Core.Features.RecipeFeature.Queries.Response;
using RecipeApp.Core.Wrappers;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.RecipeFeature.Queries.Model
{
    public class GetAllRecipesAsPaginatedQuery : IRequest<ReturnBase<PaginatedResult<GetAllRecipesResponse>>>
    {
        public int? CategoryId { get; set; } // To Filter 
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
