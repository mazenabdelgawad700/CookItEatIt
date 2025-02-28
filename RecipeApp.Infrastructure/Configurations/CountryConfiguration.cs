using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Infrastructure.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasKey(e => e.Id);

            // Relationship many users (belong to)-> one country 
            builder.HasMany(x => x.Users)
                   .WithOne(x => x.Country)
                   .HasForeignKey(x => x.CountryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}