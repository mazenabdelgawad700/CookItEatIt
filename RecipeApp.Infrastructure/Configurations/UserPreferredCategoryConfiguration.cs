using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Infrastructure.Configurations
{
    internal class UserPreferredCategoryConfiguration : IEntityTypeConfiguration<UserPreferredCategory>
    {
        public void Configure(EntityTypeBuilder<UserPreferredCategory> builder)
        {
            builder.HasKey(e => new { e.UserPreferencesId, e.CategoryId });
        }
    }
}