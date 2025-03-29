using MediatR;
using RecipeApp.Shared.Bases;
using RecipeApp.Shared.SharedResponse;

namespace RecipeApp.Core.Features.ApplicationUserSettingsFeature.Query.Model
{
    public class GetUserSettingsQuery : IRequest<ReturnBase<GetUserSettingsResponse>>
    {
        public int Id { get; set; }
    }
}
