using Models.Accounts;
using Models.Core;

namespace Models.Messages;
public class Message : BaseModel
{
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public string Content { get; set; }
    public MessageStatus Status { get; set; } = MessageStatus.SENT;

    // Navigation Properties
    public Account Sender { get; set; }   // 1-to-1 with Account (sender)
    public Account Receiver { get; set; } // 1-to-1 with Account (receiver)
}
