using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using RecipeApp.Core;
using RecipeApp.Core.MiddelWare;
using RecipeApp.Domain.Entities.Identity;
using RecipeApp.Infrastructure;
using RecipeApp.Infrastructure.Context;
using RecipeApp.Infrastructure.Middlewares;
using RecipeApp.Infrastructure.Seeder;
using RecipeApp.Service;

namespace RecipeApp.API;

public class Program
{
  public static async Task Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);


    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddDbContext<AppDbContext>(
        options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("constr"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
            )
    );

    #region Add Services To Dependency Injection
    builder.Services.AddInfrastructureDependencies()
        .AddServiceDependencies()
        .AddCoreDependencies()
        .AddServiceRegisteration(builder.Configuration);
    #endregion

    #region AllowCors
    string CORS = "_cors";
    builder.Services.AddCors(options =>
    {
      options.AddPolicy(name: CORS, policy =>
          {
          policy.AllowAnyHeader();
          policy.AllowAnyMethod();
          policy.AllowAnyOrigin();
        });
    });
    #endregion

    builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
    {
      options.TokenLifespan = TimeSpan.FromMinutes(30);
    });

    builder.Services.AddCustomRateLimiting();

    builder.Services.AddAuthorizationBuilder()
        .AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"))
        .AddPolicy("UserOnly", policy => policy.RequireRole("User"));

    var app = builder.Build();

    using (IServiceScope scope = app.Services.CreateScope())
    {
      RoleManager<Role> roleManager =
          scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

      await RoleSeeder.SeedRolesAsync(roleManager);
    }

    if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }

    app.UseMiddleware<ErrorHandlerMiddleware>();

    app.UseStaticFiles(new StaticFileOptions
    {
      FileProvider = new PhysicalFileProvider(
             Path.Combine(builder.Configuration.GetSection("ProfilePicturesPath").Value!,
             builder.Configuration.GetSection("ProfilePicturesFolderName").Value!)
        ),
      RequestPath = "/Resources"
    });

    app.UseHttpsRedirection();

    app.UseCors(CORS);

    app.UseRateLimiter();

    app.UseAuthentication();
    app.UseAuthorization();


    app.MapControllers();

    app.Run();
  }
}
