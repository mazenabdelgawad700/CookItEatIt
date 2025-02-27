using AutoMapper;
using MediatR;
using RecipeApp.Core.Features.Category.Command.Model;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.Category.Command.Handler
{
    internal class CategoryHandler : IRequestHandler<AddCategoryCommand, ReturnBase<bool>>,
        IRequestHandler<UpdateCategoryCommand, ReturnBase<bool>>
    {

        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryHandler(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }
        public async Task<ReturnBase<bool>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var category = _mapper.Map<Domain.Entities.Models.Category>(request);

                ReturnBase<bool> addCategoryResult = await _categoryService.AddCategoryAsync(category);

                if (addCategoryResult.Succeeded)
                    return ReturnBaseHandler.Success(true, addCategoryResult.Message);

                return ReturnBaseHandler.BadRequest<bool>(addCategoryResult.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }

        public async Task<ReturnBase<bool>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var category = _mapper.Map<Domain.Entities.Models.Category>(request);

                ReturnBase<bool> updateCategoryResult = await _categoryService.UpdateCategoryAsync(category);

                if (updateCategoryResult.Succeeded)
                    return ReturnBaseHandler.Success(true, updateCategoryResult.Message);

                return ReturnBaseHandler.BadRequest<bool>(updateCategoryResult.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
    }
}
