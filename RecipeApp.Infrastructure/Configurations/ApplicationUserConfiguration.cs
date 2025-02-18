using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeApp.Domain.Entities.Identity;

namespace RecipeApp.Infrastructure.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Email)
                   .IsRequired()
                   .HasMaxLength(256);

            builder.Property(x => x.ProfilePictureURL)
                   .HasMaxLength(500);

            builder.Property(x => x.Bio)
                   .HasMaxLength(1000);

            //builder.Property(x => x.Country)
            //       .HasMaxLength(100);

            builder.Property(x => x.IsVerifiedChef)
                   .IsRequired()
                   .HasDefaultValue(false);

            // Relationship with Followers
            builder.HasMany(x => x.Followers)
                   .WithOne(x => x.Following)
                   .HasForeignKey(x => x.FollowingId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Following)
                   .WithOne(x => x.Follower)
                   .HasForeignKey(x => x.FollowerId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Relationship with Saved Recipes
            builder.HasMany(x => x.SavedRecipes)
                   .WithOne(x => x.User)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}