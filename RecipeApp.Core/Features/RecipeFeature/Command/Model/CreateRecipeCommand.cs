using MediatR;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.RecipeFeature.Command.Model
{
    public class CreateRecipeCommand : IRequest<ReturnBase<int>>
    {
        public string RecipeName { get; set; }
        public List<CreateIngredientDto> Ingredients { get; set; }
        public List<CreateInstructionDto> Instructions { get; set; }
        public bool IsVegetarian { get; set; }
        public int PreferredDishId { get; set; }
        public string Description { get; set; }
        public int ServesCount { get; set; }
        public int CookTimeMinutes { get; set; }
        public int UserId { get; set; }
    }

    public class CreateIngredientDto
    {
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
    }

    public class CreateInstructionDto
    {
        public int InstructionNumber { get; set; }
        public string Description { get; set; }
    }
}