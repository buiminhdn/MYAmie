using Common.DTOs.Core;

namespace Common.DTOs.FriendshipDtos;
public class FriendRequestParams : BaseParams
{
    public int OtherUserId { get; set; }
}

