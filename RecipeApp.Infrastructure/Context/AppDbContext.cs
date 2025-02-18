using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipeApp.Domain.Entities.Identity;
using RecipeApp.Domain.Entities.Models;
using System.Reflection;

namespace RecipeApp.Infrastructure.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, Role, int, IdentityUserClaim<int>, IdentityUserRole<int>, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Recipe> Recipe { get; set; }
        public DbSet<SavedRecipe> SavedRecipe { get; set; }
        public DbSet<UserFollower> UserFollower { get; set; }
        public DbSet<RecipeLike> RecipeLike { get; set; }
        public DbSet<RecipeCategory> RecipeCategory { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Instruction> Instruction { get; set; }
        public DbSet<Ingredient> Ingredient { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<ChefRecipeRequest> ChefRecipeRequest { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}