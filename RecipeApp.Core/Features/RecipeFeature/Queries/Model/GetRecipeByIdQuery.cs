using MediatR;
using RecipeApp.Core.Features.RecipeFeature.Queries.Response;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.RecipeFeature.Queries.Model
{
    public class GetRecipeByIdQuery : IRequest<ReturnBase<GetRecipeByIdResponse>>
    {
        public int Id { get; set; }
    }
}
