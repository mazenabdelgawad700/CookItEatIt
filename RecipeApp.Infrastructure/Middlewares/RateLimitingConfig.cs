using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;

namespace RecipeApp.Infrastructure.Middlewares
{
    public static class RateLimitingConfig
    {
        public static void AddCustomRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("PasswordChangePolicy", opt =>
                {
                    opt.Window = TimeSpan.FromHours(1);
                    opt.PermitLimit = 5;
                    opt.QueueLimit = 0;
                });
                options.AddFixedWindowLimiter("LogInPolicy", opt =>
                {
                    opt.Window = TimeSpan.FromMinutes(30);
                    opt.PermitLimit = 3;
                    opt.QueueLimit = 0;
                });
            });
        }
    }
}