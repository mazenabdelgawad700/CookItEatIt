using AutoMapper;
using RecipeApp.Core.Features.ApplicationUserFeature.Command.Model;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Core.Mapping.UserFollowerMapping
{
    public class ToggleFollowUserMappingProfile : Profile
    {
        public ToggleFollowUserMappingProfile()
        {
            CreateMap<ToggleFollowUserCommand, UserFollower>();
        }
    }
}
