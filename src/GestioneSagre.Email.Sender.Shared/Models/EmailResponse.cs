using GestioneSagre.Email.Sender.Shared.Enums;

namespace GestioneSagre.Email.Sender.Shared.Models;

public class EmailResponse
{
    public int Id { get; set; }
    public Guid MessageId { get; set; }
    public string Recipient { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public int SenderCount { get; set; }
    public MailStatus Status { get; set; }
}