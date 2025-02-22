using Microsoft.Extensions.DependencyInjection;
using RecipeApp.Service.Abstraction;
using RecipeApp.Service.Implementaion;

namespace RecipeApp.Service;

public static class ModuleServiceDependancies
{
    public static IServiceCollection AddServiceDependancies(this IServiceCollection services)
    {
        services.AddTransient<IProfilePictureService, ProfilePictureService>();
        services.AddTransient<ISendEmailService, SendEmailService>();
        services.AddTransient<IConfirmEmailSerivce, ConfirmEmailSerivce>();
        services.AddTransient<IAuthenticationService, AuthenticationService>();
        services.AddTransient<IFileService, FileService>();
        return services;
    }
}