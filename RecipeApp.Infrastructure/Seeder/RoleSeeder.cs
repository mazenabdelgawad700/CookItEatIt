using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecipeApp.Domain.Entities.Identity;

namespace RecipeApp.Infrastructure.Seeder
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAsync(RoleManager<Role> _roleManager)
        {
            bool existingRoles = await _roleManager.Roles.AnyAsync();

            if (!existingRoles)
            {
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "Admin"
                });
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "User"
                });
            }

        }
    }
}