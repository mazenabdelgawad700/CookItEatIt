using MediatR;
using RecipeApp.Shared.Bases;
using RecipeApp.Shared.SharedResponse;

namespace RecipeApp.Core.Features.ApplicationUserSettingsFeature.Command.Model
{
    public class UpdateApplicationUserSettingsCommand : UpdateApplicationUserSettingsCommandShared, IRequest<ReturnBase<bool>>
    {

    }
}