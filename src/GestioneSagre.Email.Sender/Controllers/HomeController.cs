using GestioneSagre.Email.Sender.BusinessLayer.Command;
using GestioneSagre.Email.Sender.Controllers.Common;
using GestioneSagre.Email.Sender.DataAccessLayer;
using GestioneSagre.Email.Sender.Shared.Enums;
using GestioneSagre.Email.Sender.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestioneSagre.Email.Sender.Controllers;

public class HomeController : BaseController
{
    private readonly ILogger<HomeController> logger;
    private readonly EmailSenderDbContext dbContext;
    private readonly IMediator mediator;

    public HomeController(ILogger<HomeController> logger, EmailSenderDbContext dbContext, IMediator mediator)
    {
        this.logger = logger;
        this.dbContext = dbContext;
        this.mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> SendEmailAsync(EmailRequest request)
    {
        try
        {
            var result = await mediator.Send(new SendEmailCommand(request));

            var message = await dbContext.EmailMessages
                .AsNoTracking()
                .Where(x => x.MessageId == request.MessageId)
                .FirstOrDefaultAsync();

            if (!result.Succeeded)
            {
                var entity1 = await dbContext.EmailMessages.FindAsync(message.Id);
                var newCount = entity1.SenderCount + 1;

                entity1.ChangeSenderCount(newCount);

                await dbContext.SaveChangesAsync();

                return BadRequest();
            }

            var entity2 = await dbContext.EmailMessages.FindAsync(message.Id);
            entity2.ChangeStatus(MailStatus.Sent);

            await dbContext.SaveChangesAsync();

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}