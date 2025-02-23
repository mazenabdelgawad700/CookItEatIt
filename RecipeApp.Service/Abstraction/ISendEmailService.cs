using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Abstraction
{
    public interface ISendEmailService
    {
        Task<ReturnBase<string>> SendEmailAsync(string email, string message, string subject, string contentType = "text/plain");
    }
}