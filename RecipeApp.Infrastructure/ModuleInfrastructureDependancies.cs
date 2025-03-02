﻿using Microsoft.Extensions.DependencyInjection;
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

    services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));

    return services;
  }
}