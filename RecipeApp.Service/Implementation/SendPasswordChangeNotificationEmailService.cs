using RecipeApp.Domain.Entities.Identity;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Implementation
{
    internal class SendPasswordChangeNotificationEmailService : ISendPasswordChangeNotificationEmailService
    {

        private readonly ISendEmailService _emailService;


        public SendPasswordChangeNotificationEmailService(ISendEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task<ReturnBase<bool>> SendPasswordChangeNotificationAsync(ApplicationUser user)
        {
            try
            {
                if (user is not null)
                {
                    string message = @"
                <p>Your password has been successfully changed.</p>
                <p>If you did not request this change, please contact support immediately.</p>";

                    ReturnBase<string> sendEmailResult = await _emailService.SendEmailAsync(
                        user.Email!,
                        message,
                        "Changing password",
                        "text/html"
                    );

                    return ReturnBaseHandler.Success(sendEmailResult.Data == "Success", sendEmailResult.Message);
                }

                return ReturnBaseHandler.Failed<bool>("User not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
    }
}
