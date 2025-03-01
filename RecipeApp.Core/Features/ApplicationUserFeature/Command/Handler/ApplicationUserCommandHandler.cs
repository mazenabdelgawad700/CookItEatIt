using AutoMapper;
using MediatR;
using RecipeApp.Core.Features.ApplicationUserFeature.Command.Model;
using RecipeApp.Domain.Entities.Identity;
using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.ApplicationUserFeature.Command.Handler
{
  internal class ApplicationUserCommandHandler : IRequestHandler<UpdateApplicationUserCommand, ReturnBase<bool>>,
  IRequestHandler<AssignUserToCountryCommand, ReturnBase<bool>>
  {

    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly IApplicationUserService _applicationUserService;
    private readonly IConfirmEmailService _confirmationEmailService;
    private readonly IAuthenticationService _authenticationService;
    private readonly ISendEmailService _sendEmailService;
    private readonly IMapper _mapper;
    private readonly ICountryService _countryService;


    public ApplicationUserCommandHandler(IApplicationUserService applicationUserService, IMapper mapper, IConfirmEmailService confirmationEmailService, IAuthenticationService authenticationService, ISendEmailService sendEmailService, IApplicationUserRepository applicationUserRepository, ICountryService countryService)
    {
      _applicationUserRepository = applicationUserRepository;
      _confirmationEmailService = confirmationEmailService;
      _applicationUserService = applicationUserService;
      _authenticationService = authenticationService;
      _sendEmailService = sendEmailService;
      _mapper = mapper;
      _countryService = countryService;
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

        bool isCountryUpdated = request.CountryId is not null &&
            request.CountryId != getUserResult.Data.CountryId;

        if (isUserNameUpdated)
        {
          ReturnBase<bool> exsitingUserNameResult = await _authenticationService.IsEmailAlreadyRegisteredAsync(request.UserName);

          if (exsitingUserNameResult.Succeeded)
          {
            return ReturnBaseHandler.BadRequest<bool>("User Name already used");
          }
        }

        if (isEmailUpdated)
        {
          ReturnBase<bool> exsitingEmailResult = await _authenticationService.IsEmailAlreadyRegisteredAsync(request.Email);

          if (exsitingEmailResult.Succeeded)
          {
            return ReturnBaseHandler.BadRequest<bool>("Email already used");
          }
        }

        if (isCountryUpdated)
        {
          ReturnBase<bool> countryResult = await _applicationUserService.IsCountryValidAsync(request.CountryId);
          if (!countryResult.Succeeded)
          {
            return ReturnBaseHandler.BadRequest<bool>(countryResult.Message);
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

    public async Task<ReturnBase<bool>> Handle(AssignUserToCountryCommand request, CancellationToken cancellationToken)
    {
      try
      {
        var user = await _applicationUserService.GetApplicationUserProfileByIdAsync(request.UserId);
        if (!user.Succeeded)
          return ReturnBaseHandler.NotFound<bool>("User not found");

        var countryExists = await _countryService.IsCountryExistsAsync(request.CountryId);
        if (!countryExists.Succeeded)
          return ReturnBaseHandler.NotFound<bool>(countryExists.Message);

        user.Data.CountryId = request.CountryId;
        user.Data.UpdatedAt = DateTime.UtcNow;

        var updateResult = await _applicationUserService.UpdateApplicationUserAsync(user.Data);
        if (!updateResult.Succeeded)
          return ReturnBaseHandler.Failed<bool>(updateResult.Message);

        return ReturnBaseHandler.Success(true, "User assigned to country successfully");
      }
      catch (Exception ex)
      {
        return ReturnBaseHandler.Failed<bool>(ex.Message);
      }
    }

  }
}
