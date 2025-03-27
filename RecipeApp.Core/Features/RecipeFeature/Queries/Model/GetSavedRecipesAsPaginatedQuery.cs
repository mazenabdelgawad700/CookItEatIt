using MediatR;
using RecipeApp.Core.Features.RecipeFeature.Queries.Response;
using RecipeApp.Core.Wrappers;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.RecipeFeature.Queries.Model
{
    public class GetSavedRecipesAsPaginatedQuery : IRequest<ReturnBase<PaginatedResult<GetSavedRecipesAsPaginatedResponse>>>
    {
        public int? CategoryId { get; set; } // To Filter 
        public int UserId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
