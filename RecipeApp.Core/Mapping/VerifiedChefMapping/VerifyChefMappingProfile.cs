using AutoMapper;
using RecipeApp.Core.Features.VerifiedChefFeature.Query.Response;
using RecipeApp.Domain.Entities.Identity;

namespace RecipeApp.Core.Mapping.VerifiedChefMapping
{
    public class VerifyChefMappingProfile : Profile
    {
        public VerifyChefMappingProfile()
        {
            CreateMap<ApplicationUser, VerifiedChefAsPaginatedResponse>();
        }
    }
}
