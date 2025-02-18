using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Infrastructure.Configurations
{
    public class UserPreferencesConfiguration : IEntityTypeConfiguration<UserPreferences>
    {
        public void Configure(EntityTypeBuilder<UserPreferences> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.IsVegetarian)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(p => p.AcceptNewDishNotification)
                   .IsRequired()
                   .HasDefaultValue(true);

            builder.Property(p => p.DefaultHungryHeads)
                   .IsRequired()
                   .HasDefaultValue(4);

            // Many-to-Many relationship with Category
            builder.HasMany(p => p.PreferredCategories)
                   .WithMany()
                   .UsingEntity<UserPreferredCategory>(
                       j => j
                           .HasOne(upc => upc.Category)
                           .WithMany()
                           .HasForeignKey(upc => upc.CategoryId),
                       j => j
                           .HasOne(upc => upc.UserPreferences)
                           .WithMany()
                           .HasForeignKey(upc => upc.UserPreferencesId),
                       j =>
                       {
                           j.HasKey(t => new { t.UserPreferencesId, t.CategoryId });
                           j.ToTable("UserPreferredCategories");
                       });
        }
    }

}
