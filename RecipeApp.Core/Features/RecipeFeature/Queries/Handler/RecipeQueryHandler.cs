using AutoMapper;
using MediatR;
using RecipeApp.Core.Features.RecipeFeature.Queries.Model;
using RecipeApp.Core.Features.RecipeFeature.Queries.Response;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.RecipeFeature.Queries.Handler
{
    public class RecipeQueryHandler : IRequestHandler<GetRecipeByIdQuery, ReturnBase<GetRecipeByIdResponse>>,
        IRequestHandler<GetRecipesForUserQuery, ReturnBase<IQueryable<GetRecipesForUserResponse>>>
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

                return ReturnBaseHandler.Success(mappedResult, "");

            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<GetRecipeByIdResponse>(ex.Message);
            }
        }

        public async Task<ReturnBase<IQueryable<GetRecipesForUserResponse>>> Handle(GetRecipesForUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var getRecipeByIdResult = _recipeService.GetRecipesForUser(request.UserId);

                if (!getRecipeByIdResult.Succeeded)
                    return ReturnBaseHandler.Failed<IQueryable<GetRecipesForUserResponse>>(getRecipeByIdResult.Message);

                var mappedResult = _mapper.ProjectTo<GetRecipesForUserResponse>(getRecipeByIdResult.Data);

                return ReturnBaseHandler.Success(mappedResult, "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<IQueryable<GetRecipesForUserResponse>>(ex.Message);
            }
        }
    }
}
