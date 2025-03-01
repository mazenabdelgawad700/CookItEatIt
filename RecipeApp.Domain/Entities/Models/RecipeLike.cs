using RecipeApp.Domain.Entities.Identity;

namespace RecipeApp.Domain.Entities.Models
{
    public class RecipeLike
    {
        public int UserId { get; set; }
        public int RecipeId { get; set; }
        public DateTime LikedAt { get; set; } = DateTime.UtcNow;
        public virtual ApplicationUser User { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}