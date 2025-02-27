using MediatR;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.Category.Command.Model
{
    public class UpdateCategoryCommand : IRequest<ReturnBase<bool>>
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}