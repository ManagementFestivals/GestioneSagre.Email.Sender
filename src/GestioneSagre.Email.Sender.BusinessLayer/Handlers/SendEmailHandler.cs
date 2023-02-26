using GestioneSagre.Email.Sender.BusinessLayer.Command;
using GestioneSagre.Email.Sender.BusinessLayer.Services;
using GestioneSagre.SharedKernel.Models.Email;
using MediatR;

namespace GestioneSagre.Email.Sender.BusinessLayer.Handlers;

public class SendEmailHandler : IRequestHandler<SendEmailCommand, EmailSendResponse>
{
    private readonly IEmailClient emailClient;

    public SendEmailHandler(IEmailClient emailClient)
    {
        this.emailClient = emailClient;
    }

    public async Task<EmailSendResponse> Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        var result = await emailClient.SendEmailAsync(request.RecipientEmail, string.Empty, request.Subject, request.HtmlMessage);

        if (!result)
        {
            return new EmailSendResponse { Succeeded = false };
        }

        return new EmailSendResponse { Succeeded = true };
    }
}