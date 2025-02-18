namespace RecipeApp.Domain.Entities.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Recipe> Recipes { get; set; }
    }
}
