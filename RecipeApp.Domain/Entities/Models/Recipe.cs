using RecipeApp.Domain.Entities.Identity;

namespace RecipeApp.Domain.Entities.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PreferredDishId { get; set; }
        public string RecipeName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ImgURL { get; set; }
        public byte ServesCount { get; set; }
        public int CookTimeMinutes { get; set; }
        public bool IsVegetarian { get; set; }
        public int LikesCount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public virtual ApplicationUser User { get; set; }
        public virtual PreferredDish PreferredDish { get; set; }
        public virtual ICollection<Ingredient> Ingredients { get; set; }
        public virtual ICollection<Instruction> Instructions { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<RecipeLike> Likes { get; set; }
        public virtual ICollection<SavedRecipe> SavedRecipes { get; set; }
        public virtual ICollection<RecipeCategory> RecipeCategories { get; set; }
    }
}