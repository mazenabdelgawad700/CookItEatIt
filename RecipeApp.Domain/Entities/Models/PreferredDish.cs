namespace RecipeApp.Domain.Entities.Models
{
    public class PreferredDish
    {
        public int Id { get; set; }
        public int UserPreferencesId { get; set; }
        public int CountryId { get; set; }
        public string DishName { get; set; }
        public string ImageUrl { get; set; }

        public UserPreferences UserPreferences { get; set; }
        public Country Country { get; set; }
    }
}