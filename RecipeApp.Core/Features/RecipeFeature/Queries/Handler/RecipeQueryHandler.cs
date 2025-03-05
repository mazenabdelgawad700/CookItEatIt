using AutoMapper;
using MediatR;
using RecipeApp.Core.Features.RecipeFeature.Queries.Model;
using RecipeApp.Core.Features.RecipeFeature.Queries.Response;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.RecipeFeature.Queries.Handler
{
    public class RecipeQueryHandler : IRequestHandler<GetRecipeByIdQuery, ReturnBase<GetRecipeByIdResponse>>
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
    }
}
