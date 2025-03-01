using RecipeApp.Domain.Entities.Identity;

namespace RecipeApp.Domain.Entities.Models
{
    public class UserPreferredDishes
    {
        public int UserId { get; set; }
        public int PreferredDishId { get; set; }
        public virtual PreferredDish PreferredDish { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
