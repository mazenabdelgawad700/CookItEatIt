using RecipeApp.Core.Features.RecipeFeature.Command.Model;

namespace RecipeApp.Core.Features.RecipeFeature.Queries.Response
{
    public class GetRecipeByIdResponse
    {
        public int Id { get; set; }
        public string RecipeName { get; set; }
        public string ImgURL { get; set; }
        public List<CreateIngredientDto> Ingredients { get; set; }
        public List<CreateInstructionDto> Instructions { get; set; }
        public List<CategoryDto> Categories { get; set; }
        public bool IsCurrentUserLiked { get; set; } = false;
        public int LikesCount { get; set; }
        public bool IsVegetarian { get; set; }
        public int PreferredDishId { get; set; }
        public string Description { get; set; }
        public int ServesCount { get; set; }
        public int CookTimeMinutes { get; set; }
        public int UserId { get; set; }
    }

    public class CategoryDto
    {
        public string Name { get; set; } = null!;
    }
    //public class LikeDto
    //{
    //    public int UserId { get; set; }
    //}
}
