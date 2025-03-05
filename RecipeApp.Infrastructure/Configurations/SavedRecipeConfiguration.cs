using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Infrastructure.Configurations
{
    public class SavedRecipeConfiguration : IEntityTypeConfiguration<SavedRecipe>
    {
        public void Configure(EntityTypeBuilder<SavedRecipe> builder)
        {
            builder.HasKey(e => new { e.UserId, e.RecipeId });

            // One-to-many: Recipe to SavedRecipe
            builder.HasOne(sr => sr.Recipe)
                   .WithMany(r => r.SavedRecipes)
                   .HasForeignKey(sr => sr.RecipeId)
                   .OnDelete(DeleteBehavior.Cascade); // Optional: Cascade delete if desired

            // One-to-many: ApplicationUser to SavedRecipe
            builder.HasOne(sr => sr.User)
                   .WithMany(u => u.SavedRecipes)
                   .HasForeignKey(sr => sr.UserId)
                   .OnDelete(DeleteBehavior.Cascade); // Optional: Cascade delete if desired

            // Configure SavedAt default value
            builder.Property(sr => sr.SavedAt)
                   .HasDefaultValueSql("GETDATE()");
        }
    }
}
