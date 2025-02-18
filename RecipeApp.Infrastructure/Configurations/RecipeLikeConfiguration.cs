using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Infrastructure.Configurations
{
    public class RecipeLikeConfiguration : IEntityTypeConfiguration<RecipeLike>
    {
        public void Configure(EntityTypeBuilder<RecipeLike> builder)
        {
            builder.HasKey(x => new { x.UserId, x.RecipeId });

            builder.Property(x => x.LikedAt)
                   .IsRequired();

            builder.HasOne(x => x.User)
                   .WithMany(x => x.LikedRecipes)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Recipe)
                   .WithMany(x => x.Likes)
                   .HasForeignKey(x => x.RecipeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}