﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Infrastructure.Configurations
{
    public class PreferredDishConfiguration : IEntityTypeConfiguration<PreferredDish>
    {
        public void Configure(EntityTypeBuilder<PreferredDish> builder)
        {
            builder.HasKey(pd => pd.Id);

            builder.Property(pd => pd.DishName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasIndex(pd => pd.DishName).IsUnique();

            builder.Property(pd => pd.ImageUrl)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.HasMany(pd => pd.UserPreferredDishes)
                .WithOne(upd => upd.PreferredDish)
                .HasForeignKey(upd => upd.PreferredDishId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many relationship with Recipes
            builder.HasMany(x => x.Recipes)
                   .WithOne(x => x.PreferredDish)
                   .HasForeignKey(x => x.PreferredDishId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
