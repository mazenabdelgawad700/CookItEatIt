using MediatR;
using RecipeApp.Core.Features.Authentication.Command.Models;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.Authentication.Command.Hnadler
{
    internal class AuthenticationHandler : IRequestHandler<LoginCommand, ReturnBase<string>>
    {
        private readonly IAuthenticationService _authenticationService;


        public AuthenticationHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<ReturnBase<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ReturnBase<string> loginUserResult = await _authenticationService.LoginInAsync(request.Email, request.Password);

                if (loginUserResult.Succeeded)
                    return ReturnBaseHandler.Success(loginUserResult.Data, loginUserResult.Message);

                return loginUserResult.Message switch
                {
                    "InvalidCredentials" => ReturnBaseHandler.Failed<string>("Please, enter valid credentials"),
                    "InvalidEmailOrPassword" => ReturnBaseHandler.Failed<string>("Invalid email or password"),
                    _ => ReturnBaseHandler.Failed<string>("Failed to log in, Please try again"),
                };
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<string>(ex.Message);
            }
        }
    }
}