using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Infrastructure.Configurations
{
    public class UserPreferredDishesConfiguration : IEntityTypeConfiguration<UserPreferredDishes>
    {
        public void Configure(EntityTypeBuilder<UserPreferredDishes> builder)
        {
            builder.HasKey(upd => new { upd.UserId, upd.PreferredDishId });

            // Relationships
            builder.HasOne(upd => upd.ApplicationUser)
                .WithMany(au => au.UserPreferredDishes)
                .HasForeignKey(upd => upd.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(upd => upd.PreferredDish)
                .WithMany(pd => pd.UserPreferredDishes)
                .HasForeignKey(upd => upd.PreferredDishId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}