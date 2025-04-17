using Common.DTOs.FriendshipDtos;
using Common.Responses;
using Common.ViewModels.FriendshipVMs;

namespace BLL.Interfaces;
public interface IFriendshipService
{
    Task<ApiResponse<PagedFriendshipsVM>> GetFriendships(FilterFriendshipParams param);
    Task<ApiResponse> AddFriend(FriendRequestParams param);
    Task<ApiResponse> CancelFriend(FriendRequestParams param);
    Task<ApiResponse> RemoveFriend(FriendRequestParams param);
    Task<ApiResponse> AcceptFriend(FriendRequestParams param);
    Task<ApiResponse<FriendshipVM>> GetFriendship(FriendRequestParams param);
}
