using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using RecipeApp.Core.Features.Authentication.Command.Models;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.Authentication.Command.Hnadler
{
    internal class AuthenticationHandler : IRequestHandler<LoginCommand, ReturnBase<string>>, IRequestHandler<RegisterCommand, ReturnBase<string>>,
        IRequestHandler<ConfirmEmailCommand, ReturnBase<bool>>,
        IRequestHandler<ChangePasswordCommand, ReturnBase<bool>>,
        IRequestHandler<RefreshTokenCommand, ReturnBase<string>>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IConfirmEmailSerivce _confirmEmailSerivce;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationHandler(IAuthenticationService authenticationService, IConfirmEmailSerivce confirmEmailSerivce, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
            _confirmEmailSerivce = confirmEmailSerivce;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ReturnBase<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
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
                string? ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();

                if (ipAddress is null)
                    ReturnBaseHandler.Failed<string>("Something went wront, Please try again");

                ReturnBase<string> loginUserResult = await _authenticationService.LoginInAsync(request.Email, request.Password, ipAddress);

                if (loginUserResult.Succeeded)
                    return ReturnBaseHandler.Success(loginUserResult.Data, loginUserResult.Message);

                return loginUserResult.Message switch
                {
                    "InvalidCredentials" => ReturnBaseHandler.Failed<string>("Please, enter valid credentials"),
                    "InvalidEmailOrPassword" => ReturnBaseHandler.Failed<string>("Invalid email or password"),
                    _ => ReturnBaseHandler.Failed<string>(loginUserResult.Message),
                };
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<string>(ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ReturnBase<bool> changePasswordUserResult = await _authenticationService.ChangePasswordAsync(request.Id, request.CurrentPassword, request.NewPassword);

                if (changePasswordUserResult.Succeeded)
                    return ReturnBaseHandler.Success(changePasswordUserResult.Data, changePasswordUserResult.Message);

                return changePasswordUserResult.Message switch
                {
                    "PasswordsDoNotProvided" => ReturnBaseHandler.Failed<bool>("Please, enter required fields"),
                    "InvalidUserId" => ReturnBaseHandler.Failed<bool>("User Not Found"),
                    "FailedToSendChangePasswordEmail" => ReturnBaseHandler.Failed<bool>("Something went wrong. Please, try again"),
                    _ => ReturnBaseHandler.Failed<bool>(changePasswordUserResult.Message),
                };
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
        public async Task<ReturnBase<string>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.AccessToken is null)
                    return ReturnBaseHandler.Failed<string>("Access Token is required");

                ReturnBase<string> refreshTokenResult = await _authenticationService.RefreshTokenAsync(request.AccessToken);

                if (refreshTokenResult.Succeeded)
                    return ReturnBaseHandler.Success(refreshTokenResult.Data, refreshTokenResult.Message);

                return refreshTokenResult.Message switch
                {
                    "InvalidAccessToken" => ReturnBaseHandler.Failed<string>("Your session has expired. please log in again."),
                    "FailedToGenerateNewAccessToken" =>
                    ReturnBaseHandler.Failed<string>("Something went wrong. Please log in again."),
                    _ => ReturnBaseHandler.Failed<string>(refreshTokenResult.Message),
                };
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<string>(ex.Message);
            }
        }
    }
}