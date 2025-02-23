using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeApp.Domain.Entities.Identity;

namespace RecipeApp.Infrastructure.Configurations
{
    internal class ApplicationUserRefreshTokenConfiguration : IEntityTypeConfiguration<ApplicationUserRefreshToken>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserRefreshToken> builder)
        {
            builder.HasKey(rt => rt.Id);

            builder.HasOne(rt => rt.User)
                   .WithMany(u => u.ApplicationUserRefreshTokens)
                   .HasForeignKey(rt => rt.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(rt => rt.RefreshToken).IsUnique();

            builder.Property(rt => rt.RefreshToken)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(rt => rt.JwtId)
                   .IsRequired()
                   .HasMaxLength(100);
        }
    }
}
