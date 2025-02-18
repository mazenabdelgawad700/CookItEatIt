namespace RecipeApp.Domain.Entities.Models
{
    public class RecipeCategory
    {
        public int RecipeId { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}