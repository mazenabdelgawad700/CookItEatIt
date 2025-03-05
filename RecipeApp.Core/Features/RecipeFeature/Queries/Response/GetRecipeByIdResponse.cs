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
        public bool IsVegetarian { get; set; }
        public int PreferredDishId { get; set; }
        public string Description { get; set; }
        public int ServesCount { get; set; }
        public int CookTimeMinutes { get; set; }
        public int UserId { get; set; }
    }
}
