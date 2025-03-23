using Microsoft.Extensions.DependencyInjection;
using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Infrastructure.InfrastructureBases;
using RecipeApp.Infrastructure.Repositories;

namespace RecipeApp.Infrastructure;

public static class ModuleInfrastructureDependencies
{
    public static IServiceCollection AddInfrastructureDependencies
        (this IServiceCollection services)
    {
        services.AddTransient<IApplicationUserRepository, ApplicationUserRepository>();
        services.AddTransient<ICategoryRepository, CategoryRepository>();
        services.AddTransient<IPreferredDishRepository, PreferredDishRepository>();
        services.AddTransient<ICountryRepository, CountryRepository>();
        services.AddTransient<IUserPreferredDishesRepository, UserPreferredDishesRepository>();
        services.AddTransient<IUserPreferredCategoriesRepository, UserPreferredCategoriesRepository>();
        services.AddTransient<IUserPreferencesRepository, UserPreferencesRepository>();
        services.AddTransient<IRecipeRepository, RecipeRepository>();
        services.AddTransient<IIngredientRepository, IngredientRepository>();
        services.AddTransient<IInstructionRepository, InstructionRepository>();
        services.AddTransient<IRecipeCategoryRepository, RecipeCategoryRepository>();
        services.AddTransient<IRecipeLikeRepository, RecipeLikeRepository>();
        services.AddTransient<ISavedRecipeRepository, SavedRecipeRepository>();

        services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));

        return services;
    }
}