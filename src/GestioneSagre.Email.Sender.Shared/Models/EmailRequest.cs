namespace GestioneSagre.Email.Sender.Shared.Models;

public class EmailRequest
{
    public Guid MessageId { get; set; }
    public string RecipientEmail { get; set; }
    public string Subject { get; set; }
    public string HtmlMessage { get; set; }
}