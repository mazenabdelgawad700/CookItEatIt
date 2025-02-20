using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RecipeApp.Domain.Entities.Identity;
using RecipeApp.Infrastructure.Context;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;
using System.Net;

namespace RecipeApp.Service.Implementaion
{
    internal class ApplicationUserService : IApplicationUserService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISendEmailService _emailService;

        public ApplicationUserService(UserManager<ApplicationUser> userManager, AppDbContext dbContext, IHttpContextAccessor httpContextAccessor,
            ISendEmailService emailService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }

        public async Task<ReturnBase<string>> AddApplicationUserAsync(ApplicationUser appUser, string password)
        {
            try
            {
                IdentityResult createUserResult = await _userManager.CreateAsync(appUser, password);

                if (createUserResult.Succeeded)
                {
                    ReturnBase<bool> sendConfirmationEmailResult = await SendEmailConfirmationAsync(appUser);
                    if (sendConfirmationEmailResult.Data)
                    {
                        return ReturnBaseHandler.Created("", $"We have sent a confirmation email to {appUser.Email}.Please, confirm your email");
                    }
                    return ReturnBaseHandler.Created("", "We could not send a confirmation email to you, please log in to confirm your email!");
                }
                return ReturnBaseHandler.Failed<string>("An error occurred while creating user.");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<string>($"{ex.Message}");
            }
        }
        public async Task<ReturnBase<bool>> IsEmailAlreadyRegisteredAsync(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return ReturnBaseHandler.BadRequest<bool>("Email is required.");
                }

                ApplicationUser? user = await _userManager.FindByEmailAsync(email);
                if (user is not null)
                {
                    return ReturnBaseHandler.Success(true, "Email is already registered.");
                }
                return ReturnBaseHandler.Success(false, "Email is available.");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>($"An error occurred: {ex.Message}");
            }
        }
        private async Task<ReturnBase<bool>> SendEmailConfirmationAsync(ApplicationUser user)
        {
            try
            {
                if (user is not null)
                {
                    string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    string encodedToken = WebUtility.UrlEncode(code);
                    HttpRequest resquestAccessor = _httpContextAccessor.HttpContext.Request;

                    UriBuilder uriBuilder = new()
                    {
                        Scheme = resquestAccessor.Scheme,
                        Host = resquestAccessor.Host.Host,
                        Port = resquestAccessor.Host.Port ?? -1,
                        Path = "Api/V1/Authentication/ConfirmEmail",
                        Query = $"userId={user.Id}&token={encodedToken}"
                    };

                    string returnUrl = uriBuilder.ToString();

                    string message = $"<html><body>To Confirm Email Click Link: <a href=\"{returnUrl}\">Confirmation Link</a></body></html>";

                    ReturnBase<string> sendEmailResult = await _emailService.SendEmailAsync(user.Email, message, "text/html");
                    return ReturnBaseHandler.Success(sendEmailResult.Data == "Success", sendEmailResult.Message);
                }
                return ReturnBaseHandler.Failed<bool>("");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
    }
}