using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Infrastructure.Configurations
{
    internal class UserPreferredCategoryConfiguration : IEntityTypeConfiguration<UserPreferredCategory>
    {
        public void Configure(EntityTypeBuilder<UserPreferredCategory> builder)
        {
            builder.HasKey(e => new { e.UserId, e.CategoryId });

            // Relationships
            builder.HasOne(upc => upc.ApplicationUser)
                   .WithMany(au => au.UserPreferredCategories)
                   .HasForeignKey(upc => upc.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(upc => upc.Category)
                   .WithMany(au => au.UserPreferredCategories)
                   .HasForeignKey(upc => upc.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}