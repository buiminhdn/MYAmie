using BLL.Interfaces;
using Common.DTOs.FriendshipDtos;
using Common.Pagination;
using Common.Responses;
using Common.ViewModels.FriendshipVMs;
using DAL.Repository.Core;
using Microsoft.Extensions.Logging;
using Models.Friendships;
using Utility;
using Utility.Constants;

namespace BLL.Services;
public class FriendshipService(IUnitOfWork unitOfWork, ILogger<FriendshipService> logger) : IFriendshipService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<FriendshipService> _logger = logger;

    public async Task<ApiResponse<PagedFriendshipsVM>> GetFriendships(FilterFriendshipParams param)
    {
        try
        {
            var friendships = await _unitOfWork.Friendships.GetAllAsync(f =>
                  f.RequesterId == param.CurrentUserId || f.RequesteeId == param.CurrentUserId,
                  "Requester,Requestee");

            var friendshipVMs = friendships.Select(f => new FriendshipVM
            {
                Id = f.Id,
                OtherUserId = f.RequesterId == param.CurrentUserId ? f.RequesteeId : f.RequesterId,
                OtherUserName = f.RequesterId == param.CurrentUserId ?
                    $"{f.Requestee.LastName} {f.Requestee.FirstName}" :
                    $"{f.Requester.LastName} {f.Requester.FirstName}",
                OtherUserAvatar = f.RequesterId == param.CurrentUserId ? f.Requestee.Avatar : f.Requester.Avatar,
                IsRequester = f.RequesterId == param.CurrentUserId,
                Status = f.Status
            });

            var pagedFriendships = PagedList<FriendshipVM>.ToPagedList(friendshipVMs, param.PageNumber, param.PageSize);

            var pagedFriendshipsVM = new PagedFriendshipsVM
            {
                Friends = pagedFriendships,
                Pagination = pagedFriendships.PaginationData
            };

            return ApiResponse<PagedFriendshipsVM>.Success(pagedFriendshipsVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching friendships for UserId {userId}", param.CurrentUserId);
            return ApiResponse<PagedFriendshipsVM>.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse> AcceptFriend(FriendRequestParams param)
    {
        try
        {
            var friendShip = await _unitOfWork.Friendships.GetAsync(f =>
                           f.RequesterId == param.OtherUserId && f.RequesteeId == param.CurrentUserId && f.Status == FriendshipStatus.PENDING);

            if (friendShip == null)
                return ApiResponse.Failure(ResponseMessages.FriendshipNotFound);

            friendShip.Status = FriendshipStatus.ACCEPTED;
            friendShip.UpdatedBy = param.CurrentUserId;
            friendShip.UpdatedDate = DateTimeUtils.TimeInEpoch();

            await _unitOfWork.Friendships.UpdateAsync(friendShip);

            return await _unitOfWork.SaveAsync() > 0
               ? ApiResponse.Success(ResponseMessages.FriendshipAccepted)
               : ApiResponse.Failure(ResponseMessages.FriendshipAcceptFailed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error accepting friend request. CurrentUserId: {CurrentUserId}, OtherUserId: {OtherUserId}", param.CurrentUserId, param.OtherUserId);
            return ApiResponse.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse> AddFriend(FriendRequestParams param)
    {
        try
        {
            var friend = await _unitOfWork.Accounts.GetByIdAsync(param.OtherUserId);

            if (friend == null)
                return ApiResponse.Failure(ResponseMessages.AccountNotFound);

            var existingFriendship = await _unitOfWork.Friendships.IsFriend(param.CurrentUserId, param.OtherUserId);

            if (existingFriendship)
                return ApiResponse.Failure(ResponseMessages.FriendshipAlreadyExists);

            var friendship = new Friendship
            {
                RequesterId = param.CurrentUserId,
                RequesteeId = param.OtherUserId,
                Status = FriendshipStatus.PENDING,
                CreatedBy = param.CurrentUserId,
                CreatedDate = DateTimeUtils.TimeInEpoch()
            };

            await _unitOfWork.Friendships.AddAsync(friendship);

            return await _unitOfWork.SaveAsync() > 0
               ? ApiResponse.Success(ResponseMessages.FriendshipRequestSent)
               : ApiResponse.Failure(ResponseMessages.FriendshipRequestFailed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending friend request. CurrentUserId: {CurrentUserId}, OtherUserId: {OtherUserId}", param.CurrentUserId, param.OtherUserId);
            return ApiResponse.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse> CancelFriend(FriendRequestParams param)
    {
        try
        {
            var friendShip = await _unitOfWork.Friendships.GetAsync(f =>
                           f.RequesterId == param.CurrentUserId && f.RequesteeId == param.OtherUserId && f.Status == FriendshipStatus.PENDING);

            if (friendShip == null)
                return ApiResponse.Failure(ResponseMessages.FriendshipNotFound);

            await _unitOfWork.Friendships.DeleteAsync(friendShip);

            return await _unitOfWork.SaveAsync() > 0
               ? ApiResponse.Success(ResponseMessages.FriendshipRequestCancelled)
               : ApiResponse.Failure(ResponseMessages.FriendshipCancelFailed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error canceling friend request. CurrentUserId: {CurrentUserId}, OtherUserId: {OtherUserId}", param.CurrentUserId, param.OtherUserId);
            return ApiResponse.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse> RemoveFriend(FriendRequestParams param)
    {
        try
        {
            var friendship = await _unitOfWork.Friendships.GetAsync(f =>
                ((f.RequesterId == param.CurrentUserId && f.RequesteeId == param.OtherUserId) ||
                 (f.RequesterId == param.OtherUserId && f.RequesteeId == param.CurrentUserId)) &&
                f.Status == FriendshipStatus.ACCEPTED);

            if (friendship == null)
                return ApiResponse.Failure(ResponseMessages.FriendshipNotFound);

            await _unitOfWork.Friendships.DeleteAsync(friendship);

            return await _unitOfWork.SaveAsync() > 0
               ? ApiResponse.Success(ResponseMessages.FriendshipRemoved)
               : ApiResponse.Failure(ResponseMessages.FriendshipRemoveFailed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing friend. CurrentUserId: {CurrentUserId}, OtherUserId: {OtherUserId}", param.CurrentUserId, param.OtherUserId);
            return ApiResponse.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse<FriendshipVM>> GetFriendship(FriendRequestParams param)
    {
        try
        {
            if (param.CurrentUserId <= 0)
            {
                return ApiResponse<FriendshipVM>.Success(CreateEmptyFriendshipVM(param.OtherUserId));
            }

            var friendship = await _unitOfWork.Friendships.GetAsync(f =>
            ((f.RequesterId == param.CurrentUserId && f.RequesteeId == param.OtherUserId) ||
             (f.RequesterId == param.OtherUserId && f.RequesteeId == param.CurrentUserId)),
            includeProperties: "Requester,Requestee");

            if (friendship == null)
            {
                return ApiResponse<FriendshipVM>.Success(CreateEmptyFriendshipVM(param.OtherUserId));
            }

            // Determine who the "other user" is
            var isRequester = friendship.RequesterId == param.CurrentUserId;
            var otherUser = isRequester ? friendship.Requestee : friendship.Requester;

            var friendshipVM = new FriendshipVM
            {
                Id = friendship.Id,
                OtherUserId = otherUser.Id,
                OtherUserName = otherUser.FirstName,
                OtherUserAvatar = otherUser.Avatar,
                IsRequester = isRequester,
                Status = friendship.Status
            };

            return ApiResponse<FriendshipVM>.Success(friendshipVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving friendship. CurrentUserId: {CurrentUserId}, OtherUserId: {OtherUserId}", param.CurrentUserId, param.OtherUserId);
            return ApiResponse<FriendshipVM>.Failure(ResponseMessages.UnexpectedError);
        }
    }

    private static FriendshipVM CreateEmptyFriendshipVM(int otherUserId)
    {
        return new FriendshipVM
        {
            Id = 0,
            OtherUserId = otherUserId,
            OtherUserName = "",
            OtherUserAvatar = "",
            IsRequester = false,
            Status = FriendshipStatus.NONE
        };
    }
}

