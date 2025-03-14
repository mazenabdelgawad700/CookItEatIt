using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Infrastructure.Configurations
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            // Relationship with Recipes (Many-to-Many)
            builder.HasMany(c => c.Recipes)
                   .WithMany(r => r.Categories)
                   .UsingEntity<RecipeCategory>(
                       j => j.HasOne(rc => rc.Recipe)
                             .WithMany(r => r.RecipeCategories)
                             .HasForeignKey(rc => rc.RecipeId),
                       j => j.HasOne(rc => rc.Category)
                             .WithMany(c => c.RecipeCategories)
                             .HasForeignKey(rc => rc.CategoryId),
                       j =>
                       {
                           j.HasKey(rc => new { rc.RecipeId, rc.CategoryId });
                       }
                   );

            // Many-to-many with ApplicationUser via UserPreferredCategory
            builder.HasMany(c => c.UserPreferredCategories)
                   .WithOne(upc => upc.Category)
                   .HasForeignKey(upc => upc.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}