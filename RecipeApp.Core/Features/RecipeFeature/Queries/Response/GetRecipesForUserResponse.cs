namespace RecipeApp.Core.Features.RecipeFeature.Queries.Response
{
    public class GetRecipesForUserResponse
    {
        public int Id { get; set; }
        public string RecipeName { get; set; }
        public bool IsVegterian { get; set; }
        public int LikesCount { get; set; }
        public string ImgURL { get; set; }
    }
}
