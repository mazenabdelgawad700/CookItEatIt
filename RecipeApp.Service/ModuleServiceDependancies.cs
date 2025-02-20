using Microsoft.Extensions.DependencyInjection;
using RecipeApp.Service.Abstraction;
using RecipeApp.Service.Implementaion;

namespace RecipeApp.Service;

public static class ModuleServiceDependancies
{
    public static IServiceCollection AddServiceDependancies(this IServiceCollection services)
    {
        services.AddTransient<IApplicationUserService, ApplicationUserService>();
        services.AddTransient<ISendEmailService, SendEmailService>();
        return services;
    }
}