using MediatR;
using RecipeApp.Core.Features.ApplicationUserSettingsFeature.Query.Model;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;
using RecipeApp.Shared.SharedResponse;

namespace RecipeApp.Core.Features.ApplicationUserSettingsFeature.Query.Handler
{
    public class ApplicationUserSettingsQueryHandler : IRequestHandler<GetUserSettingsQuery, ReturnBase<GetUserSettingsResponse>>
    {
        private readonly IApplicationUserService _applicationUserService;

        public ApplicationUserSettingsQueryHandler(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }



        public async Task<ReturnBase<GetUserSettingsResponse>> Handle(GetUserSettingsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var getUserSettingsResult = await _applicationUserService.GetApplicationUserSettingsAsync(request.Id);

                if (!getUserSettingsResult.Succeeded)
                    return ReturnBaseHandler.Failed<GetUserSettingsResponse>(getUserSettingsResult.Message);

                return ReturnBaseHandler.Success(getUserSettingsResult.Data, "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<GetUserSettingsResponse>(ex.Message);
            }
        }
    }
}
