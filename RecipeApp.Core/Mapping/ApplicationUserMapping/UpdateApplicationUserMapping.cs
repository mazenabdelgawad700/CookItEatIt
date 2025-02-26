using AutoMapper;
using RecipeApp.Core.Features.ApplicationUserFeature.Command.Model;
using RecipeApp.Domain.Entities.Identity;

namespace RecipeApp.Core.Mapping.ApplicationUserMapping
{
    public class UpdateApplicationUserMapping : Profile
    {
        public UpdateApplicationUserMapping()
        {
            CreateMap<UpdateApplicationUserCommand, ApplicationUser>();
        }
    }
}