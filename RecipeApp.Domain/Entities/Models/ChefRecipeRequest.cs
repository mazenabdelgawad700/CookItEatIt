using RecipeApp.Domain.Entities.Identity;

namespace RecipeApp.Domain.Entities.Models
{
    public class ChefRecipeRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ChefId { get; set; }
        public string Description { get; set; } = null!;
        public string Status { get; set; } = null!; // Pending, Approved, Rejected
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationUser Chef { get; set; }
    }
}