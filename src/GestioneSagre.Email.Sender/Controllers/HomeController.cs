using GestioneSagre.Email.Sender.BusinessLayer.Command;
using GestioneSagre.Email.Sender.BusinessLayer.Services;
using GestioneSagre.Email.Sender.Controllers.Common;
using GestioneSagre.SharedKernel.Models.Email;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestioneSagre.Email.Sender.Controllers;

public class HomeController : BaseController
{
    private readonly ILogger<HomeController> logger;
    private readonly IEmailService emailService;
    private readonly IMediator mediator;

    public HomeController(ILogger<HomeController> logger, IEmailService emailService, IMediator mediator)
    {
        this.logger = logger;
        this.emailService = emailService;
        this.mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> SendEmailAsync(EmailRequest request)
    {
        try
        {
            var message = await emailService.GetEmailMessageAsync(request.MessageId);

            if (message == null)
            {
                return NotFound();
            }

            var result = await mediator.Send(new SendEmailCommand(request));

            if (!result.Succeeded)
            {
                await emailService.IncrementEmailCounter(message.Id);
                return BadRequest();
            }

            await emailService.ChangeEmailSendStatus(message.Id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}