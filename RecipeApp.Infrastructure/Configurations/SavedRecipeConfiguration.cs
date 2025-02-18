using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Infrastructure.Configurations
{
    public class SavedRecipeConfiguration : IEntityTypeConfiguration<SavedRecipe>
    {
        public void Configure(EntityTypeBuilder<SavedRecipe> builder)
        {
            builder.HasKey(e => new { e.UserId, e.RecipeId });
        }
    }
}
