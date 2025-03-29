using AutoMapper;
using RecipeApp.Core.Features.ApplicationUserSettingsFeature.Query.Model;
using RecipeApp.Domain.Entities.Identity;

namespace RecipeApp.Core.Mapping.ApplicationUserSettingsMapping
{
    public class ApplicationUserSettingsMappingProfile : Profile
    {
        public ApplicationUserSettingsMappingProfile()
        {
            CreateMap<ApplicationUser, GetUserSettingsQuery>();
        }
    }
}
