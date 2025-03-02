﻿using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using RecipeApp.Core.Behaviors;
using System.Reflection;

namespace RecipeApp.Core;

public static class ModuleCoreDependencies
{
    public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
    {
        services.AddMediatR(
        cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly())
        );

        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddHttpContextAccessor();

        return services;
    }
}