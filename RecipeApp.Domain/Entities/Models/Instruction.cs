namespace RecipeApp.Domain.Entities.Models
{
    public class Instruction
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public int InstructionNumber { get; set; }
        public string Description { get; set; } = null!;
        public virtual Recipe Recipe { get; set; }
    }
}