using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeApp.Domain.Entities.Identity;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Infrastructure.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(256);

            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.UserName)
                   .IsRequired()
                   .HasMaxLength(256);

            builder.Property(u => u.ProfilePictureURL)
                   .HasMaxLength(500);

            builder.Property(u => u.PreferredTheme)
                   .HasDefaultValue(1);

            builder.Property(u => u.Bio)
                   .HasMaxLength(1000);

            builder.Property(u => u.IsVerifiedChef)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(u => u.RecipesCount)
                   .HasDefaultValue(0)
                   .IsRequired();

            builder.Property(u => u.FollowersCount)
                   .HasDefaultValue(0)
                   .IsRequired();

            builder.Property(u => u.FollowingCount)
                   .HasDefaultValue(0)
                   .IsRequired();

            // Relationship with Followers
            builder.HasMany(u => u.Followers)
                   .WithOne(u => u.Following)
                   .HasForeignKey(u => u.FollowingId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.Following)
                   .WithOne(u => u.Follower)
                   .HasForeignKey(u => u.FollowerId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Relationship with Saved Recipes
            builder.HasMany(u => u.SavedRecipes)
                   .WithOne(sr => sr.User)
                   .HasForeignKey(sr => sr.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.Preferences)
                   .WithOne(p => p.User)
                   .HasForeignKey<UserPreferences>(p => p.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}