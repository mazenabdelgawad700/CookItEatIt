using AutoMapper;
using MediatR;
using RecipeApp.Core.Features.ApplicationUserFeature.Query.Model;
using RecipeApp.Core.Features.ApplicationUserFeature.Query.Response;
using RecipeApp.Domain.Entities.Identity;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.ApplicationUserFeature.Query.Handler
{
    internal class ApplicationUserQueryHandler : IRequestHandler<GetApplicationUserProfileByIdQuery, ReturnBase<GetApplicationUserProfileByIdResponse>>
    {

        private readonly IApplicationUserService _applicationUserService;
        private readonly IMapper _mapper;


        public ApplicationUserQueryHandler(IApplicationUserService applicationUserService, IMapper mapper)
        {
            _applicationUserService = applicationUserService;
            _mapper = mapper;
        }

        public async Task<ReturnBase<GetApplicationUserProfileByIdResponse>> Handle(GetApplicationUserProfileByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                ReturnBase<ApplicationUser> result = await _applicationUserService.GetApplicationUserProfileByIdAsync(request.Id);
                if (!result.Succeeded)
                    return ReturnBaseHandler.Failed<GetApplicationUserProfileByIdResponse>(result.Message);

                var mappedResult = _mapper.Map<ApplicationUser, GetApplicationUserProfileByIdResponse>(result.Data);

                return ReturnBaseHandler.Success(mappedResult, "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<GetApplicationUserProfileByIdResponse>(ex.Message);
            }
        }
    }
}