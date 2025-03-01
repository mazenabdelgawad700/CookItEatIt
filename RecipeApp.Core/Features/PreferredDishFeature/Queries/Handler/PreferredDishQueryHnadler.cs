using AutoMapper;
using MediatR;
using RecipeApp.Core.Features.PreferredDishFeature.Queries.Model;
using RecipeApp.Core.Features.PreferredDishFeature.Queries.Response;
using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.PreferredDishFeature.Queries.Handler
{
  public class PreferredDishQueryHandler :
      IRequestHandler<GetAllPreferredDishesQuery, ReturnBase<IQueryable<GetAllPreferredDishesResponse>>>
  {
    private readonly IPreferredDishRepository _preferredDishRepository;
    private readonly IMapper _mapper;

    public PreferredDishQueryHandler(IPreferredDishRepository preferredDishRepository, IMapper mapper)
    {
      _preferredDishRepository = preferredDishRepository;
      _mapper = mapper;
    }

    public async Task<ReturnBase<IQueryable<GetAllPreferredDishesResponse>>> Handle(
        GetAllPreferredDishesQuery request,
        CancellationToken cancellationToken)
    {
      try
      {
        var dishes = _preferredDishRepository.GetTableNoTracking();

        if (!dishes.Succeeded)
          return ReturnBaseHandler.NotFound<IQueryable<GetAllPreferredDishesResponse>>(dishes.Message);

        var mappedDishes = _mapper.ProjectTo<GetAllPreferredDishesResponse>(dishes.Data);

        return ReturnBaseHandler.Success(mappedDishes, "Preferred dishes retrieved successfully");
      }
      catch (Exception ex)
      {
        return ReturnBaseHandler.Failed<IQueryable<GetAllPreferredDishesResponse>>(ex.Message);
      }
    }
  }
}
