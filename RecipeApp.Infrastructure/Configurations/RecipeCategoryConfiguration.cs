using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Infrastructure.Configurations
{
    internal class RecipeCategoryConfiguration : IEntityTypeConfiguration<RecipeCategory>
    {
        public void Configure(EntityTypeBuilder<RecipeCategory> builder)
        {
            builder.HasKey(x => new { x.RecipeId, x.CategoryId });

            builder.HasOne(x => x.Recipe)
                   .WithMany(x => x.RecipeCategories)
                   .HasForeignKey(x => x.RecipeId);

            builder.HasOne(x => x.Category)
                   .WithMany(x => x.RecipeCategories)
                   .HasForeignKey(x => x.CategoryId);
        }
    }
}
