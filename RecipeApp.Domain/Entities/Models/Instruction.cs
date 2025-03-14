namespace RecipeApp.Domain.Entities.Models
{
    public class Instruction
    {
        public int RecipeId { get; set; }
        public byte InstructionNumber { get; set; }
        public string Description { get; set; } = null!;
        public virtual Recipe Recipe { get; set; }
    }
}