using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecipeApp.Core;
using RecipeApp.Core.MiddelWare;
using RecipeApp.Infrastructure;
using RecipeApp.Infrastructure.Context;
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
        builder.Services.AddInfrastructureDependancies()
            .AddServiceDependancies()
            .AddCoreDependancies()
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


        var app = builder.Build();

        if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ErrorHandlerMiddleware>();

        app.UseHttpsRedirection();

        app.UseCors(CORS);
        app.UseAuthorization();
        app.UseAuthentication();


        app.MapControllers();

        app.Run();
    }
}
