namespace RecipeApp.Core.Features.RecipeFeature.Queries.Response
{
    public class GetSavedRecipesAsPaginatedResponse
    {
        public int RecipeId { get; set; }
        public string ImgURL { get; set; }
        public string RecipeName { get; set; } = null!;
        public int LikesCount { get; set; }
        public bool IsVegetarian { get; set; }
    }
}
