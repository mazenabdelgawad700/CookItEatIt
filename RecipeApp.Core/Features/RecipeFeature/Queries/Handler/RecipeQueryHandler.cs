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
        IRequestHandler<GetRecipesForUserQuery, ReturnBase<PaginatedResult<GetRecipesForUserResponse>>>,
        IRequestHandler<GetAllRecipesAsPaginatedQuery, ReturnBase<PaginatedResult<GetAllRecipesResponse>>>,
        IRequestHandler<GetTrindingNowRecipesAsPaginatedQuery, ReturnBase<PaginatedResult<GetTrindingNowRecipesResponse>>>,
        IRequestHandler<SearchRecipeQuery, ReturnBase<IQueryable<SearchRecipeResponse>>>,
        IRequestHandler<GetSavedRecipesAsPaginatedQuery, ReturnBase<PaginatedResult<GetSavedRecipesAsPaginatedResponse>>>
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
                var getRecipeByIdResult = _recipeService.GetRecipesForUserAsync(request.UserId);

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

        public async Task<ReturnBase<PaginatedResult<GetAllRecipesResponse>>> Handle(GetAllRecipesAsPaginatedQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var getRecipesResult = _recipeService.GetAllRecipes(request.CategoryId);

                if (!getRecipesResult.Succeeded)
                    return ReturnBaseHandler.Failed<PaginatedResult<GetAllRecipesResponse>>(getRecipesResult.Message);

                var mappedResult = await _mapper.ProjectTo<GetAllRecipesResponse>(getRecipesResult.Data).ToPaginatedListAsync(request.PageNumber, request.PageSize);

                return ReturnBaseHandler.Success(mappedResult, "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<PaginatedResult<GetAllRecipesResponse>>(ex.Message);
            }
        }
        public async Task<ReturnBase<PaginatedResult<GetTrindingNowRecipesResponse>>> Handle(GetTrindingNowRecipesAsPaginatedQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var getRecipesResult = _recipeService.GetTrindingNowRecipes();

                if (!getRecipesResult.Succeeded)
                    return ReturnBaseHandler.Failed<PaginatedResult<GetTrindingNowRecipesResponse>>(getRecipesResult.Message);

                var mappedResult = await _mapper.ProjectTo<GetTrindingNowRecipesResponse>(getRecipesResult.Data).ToPaginatedListAsync(request.PageNumber, request.PageSize);

                return ReturnBaseHandler.Success(mappedResult, "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<PaginatedResult<GetTrindingNowRecipesResponse>>(ex.Message);
            }
        }
        public async Task<ReturnBase<IQueryable<SearchRecipeResponse>>> Handle(SearchRecipeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var getRecipesResult = _recipeService.GetRecipesSearchResult(request.SearchQuery);

                if (!getRecipesResult.Succeeded)
                    return ReturnBaseHandler.Failed<IQueryable<SearchRecipeResponse>>(getRecipesResult.Message);

                var mappedResult = _mapper.ProjectTo<SearchRecipeResponse>(getRecipesResult.Data);

                return ReturnBaseHandler.Success(mappedResult, "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<IQueryable<SearchRecipeResponse>>(ex.Message);
            }
        }
        public async Task<ReturnBase<PaginatedResult<GetSavedRecipesAsPaginatedResponse>>> Handle(GetSavedRecipesAsPaginatedQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var getSavedRecipesResult = _recipeService.GetSavedRecipesForUserAsync(request.UserId, request.CategoryId ?? null);

                if (!getSavedRecipesResult.Succeeded)
                    return ReturnBaseHandler.Failed<PaginatedResult<GetSavedRecipesAsPaginatedResponse>>(getSavedRecipesResult.Message);

                var mappedResult = await _mapper.ProjectTo<GetSavedRecipesAsPaginatedResponse>(getSavedRecipesResult.Data).ToPaginatedListAsync(request.PageNumber, request.PageSize);

                return ReturnBaseHandler.Success(mappedResult, "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<PaginatedResult<GetSavedRecipesAsPaginatedResponse>>(ex.Message);
            }
        }
    }
}
