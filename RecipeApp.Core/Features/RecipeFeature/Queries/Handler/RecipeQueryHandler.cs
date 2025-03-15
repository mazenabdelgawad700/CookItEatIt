using AutoMapper;
using MediatR;
using RecipeApp.Core.Features.RecipeFeature.Queries.Model;
using RecipeApp.Core.Features.RecipeFeature.Queries.Response;
using RecipeApp.Core.Wrappers;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.RecipeFeature.Queries.Handler
{
    public class RecipeQueryHandler : IRequestHandler<GetRecipeByIdQuery, ReturnBase<GetRecipeByIdResponse>>,
        IRequestHandler<GetRecipesForUserQuery, ReturnBase<PaginatedResult<GetRecipesForUserResponse>>>
    {
        private readonly IRecipeService _recipeService;
        private readonly IMapper _mapper;


        public RecipeQueryHandler(IRecipeService recipeService, IMapper mapper)
        {
            _recipeService = recipeService;
            _mapper = mapper;
        }


        public async Task<ReturnBase<GetRecipeByIdResponse>> Handle(GetRecipeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var getRecipeByIdResult = await _recipeService.GetRecipeByIdAsync(request.Id);


                if (!getRecipeByIdResult.Succeeded)
                    return ReturnBaseHandler.Failed<GetRecipeByIdResponse>(getRecipeByIdResult.Message);

                var mappedResult = _mapper.Map<GetRecipeByIdResponse>(getRecipeByIdResult.Data);

                var checkIfCurrentUserLikedTheRecipeResult = await _recipeService.IsCurrentUserLikedTheRecipeAsync(request.UserId, mappedResult.Id);

                if (checkIfCurrentUserLikedTheRecipeResult.Message.Equals("UserNotFound"))
                    return ReturnBaseHandler.Failed<GetRecipeByIdResponse>("Invalid Token");

                if (checkIfCurrentUserLikedTheRecipeResult.Succeeded)
                    mappedResult.IsCurrentUserLiked = true;

                return ReturnBaseHandler.Success(mappedResult, "");

            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<GetRecipeByIdResponse>(ex.Message);
            }
        }

        public async Task<ReturnBase<PaginatedResult<GetRecipesForUserResponse>>> Handle(GetRecipesForUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var getRecipeByIdResult = _recipeService.GetRecipesForUser(request.UserId);

                if (!getRecipeByIdResult.Succeeded)
                    return ReturnBaseHandler.Failed<PaginatedResult<GetRecipesForUserResponse>>(getRecipeByIdResult.Message);

                var mappedResult = await _mapper.ProjectTo<GetRecipesForUserResponse>(getRecipeByIdResult.Data).ToPaginatedListAsync(request.PageNumber, request.PageSize);

                return ReturnBaseHandler.Success(mappedResult, "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<PaginatedResult<GetRecipesForUserResponse>>(ex.Message);
            }
        }
    }
}
