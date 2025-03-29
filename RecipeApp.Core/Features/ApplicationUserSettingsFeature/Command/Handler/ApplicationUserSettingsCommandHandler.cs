using MediatR;
using RecipeApp.Core.Features.ApplicationUserSettingsFeature.Command.Model;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.ApplicationUserSettingsFeature.Command.Handler
{
    public class ApplicationUserSettingsCommandHandler : IRequestHandler<UpdateApplicationUserSettingsCommand, ReturnBase<bool>>
    {
        private readonly IApplicationUserService _applicationUserService;

        public ApplicationUserSettingsCommandHandler(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }

        public async Task<ReturnBase<bool>> Handle(UpdateApplicationUserSettingsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var updateUserSettingsResult = await _applicationUserService.UpdateApplicationUserSettingsAsync(request);

                if (!updateUserSettingsResult.Succeeded)
                    return ReturnBaseHandler.Failed<bool>(updateUserSettingsResult.Message);

                return ReturnBaseHandler.Success(true, "Settings Updated Successfully");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
    }
}
