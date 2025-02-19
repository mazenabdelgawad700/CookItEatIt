using AutoMapper;
using RecipeApp.Core.Features.ApplicationUser.Command.Model;
using RecipeApp.Domain.Entities.Identity;

namespace RecipeApp.Core.Mapping.ApplicationUserMApping
{
    public class AddApplicationUserMapping : Profile
    {
        public AddApplicationUserMapping()
        {
            CreateMap<AddApplicationUserCommand, ApplicationUser>();
        }
    }
}