namespace RecipeApp.Core.Features.PreferredDishFeature.Queries.Response
{
    public class GetAllPreferredDishesResponse
    {
        public int Id { get; set; }
        public string DishName { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
    }
}