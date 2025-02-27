using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using RecipeApp.Core.Features.Category.Queries.Model;
using RecipeApp.Core.Features.Category.Queries.Response;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.Category.Queries.Handler
{
    internal class CategoryQueryHandler : IRequestHandler<GetAllCategoriesQuery, ReturnBase<IQueryable<GetAllCategoriesResponse>>>
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;


        public CategoryQueryHandler(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public async Task<ReturnBase<IQueryable<GetAllCategoriesResponse>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var getCategoriesResult = _categoryService.GetAllCategories();

                if (getCategoriesResult is null || getCategoriesResult.Data is null)
                    return ReturnBaseHandler.Failed<IQueryable<GetAllCategoriesResponse>>();

                var mappedResult = getCategoriesResult.Data
                        .ProjectTo<GetAllCategoriesResponse>(_mapper.ConfigurationProvider);

                return ReturnBaseHandler.Success(mappedResult, "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<IQueryable<GetAllCategoriesResponse>>(ex.Message);
            }
        }
    }
}
