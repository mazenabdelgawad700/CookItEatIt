using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Infrastructure.Configurations
{
    public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.RecipeName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.Description)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(x => x.ImgURL)
                   .HasMaxLength(500);

            builder.Property(x => x.ServesCount)
                   .IsRequired();

            builder.Property(x => x.CookTimeMinutes)
                   .IsRequired();

            builder.Property(x => x.IsVegetarian)
                   .IsRequired();

            builder.Property(x => x.CreatedAt)
                   .IsRequired();

            // Relationship with Categories (Many-to-Many)
            builder.HasMany(r => r.Categories)
                   .WithMany(c => c.Recipes)
                   .UsingEntity<RecipeCategory>(
                       j => j.HasOne(rc => rc.Category)
                             .WithMany(c => c.RecipeCategories)
                             .HasForeignKey(rc => rc.CategoryId),
                       j => j.HasOne(rc => rc.Recipe)
                             .WithMany(r => r.RecipeCategories)
                             .HasForeignKey(rc => rc.RecipeId),
                       j =>
                       {
                           j.HasKey(rc => new { rc.RecipeId, rc.CategoryId });
                       }
                   );

            // Relationship with User (Creator)
            builder.HasOne(x => x.User)
                   .WithMany(x => x.Recipes)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Relationship with Ingredients
            builder.HasMany(x => x.Ingredients)
                   .WithOne(x => x.Recipe)
                   .HasForeignKey(x => x.RecipeId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Relationship with Instructions
            builder.HasMany(x => x.Instructions)
                   .WithOne(x => x.Recipe)
                   .HasForeignKey(x => x.RecipeId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Recipe", "dbo", tb =>
            {
                tb.HasTrigger("tr_Recipe_UpdateRecipesCount");
            });
        }
    }
}
