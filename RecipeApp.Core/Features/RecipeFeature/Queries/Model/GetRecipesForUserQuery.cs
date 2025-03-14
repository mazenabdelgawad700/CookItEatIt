using MediatR;
using RecipeApp.Core.Features.RecipeFeature.Queries.Response;
using RecipeApp.Core.Wrappers;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.RecipeFeature.Queries.Model
{
    public class GetRecipesForUserQuery : IRequest<ReturnBase<PaginatedResult<GetRecipesForUserResponse>>>
    {
        public int UserId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}