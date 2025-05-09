﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using RecipeApp.Core.Features.Authentication.Command.Models;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;
using System.Net;

namespace RecipeApp.Core.Features.Authentication.Command.Hnadler
{
    internal class AuthenticationHandler : IRequestHandler<LoginCommand, ReturnBase<string>>, IRequestHandler<RegisterCommand, ReturnBase<string>>,
        IRequestHandler<ConfirmEmailCommand, ReturnBase<bool>>,
        IRequestHandler<ChangePasswordCommand, ReturnBase<bool>>,
        IRequestHandler<RefreshTokenCommand, ReturnBase<string>>,
        IRequestHandler<ResetPasswordEmailCommand, ReturnBase<bool>>,
        IRequestHandler<ResetPasswordCommand, ReturnBase<bool>>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IConfirmEmailService _confirmEmailService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationHandler(IAuthenticationService authenticationService, IConfirmEmailService confirmEmailService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
            _confirmEmailService = confirmEmailService;
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

                return createUserResult.Message switch
                {
                    "DuplicateUserName" => ReturnBaseHandler.Failed<string>("User Name already used"),
                    _ => ReturnBaseHandler.Failed<string>(createUserResult.Message),
                };
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
                ReturnBase<bool> confrimEmailResult = await _confirmEmailService.ConfirmEmailAsync(request.UserId, request.Token);

                if (confrimEmailResult.Succeeded)
                {
                    return ReturnBaseHandler.Success(true, confrimEmailResult.Message);
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

                if (loginUserResult.Message.Equals("InvalidCredentials"))
                    return ReturnBaseHandler.Failed<string>("Please, enter valid credentials");

                else if (loginUserResult.Message.Equals("InvalidEmailOrPassword"))
                    return ReturnBaseHandler.Failed<string>("Invalid email or password");

                else
                {
                    return loginUserResult.StatusCode switch
                    {
                        HttpStatusCode.BadRequest => ReturnBaseHandler.BadRequest<string>(loginUserResult.Message),
                        _ => ReturnBaseHandler.Failed<string>(loginUserResult.Message)
                    };
                }

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

        public async Task<ReturnBase<bool>> Handle(ResetPasswordEmailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Email is null)
                    return ReturnBaseHandler.Failed<bool>("Email is required");

                ReturnBase<bool> sendResetPasswordEmailResult = await _authenticationService.SendResetPasswordEmailAsync(request.Email);

                if (sendResetPasswordEmailResult.Succeeded)
                    return ReturnBaseHandler.Success(sendResetPasswordEmailResult.Data, sendResetPasswordEmailResult.Message);

                return ReturnBaseHandler.Failed<bool>(sendResetPasswordEmailResult.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.ResetPasswordToken is null)
                    return ReturnBaseHandler.Failed<bool>("Invalid Token");

                if (request.NewPassword is null)
                    return ReturnBaseHandler.Failed<bool>("Invalid Password");

                if (request.Email is null)
                    return ReturnBaseHandler.Failed<bool>("Invalid Email");

                ReturnBase<bool> resetPasswordResult = await _authenticationService.ResetPasswordAsync(request.ResetPasswordToken, request.NewPassword, request.Email);

                if (resetPasswordResult.Succeeded)
                    return ReturnBaseHandler.Success(resetPasswordResult.Data, resetPasswordResult.Message);

                return ReturnBaseHandler.Failed<bool>(resetPasswordResult.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
    }
}