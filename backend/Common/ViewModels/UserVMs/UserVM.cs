
using Models.Friendships;

namespace Common.ViewModels.UserVMs;
public class UserVM
{
    public int Id { get; set; }
    public string Avatar { get; set; }
    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public IEnumerable<string> Characteristics { get; set; }
    public double Distance { get; set; }
    public string City { get; set; }
    public FriendshipStatus FriendStatus { get; set; } = FriendshipStatus.NONE;
    public bool IsRequester { get; set; }
}
