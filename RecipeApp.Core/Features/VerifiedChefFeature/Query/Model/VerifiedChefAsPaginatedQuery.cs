using MediatR;
using RecipeApp.Core.Features.VerifiedChefFeature.Query.Response;
using RecipeApp.Core.Wrappers;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.VerifiedChefFeature.Query.Model
{
    public class VerifiedChefAsPaginatedQuery : IRequest<ReturnBase<PaginatedResult<VerifiedChefAsPaginatedResponse>>>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
