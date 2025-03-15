namespace RecipeApp.Core.Features.RecipeFeature.Queries.Response
{
    public class GetAllRecipesResponse
    {
        public bool IsVegetarian { get; set; }
        public string RecipeName { get; set; }
        public int RecipeId { get; set; }
        public byte ServesCount { get; set; }
        public decimal CookTime { get; set; }
        public string AuthorName { get; set; }
        public string ImgURL { get; set; }
    }
}
