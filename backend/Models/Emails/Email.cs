using Models.Core;

namespace Models.Emails;
public class Email : BaseModel
{
    public string SenderEmail { get; set; }
    public string ReceiverEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public EmailStatus Status { get; set; } = EmailStatus.PENDING;
    public EmailType Type { get; set; }
}
