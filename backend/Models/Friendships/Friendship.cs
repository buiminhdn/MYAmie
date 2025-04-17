using Models.Accounts;
using Models.Core;

namespace Models.Friendships;
public class Friendship : BaseModel
{
    public int RequesterId { get; set; }
    public int RequesteeId { get; set; }
    public FriendshipStatus Status { get; set; } = FriendshipStatus.PENDING;

    // Navigation Properties
    public Account Requester { get; set; } // 1-to-1 with Account (requester)
    public Account Requestee { get; set; } // 1-to-1 with Account (requestee)
}
