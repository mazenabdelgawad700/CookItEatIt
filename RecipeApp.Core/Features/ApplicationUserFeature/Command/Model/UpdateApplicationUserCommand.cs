using MediatR;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.ApplicationUserFeature.Command.Model
{
    public class UpdateApplicationUserCommand : IRequest<ReturnBase<bool>>
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Bio { get; set; }
        public string? Email { get; set; }
        public int? CountryId { get; set; }
    }
}