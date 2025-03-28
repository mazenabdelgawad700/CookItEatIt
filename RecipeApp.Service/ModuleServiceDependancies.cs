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
        services.AddTransient<IConfirmEmailService, ConfirmEmailSerivce>();
        services.AddTransient<IAuthenticationService, AuthenticationService>();
        services.AddTransient<IFileService, FileService>();
        services.AddTransient<IApplicationUserService, ApplicationUserService>();
        services.AddTransient<IPreferredDishService, PreferredDishService>();
        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<ISendPasswordChangeNotificationEmailService, SendPasswordChangeNotificationEmailService>();
        services.AddTransient<ICountryService, CountryService>();
        services.AddTransient<IUserPreferredDishesService, UserPreferredDishesService>();
        services.AddTransient<IUserPreferredCategoriesService, UserPreferredCategoriesService>();
        services.AddTransient<IUserPreferencesService, UserPreferencesService>();
        services.AddTransient<IRecipeService, RecipeService>();
        return services;
    }
}