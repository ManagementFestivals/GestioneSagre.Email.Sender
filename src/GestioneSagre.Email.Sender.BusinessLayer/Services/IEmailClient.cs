using Microsoft.AspNetCore.Identity.UI.Services;

namespace GestioneSagre.Email.Sender.BusinessLayer.Services;

public interface IEmailClient : IEmailSender
{
    Task<bool> SendEmailAsync(string recipientEmail, string replyToEmail, string subject, string htmlMessage);
}