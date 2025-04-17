using Models.Accounts;
using Models.Core;

namespace Models.Feedbacks;
public class Feedback : BaseModel
{
    public int SenderId { get; set; }
    public int TargetId { get; set; }
    public FeedbackTargetType TargetType { get; set; }
    public int Rating { get; set; }
    public string Content { get; set; }
    public string Response { get; set; }

    // Navigation Property
    public Account Sender { get; set; } // 1-to-1 with Account (sender)
}
