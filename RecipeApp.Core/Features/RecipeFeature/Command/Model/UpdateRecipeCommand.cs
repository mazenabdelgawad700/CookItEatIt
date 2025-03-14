using MediatR;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.RecipeFeature.Command.Model
{
    public class UpdateRecipeCommand : IRequest<ReturnBase<bool>>
    {
        public int RecipeId { get; set; }
        public int UserId { get; set; }
        public string RecipeName { get; set; }
        public List<CreateIngredientDto> Ingredients { get; set; }
        public List<CreateInstructionDto> Instructions { get; set; }
        public List<int> CategoryIds { get; set; }
        public bool IsVegetarian { get; set; }
        public int PreferredDishId { get; set; }
        public string Description { get; set; }
        public int ServesCount { get; set; }
        public int CookTimeMinutes { get; set; }
    }
}