using AutoMapper;
using MediatR;
using RecipeApp.Core.Features.VerifiedChefFeature.Query.Model;
using RecipeApp.Core.Features.VerifiedChefFeature.Query.Response;
using RecipeApp.Core.Wrappers;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.VerifiedChefFeature.Query.Handler
{
    public class VerifiedChefQueryHandler : IRequestHandler<VerifiedChefAsPaginatedQuery, ReturnBase<PaginatedResult<VerifiedChefAsPaginatedResponse>>>
    {
        private readonly IApplicationUserService _applicationUserService;
        private readonly IMapper _mapper;


        public VerifiedChefQueryHandler(IApplicationUserService applicationUserService, IMapper mapper)
        {
            _applicationUserService = applicationUserService;
            _mapper = mapper;
        }

        public async Task<ReturnBase<PaginatedResult<VerifiedChefAsPaginatedResponse>>> Handle(VerifiedChefAsPaginatedQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var getVerifiedChefsResult = _applicationUserService.VerifiedChefList();

                if (!getVerifiedChefsResult.Succeeded)
                    return ReturnBaseHandler.Failed<PaginatedResult<VerifiedChefAsPaginatedResponse>>(getVerifiedChefsResult.Message);

                var mappedResult = await _mapper.ProjectTo<VerifiedChefAsPaginatedResponse>(getVerifiedChefsResult.Data).ToPaginatedListAsync(request.PageNumber, request.PageSize);

                return ReturnBaseHandler.Success(mappedResult);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<PaginatedResult<VerifiedChefAsPaginatedResponse>>(ex.Message);
            }
        }
    }
}
