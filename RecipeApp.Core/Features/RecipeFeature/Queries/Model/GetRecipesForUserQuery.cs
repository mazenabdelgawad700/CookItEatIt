using MediatR;
using RecipeApp.Core.Features.RecipeFeature.Queries.Response;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.RecipeFeature.Queries.Model
{
    public class GetRecipesForUserQuery : IRequest<ReturnBase<IQueryable<GetRecipesForUserResponse>>>
    {
        public int UserId { get; set; }
    }
}