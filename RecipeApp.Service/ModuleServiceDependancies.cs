using Microsoft.Extensions.DependencyInjection;
using RecipeApp.Service.Abstraction;
using RecipeApp.Service.Implementation;

namespace RecipeApp.Service;

public static class ModuleServiceDependencies
{
    public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
    {
        services.AddTransient<IProfilePictureService, ProfilePictureService>();
        services.AddTransient<ISendEmailService, SendEmailService>();
        services.AddTransient<IConfirmEmailSerivce, ConfirmEmailSerivce>();
        services.AddTransient<IAuthenticationService, AuthenticationService>();
        services.AddTransient<IFileService, FileService>();
        services.AddTransient<ISendPasswordChangeNotificationEmailService, SendPasswordChangeNotificationEmailService>();
        return services;
    }
}