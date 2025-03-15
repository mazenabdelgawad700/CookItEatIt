using AutoMapper;
using RecipeApp.Core.Features.UserPreferencesFeature.Command.Model;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Core.Mapping.UserPreferencesMapping
{
    public class UserPreferencesMappingProfile : Profile
    {
        public UserPreferencesMappingProfile()
        {
            CreateMap<SaveUserPreferencesCommand, UserPreferences>();
        }
    }
}