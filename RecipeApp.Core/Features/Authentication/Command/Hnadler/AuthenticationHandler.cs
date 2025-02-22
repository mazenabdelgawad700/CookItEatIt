using AutoMapper;
using MediatR;
using RecipeApp.Core.Features.Authentication.Command.Models;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.Authentication.Command.Hnadler
{
    internal class AuthenticationHandler : IRequestHandler<LoginCommand, ReturnBase<string>>, IRequestHandler<AddApplicationUserCommand, ReturnBase<string>>,
        IRequestHandler<ConfirmEmailCommand, ReturnBase<bool>>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IConfirmEmailSerivce _confirmEmailSerivce;
        private readonly IMapper _mapper;

        public AuthenticationHandler(IAuthenticationService authenticationService, IConfirmEmailSerivce confirmEmailSerivce, IMapper mapper)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
            _confirmEmailSerivce = confirmEmailSerivce;
        }

        public async Task<ReturnBase<string>> Handle(AddApplicationUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Domain.Entities.Identity.ApplicationUser mappedResult = _mapper.Map<Domain.Entities.Identity.ApplicationUser>(request);

                ReturnBase<string> createUserResult = await _authenticationService.RegisterAsync(mappedResult, request.Password);

                if (createUserResult.Succeeded)
                {
                    return ReturnBaseHandler.Created("", createUserResult.Message);
                }
                return ReturnBaseHandler.Failed<string>("Failed to add user.");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<string>(ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ReturnBase<bool> confrimEmailResult = await _confirmEmailSerivce.ConfirmEmailAsync(request.UserId, request.Token);

                if (confrimEmailResult.Succeeded)
                {
                    return ReturnBaseHandler.Created(true, confrimEmailResult.Message);
                }
                return ReturnBaseHandler.Failed<bool>(confrimEmailResult.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
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