﻿using AutoMapper;
using MediatR;
using RecipeApp.Core.Features.ApplicationUserFeature.Command.Model;
using RecipeApp.Domain.Entities.Identity;
using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.ApplicationUserFeature.Command.Handler
{
    internal class ApplicationUserCommandHandler : IRequestHandler<UpdateApplicationUserCommand, ReturnBase<bool>>
    {

        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IConfirmEmailService _confirmationEmailService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ISendEmailService _sendEmailService;
        private readonly IMapper _mapper;


        public ApplicationUserCommandHandler(IApplicationUserService applicationUserService, IMapper mapper, IConfirmEmailService confirmationEmailService, IAuthenticationService authenticationService, ISendEmailService sendEmailService, IApplicationUserRepository applicationUserRepository)
        {
            _applicationUserRepository = applicationUserRepository;
            _confirmationEmailService = confirmationEmailService;
            _applicationUserService = applicationUserService;
            _authenticationService = authenticationService;
            _sendEmailService = sendEmailService;
            _mapper = mapper;
        }

        public async Task<ReturnBase<bool>> Handle(UpdateApplicationUserCommand request, CancellationToken cancellationToken)
        {
            await _applicationUserRepository.BeginTransactionAsync();
            try
            {
                ReturnBase<ApplicationUser> getUserResult = await _applicationUserService.GetApplicationUserProfileByIdAsync(request.Id);

                if (!getUserResult.Succeeded || getUserResult.Data is null)
                    return ReturnBaseHandler.Failed<bool>("User Not Found");

                string currentUserName = getUserResult.Data.UserName;
                string currentEmail = getUserResult.Data.Email;

                bool isEmailUpdated = !string.IsNullOrWhiteSpace(request.Email) && !request.Email.Equals(currentEmail, StringComparison.OrdinalIgnoreCase);

                bool isUserNameUpdated = !string.IsNullOrWhiteSpace(request.UserName) && !request.UserName.Equals(currentUserName, StringComparison.OrdinalIgnoreCase);

                if (isUserNameUpdated)
                {
                    ReturnBase<bool> exsitingUserNameResult = await _authenticationService.IsEmailAlreadyRegisteredAsync(request.UserName);

                    if (exsitingUserNameResult.Succeeded)
                    {
                        return ReturnBaseHandler.Failed<bool>("User Name already used");
                    }
                }

                if (isEmailUpdated)
                {
                    ReturnBase<bool> exsitingEmailResult = await _authenticationService.IsEmailAlreadyRegisteredAsync(request.Email);

                    if (exsitingEmailResult.Succeeded)
                    {
                        return ReturnBaseHandler.Failed<bool>("Email already used");
                    }
                }

                ApplicationUser mappedResult = _mapper.Map(request, getUserResult.Data);


                ReturnBase<bool> updateResult = await _applicationUserService.UpdateApplicationUserAsync(mappedResult);

                if (updateResult.Succeeded)
                {
                    string message;

                    if (isEmailUpdated)
                    {
                        mappedResult.EmailConfirmed = false;
                        await _confirmationEmailService.SendConfirmationEmailAsync(mappedResult);
                    }

                    if (isEmailUpdated && isUserNameUpdated)
                    {
                        message = @$"
<p>Dear {currentUserName},</p>
<p>Your email address: ""{currentEmail}"" and your user name: ""{currentUserName}"" have been changed at {DateTime.UtcNow:f}</p>
<p>The new email address is: ""{mappedResult.Email}""</p>
<p>The new user name is: ""{mappedResult.UserName}""</p>
<p>if you did not recive a confirmation email. Please log in to confirm your new email address</p>
<br />
<p>If you did not make this changes. Please contact support team at: +20 1111111111 </p>
";

                        ReturnBase<string> sendUpdateProfileEmailAndUserNameResult = await _sendEmailService.SendEmailAsync(mappedResult.Email, message, "Update Email Address", "text/html");

                        if (!sendUpdateProfileEmailAndUserNameResult.Succeeded)
                        {
                            await _applicationUserRepository.RollbackAsync();
                            return ReturnBaseHandler.Failed<bool>(sendUpdateProfileEmailAndUserNameResult.Message);
                        }
                    }
                    else if (isEmailUpdated)
                    {
                        message = @$"
<p>Dear {currentUserName},</p>
<p>Your email address: ""{currentEmail}"" has been changed at {DateTime.UtcNow:f}</p>
<p>The new email address is: ""{mappedResult.Email}""</p>
<p>if you did not recive a confirmation email. Please log in to confirm your new email address</p>
<br />
<p>If you did not make this change. Please contact support team at: +20 1111111111 </p>
";

                        ReturnBase<string> sendUpdateProfileEmailResult = await _sendEmailService.SendEmailAsync(currentEmail, message, "Update Email Address", "text/html");

                        if (!sendUpdateProfileEmailResult.Succeeded)
                        {
                            await _applicationUserRepository.RollbackAsync();
                            return ReturnBaseHandler.Failed<bool>(sendUpdateProfileEmailResult.Message);
                        }
                    }
                    else
                    {
                        message = @$"
<p>Dear {currentUserName},</p>
<p>Your user name: ""{currentUserName}"" has been changed at {DateTime.UtcNow:f}</p>
<p>Your new user name: ""{mappedResult.UserName}""</p>
<p>If you did not make this change. Please contact techincal support team at: +20 1111111111 </p>
";

                        ReturnBase<string> sendUpdateProfileUserNameResult = await _sendEmailService.SendEmailAsync(mappedResult.Email, message, "Update User Name", "text/html");

                        if (!sendUpdateProfileUserNameResult.Succeeded)
                        {
                            await _applicationUserRepository.RollbackAsync();
                            return ReturnBaseHandler.Failed<bool>(sendUpdateProfileUserNameResult.Message);
                        }
                    }

                    mappedResult.UpdatedAt = DateTime.UtcNow;

                    await _applicationUserRepository.CommitAsync();
                    await _applicationUserRepository.SaveChangesAsync();

                    return ReturnBaseHandler.Success(true, isEmailUpdated ? "User updated successfully. Please confirm your new email." : "User updated successfully");
                }

                await _applicationUserRepository.RollbackAsync();
                return ReturnBaseHandler.Failed<bool>(updateResult.Message);
            }
            catch (Exception ex)
            {
                await _applicationUserRepository.RollbackAsync();
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }

    }
}
