using Models.Friendships;

namespace Common.ViewModels.FriendshipVMs;
public class FriendshipVM
{
    public int Id { get; set; }
    public int OtherUserId { get; set; }
    public string OtherUserName { get; set; }
    public string OtherUserAvatar { get; set; }
    // Indicates whether the current user sent the request
    public bool IsRequester { get; set; }
    public FriendshipStatus Status { get; set; }
}
