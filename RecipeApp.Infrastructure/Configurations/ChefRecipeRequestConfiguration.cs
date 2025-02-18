using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Infrastructure.Configurations
{
    public class ChefRecipeRequestConfiguration : IEntityTypeConfiguration<ChefRecipeRequest>
    {
        public void Configure(EntityTypeBuilder<ChefRecipeRequest> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Description)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.Property(x => x.Status)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasOne(x => x.User)
                   .WithMany(x => x.RecipeRequests)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Chef)
                   .WithMany(x => x.ReceivedRequests)
                   .HasForeignKey(x => x.ChefId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}