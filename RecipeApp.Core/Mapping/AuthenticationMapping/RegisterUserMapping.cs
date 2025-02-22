using AutoMapper;
using RecipeApp.Core.Features.Authentication.Command.Models;
using RecipeApp.Domain.Entities.Identity;

namespace RecipeApp.Core.Mapping.AuthenticationMapping
{
    public class RegisterUserMapping : Profile
    {
        public RegisterUserMapping()
        {
            CreateMap<AddApplicationUserCommand, ApplicationUser>();
        }
    }
}