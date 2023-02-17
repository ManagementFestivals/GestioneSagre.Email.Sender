using GestioneSagre.Email.Sender.Shared.Options;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace GestioneSagre.Email.Sender.BusinessLayer.Services;

public class MailKitEmailSender : IEmailClient
{
    private readonly ILogger<MailKitEmailSender> logger;
    private readonly IOptionsMonitor<SmtpOptions> smtpOptionsMonitor;

    public MailKitEmailSender(IOptionsMonitor<SmtpOptions> smtpOptionsMonitor, ILogger<MailKitEmailSender> logger)
    {
        this.logger = logger;
        this.smtpOptionsMonitor = smtpOptionsMonitor;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        return SendEmailAsync(email, string.Empty, subject, htmlMessage);
    }

    public async Task<bool> SendEmailAsync(string recipientEmail, string replyToEmail, string subject, string htmlMessage)
    {
        try
        {
            var options = this.smtpOptionsMonitor.CurrentValue;

            using SmtpClient client = new();
            await client.ConnectAsync(options.Host, options.Port, options.Security);

            if (!string.IsNullOrEmpty(options.Username))
            {
                await client.AuthenticateAsync(options.Username, options.Password);
            }

            MimeMessage message = new();
            message.From.Add(MailboxAddress.Parse(options.Sender));
            message.To.Add(MailboxAddress.Parse(recipientEmail));

            if (replyToEmail is not (null or ""))
            {
                message.ReplyTo.Add(MailboxAddress.Parse(replyToEmail));
            }

            message.Subject = subject;
            message.Body = new TextPart("html")
            {
                Text = htmlMessage
            };

            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            return true;
        }
        catch
        {
            return false;
        }
    }
}