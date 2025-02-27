using AutoMapper;
using RecipeApp.Core.Features.ApplicationUserFeature.Query.Response;
using RecipeApp.Domain.Entities.Identity;

namespace RecipeApp.Core.Mapping.ApplicationUserMapping
{
    internal class GetApplicationUserByIdMapping : Profile
    {
        public GetApplicationUserByIdMapping()
        {
            CreateMap<ApplicationUser, GetApplicationUserProfileByIdResponse>()
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country!.Name));
        }
    }
}