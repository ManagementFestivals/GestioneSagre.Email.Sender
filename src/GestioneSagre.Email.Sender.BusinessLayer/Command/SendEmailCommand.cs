using GestioneSagre.Email.Sender.Shared.Models;
using MediatR;

namespace GestioneSagre.Email.Sender.BusinessLayer.Command;

public class SendEmailCommand : IRequest<EmailSendResponse>
{
    public Guid MessageId { get; set; }
    public string RecipientEmail { get; set; }
    public string Subject { get; set; }
    public string HtmlMessage { get; set; }

    public SendEmailCommand(EmailRequest request)
    {
        MessageId = request.MessageId;
        RecipientEmail = request.RecipientEmail;
        Subject = request.Subject;
        HtmlMessage = request.HtmlMessage;
    }
}