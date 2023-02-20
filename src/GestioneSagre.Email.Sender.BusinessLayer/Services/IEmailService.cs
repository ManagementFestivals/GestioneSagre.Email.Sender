using GestioneSagre.Email.Sender.DataAccessLayer.Entities;

namespace GestioneSagre.Email.Sender.BusinessLayer.Services;
public interface IEmailService
{
    Task<EmailMessage> GetEmailMessageAsync(Guid messageId);
    Task IncrementEmailCounter(int id);
    Task ChangeEmailSendStatus(int id);
}