using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using RecipeApp.Core;
using RecipeApp.Core.MiddelWare;
using RecipeApp.Infrastructure;
using RecipeApp.Infrastructure.Context;
using RecipeApp.Infrastructure.Middlewares;
using RecipeApp.Service;

namespace RecipeApp.API;

public class Program
{
    public static void Main(string[] args)
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

        builder.Services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromHours(1));

        builder.Services.AddCustomRateLimiting();


        var app = builder.Build();

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
