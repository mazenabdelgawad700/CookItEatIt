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
            builder.HasMany(x => x.Recipes)
                   .WithMany(x => x.Categories)
                   .UsingEntity<RecipeCategory>(
                       j => j.HasOne(rc => rc.Recipe)
                             .WithMany()
                             .HasForeignKey(rc => rc.RecipeId),
                       j => j.HasOne(rc => rc.Category)
                             .WithMany()
                             .HasForeignKey(rc => rc.CategoryId)
                   );

            // Many-to-many with ApplicationUser via UserPreferredCategory
            builder.HasMany(c => c.UserPreferredCategories)
                   .WithOne(upc => upc.Category)
                   .HasForeignKey(upc => upc.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}