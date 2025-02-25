using MediatR;
using RecipeApp.Core.Features.ApplicationUserFeature.Query.Response;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.ApplicationUserFeature.Query.Model
{
    public class GetApplicationUserProfileByIdQuery : IRequest<ReturnBase<GetApplicationUserProfileByIdResponse>>
    {
        public int Id { get; set; }
    }
}