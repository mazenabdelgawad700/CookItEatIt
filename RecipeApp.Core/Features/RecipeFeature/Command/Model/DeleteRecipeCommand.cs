using MediatR;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.RecipeFeature.Command.Model
{
    public class DeleteRecipeCommand : IRequest<ReturnBase<bool>>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ImgURL { get; set; }
    }
}
