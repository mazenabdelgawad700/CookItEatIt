namespace RecipeApp.Domain.Entities.Models
{
    public class UserPreferredCategory
    {
        public int UserPreferencesId { get; set; }
        public int CategoryId { get; set; }
        public UserPreferences UserPreferences { get; set; }
        public Category Category { get; set; }
    }
}