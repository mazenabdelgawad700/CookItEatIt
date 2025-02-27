using MediatR;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.Category.Command.Model
{
    public class AddCategoryCommand : IRequest<ReturnBase<bool>>
    {
        public string Name { get; set; } = null!;
    }
}