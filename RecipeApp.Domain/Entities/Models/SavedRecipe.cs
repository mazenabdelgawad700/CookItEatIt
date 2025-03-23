using RecipeApp.Domain.Entities.Identity;

namespace RecipeApp.Domain.Entities.Models
{
    public class SavedRecipe
    {
        public int UserId { get; set; }
        public int RecipeId { get; set; }
        public DateTime SavedAt { get; set; } = DateTime.UtcNow;
        public virtual ApplicationUser User { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}