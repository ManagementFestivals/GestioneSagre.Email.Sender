using GestioneSagre.Email.Sender.DataAccessLayer;
using GestioneSagre.Email.Sender.DataAccessLayer.Entities;
using GestioneSagre.SharedKernel.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GestioneSagre.Email.Sender.BusinessLayer.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> logger;
    private readonly EmailSenderDbContext dbContext;

    public EmailService(ILogger<EmailService> logger, EmailSenderDbContext dbContext)
    {
        this.logger = logger;
        this.dbContext = dbContext;
    }

    public async Task ChangeEmailSendStatus(int id)
    {
        var entity2 = await dbContext.EmailMessages.FindAsync(id);
        entity2.ChangeStatus(MailStatus.Sent);

        await dbContext.SaveChangesAsync();
    }

    public async Task<EmailMessage> GetEmailMessageAsync(Guid messageId)
    {
        var message = await dbContext.EmailMessages
                .AsNoTracking()
                .Where(x => x.MessageId == messageId)
                .FirstOrDefaultAsync();

        return message;
    }

    public async Task IncrementEmailCounter(int id)
    {
        var entity = await dbContext.EmailMessages.FindAsync(id);
        var newCount = entity.SenderCount + 1;

        entity.ChangeSenderCount(newCount);

        await dbContext.SaveChangesAsync();
    }
}