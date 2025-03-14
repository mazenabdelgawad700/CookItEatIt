﻿namespace RecipeApp.Domain.Entities.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Recipe> Recipes { get; set; }
        public virtual ICollection<UserPreferredCategory> UserPreferredCategories { get; set; }
        public virtual ICollection<RecipeCategory> RecipeCategories { get; set; }
    }
}
