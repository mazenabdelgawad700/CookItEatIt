using RecipeApp.Domain.Entities.Identity;

namespace RecipeApp.Domain.Entities.Models
{
    public class UserPreferences
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool IsVegetarian { get; set; }
        public bool AcceptNewDishNotification { get; set; }
        public int DefaultHungryHeads { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<PreferredDish> PreferredDishes { get; set; }
        public ICollection<Category> PreferredCategories { get; set; }
    }
}