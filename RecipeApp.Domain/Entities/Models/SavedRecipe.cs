using RecipeApp.Domain.Entities.Identity;

namespace RecipeApp.Domain.Entities.Models
{
    public class SavedRecipe
    {
        public int UserId { get; set; }
        public int RecipeId { get; set; }
        public DateTime SavedAt { get; set; } = DateTime.Now;
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}