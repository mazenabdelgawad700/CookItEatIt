using MediatR;
using RecipeApp.Core.Features.Category.Queries.Response;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.Category.Queries.Model
{
    public class GetAllCategoriesQuery : IRequest<ReturnBase<IQueryable<GetAllCategoriesResponse>>>
    {
    }
}