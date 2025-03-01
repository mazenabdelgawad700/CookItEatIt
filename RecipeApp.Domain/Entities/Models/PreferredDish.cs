namespace RecipeApp.Domain.Entities.Models
{
    public class PreferredDish
    {
        public int Id { get; set; }
        public string DishName { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public virtual ICollection<Recipe> Recipes { get; set; }
        public virtual ICollection<UserPreferredDishes> UserPreferredDishes { get; set; }
    }
}