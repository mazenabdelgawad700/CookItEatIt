using MailKit.Net.Smtp;
using MimeKit;
using RecipeApp.Domain.Helpers;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Service.Implementaion
{
    internal class SendEmailService : ISendEmailService
    {

        #region Fields
        private readonly EmailSettings _emailSettings;
        #endregion

        #region Constructors
        public SendEmailService(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }
        #endregion

        //public async Task<ReturnBase<string>> SendEmailAsync(string email, string message, string contentType = "text/plain")
        //{
        //    try
        //    {
        //            using MailKit.Net.Smtp.SmtpClient client = new ();
        //            await client.ConnectAsync(_emailSettings.ClientConnectionString, _emailSettings.ClientConnectionPort, _emailSettings.ClientConnectionUseSSL);
        //await client.AuthenticateAsync(_emailSettings.AuthenticationEmail, _emailSettings.AuthenticationPassword);

        //        BodyBuilder bodyBuilder = new();

        //        if (contentType.Equals("text/html"))
        //            bodyBuilder.HtmlBody = message;

        //        else
        //            bodyBuilder.TextBody = message;

        //        MimeMessage mimeMessage = new()
        //        {
        //            Body = bodyBuilder.ToMessageBody(),
        //        };

        //        mimeMessage.From.Add(new MailboxAddress(_emailSettings.MailBoxSenderHeader, _emailSettings.AuthenticationEmail));
        //        mimeMessage.To.Add(new MailboxAddress(_emailSettings.MailBoxSenderHeader, email));
        //        mimeMessage.Subject = _emailSettings.MailBoxSubject;

        //        await client.SendAsync(mimeMessage);

        //        client.Disconnect(true);
        //        client.Dispose();

        //        return ReturnBaseHandler.Success("Success", "");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return ReturnBaseHandler.Success("Fail", ex.Message);
        //    }
        //}

        public async Task<ReturnBase<string>> SendEmailAsync(string email, string message, string contentType = "text/plain")
        {
            try
            {
                /* SmtpClient smtpClient = new(_emailSettings.AuthenticationEmail)
                 {
                     Host = _emailSettings.ClientConnectionString,
                     Port = _emailSettings.ClientConnectionPort,
                     Credentials = new NetworkCredential(_emailSettings.AuthenticationEmail, _emailSettings.AuthenticationPassword),
                     EnableSsl = _emailSettings.ClientConnectionUseSSL,
                 };

                 MailMessage mailMessage = new()
                 {
                     From = new MailAddress(_emailSettings.AuthenticationEmail),
                     Subject = _emailSettings.MailBoxSubject,
                     Body = message,
                     IsBodyHtml = contentType == "text/html",
                 };

                 mailMessage.To.Add(email);

                 await smtpClient.SendMailAsync(mailMessage);

                 smtpClient.Dispose();*/

                MimeMessage email_Message = new();

                MailboxAddress email_From = new
                    (_emailSettings.MailBoxSenderHeader, _emailSettings.AuthenticationEmail);

                email_Message.From.Add(email_From);

                MailboxAddress email_To = new(_emailSettings.MailBoxSenderHeader, email);
                email_Message.To.Add(email_To);
                email_Message.Subject = _emailSettings.MailBoxSubject;

                BodyBuilder emailBodyBuilder = new();
                emailBodyBuilder.HtmlBody = message;
                email_Message.Body = emailBodyBuilder.ToMessageBody();


                using SmtpClient MailClient = new();
                await MailClient.ConnectAsync(_emailSettings.ClientConnectionString, _emailSettings.ClientConnectionPort, _emailSettings.ClientConnectionUseSSL);
                await MailClient.AuthenticateAsync(_emailSettings.AuthenticationEmail, _emailSettings.AuthenticationPassword);

                await MailClient.SendAsync(email_Message);
                MailClient.Disconnect(true);
                MailClient.Dispose();

                return ReturnBaseHandler.Success("Success", "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<string>(ex.Message);
            }
        }

    }
}
