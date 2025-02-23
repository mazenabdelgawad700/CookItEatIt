using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RecipeApp.Domain.Entities.Identity;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;
using System.Net;

namespace RecipeApp.Service.Implementation
{
    internal class ConfirmEmailSerivce : IConfirmEmailSerivce
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISendEmailService _emailService;

        public ConfirmEmailSerivce(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor,
            ISendEmailService emailService)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }

        public async Task<ReturnBase<bool>> ConfirmEmailAsync(int userId, string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return ReturnBaseHandler.Failed<bool>("Invalid Token");
                }
                ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());

                if (user is null)
                    return ReturnBaseHandler.Failed<bool>("Invalid User Id");

                IdentityResult confirmEmailResult = await _userManager.ConfirmEmailAsync(user, token);

                if (confirmEmailResult.Succeeded)
                    return ReturnBaseHandler.Success(true, "Email confirmed successfully");

                return ReturnBaseHandler.Failed<bool>("Failed to confirm email address, please try again");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
        public async Task<ReturnBase<bool>> SendConfirmationEmailAsync(ApplicationUser user)
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
                        Path = "api/authentication/ConfirmEmail",
                        Query = $"userId={user.Id}&token={encodedToken}"
                    };

                    string returnUrl = uriBuilder.ToString();

                    string message = $"To Confirm Email Click Link: <a href=\"{returnUrl}\">Confirmation Link</a>";

                    ReturnBase<string> sendEmailResult = await _emailService.SendEmailAsync(user.Email, message, "Confirmation Link", "text /html");
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