using BLL.Services;
using Common.DTOs.FriendshipDtos;
using Common.Pagination;
using Common.ViewModels.FriendshipVMs;
using DAL.Repository.Core;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Models.Accounts;
using Models.Friendships;
using System.Linq.Expressions;
using Utility.Constants;

namespace Tests.Services;
public class FriendshipServiceTests
{
    private readonly IUnitOfWork _mockUnitOfWork;
    private readonly ILogger<FriendshipService> _mockLogger;
    private readonly FriendshipService _friendshipService;

    public FriendshipServiceTests()
    {
        _mockUnitOfWork = A.Fake<IUnitOfWork>();
        _mockLogger = A.Fake<ILogger<FriendshipService>>();
        _friendshipService = new FriendshipService(_mockUnitOfWork, _mockLogger);
    }

    [Fact]
    public async Task GetAll_WhenValid_ReturnsPagedFriendshipsVM()
    {
        // Arrange
        var currentUserId = 1;
        var friendships = new List<Friendship>
            {
                new Friendship
                {
                    Id = 1,
                    RequesterId = currentUserId,
                    RequesteeId = 2,
                    Requester = new Account { Id = currentUserId, FirstName = "John", LastName = "Doe" },
                    Requestee = new Account { Id = 2, FirstName = "Jane", LastName = "Doe", Avatar = "avatar2.jpg" },
                    Status = FriendshipStatus.ACCEPTED
                },
                new Friendship
                {
                    Id = 2,
                    RequesterId = 3,
                    RequesteeId = currentUserId,
                    Requester = new Account { Id = 3, FirstName = "Alice", LastName = "Smith", Avatar = "avatar3.jpg" },
                    Requestee = new Account { Id = currentUserId, FirstName = "John", LastName = "Doe" },
                    Status = FriendshipStatus.PENDING
                }
            };

        var param = new FilterFriendshipParams
        {
            CurrentUserId = currentUserId,
            PageNumber = 1,
            PageSize = 10
        };

        A.CallTo(() => _mockUnitOfWork.Friendships.GetAllAsync(
            A<Expression<Func<Friendship, bool>>>._, A<string>.Ignored))
            .Returns(friendships);

        var pagedFriendships = PagedList<FriendshipVM>.ToPagedList(
            friendships.Select(f => new FriendshipVM
            {
                Id = f.Id,
                OtherUserId = f.RequesterId == currentUserId ? f.RequesteeId : f.RequesterId,
                OtherUserName = f.RequesterId == currentUserId ?
                    $"{f.Requestee.LastName} {f.Requestee.FirstName}" :
                    $"{f.Requester.LastName} {f.Requester.FirstName}",
                OtherUserAvatar = f.RequesterId == currentUserId ? f.Requestee.Avatar : f.Requester.Avatar,
                IsRequester = f.RequesterId == currentUserId,
                Status = f.Status
            }).ToList(),
            param.PageNumber,
            param.PageSize
        );

        // Act
        var result = await _friendshipService.GetFriendships(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Friends.Should().HaveCount(2);
        result.Data.Pagination.Should().NotBeNull();
        A.CallTo(() => _mockUnitOfWork.Friendships.GetAllAsync(
            A<Expression<Func<Friendship, bool>>>._,
            A<string>.Ignored)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task AcceptFriend_WhenValid_ReturnsSuccess()
    {
        // Arrange
        var currentUserId = 1;
        var otherUserId = 2;
        var friendship = new Friendship
        {
            Id = 1,
            RequesterId = otherUserId,
            RequesteeId = currentUserId,
            Status = FriendshipStatus.PENDING
        };

        var param = new FriendRequestParams
        {
            CurrentUserId = currentUserId,
            OtherUserId = otherUserId
        };

        A.CallTo(() => _mockUnitOfWork.Friendships.GetAsync(
             A<Expression<Func<Friendship, bool>>>._, A<string>.Ignored))
            .Returns(friendship);

        A.CallTo(() => _mockUnitOfWork.SaveAsync())
            .Returns(1);

        // Act
        var result = await _friendshipService.AcceptFriend(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be(ResponseMessages.FriendshipAccepted);
        A.CallTo(() => _mockUnitOfWork.Friendships.GetAsync(
            A<Expression<Func<Friendship, bool>>>._, A<string>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _mockUnitOfWork.SaveAsync()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task AcceptFriend_WhenFriendshipNotFound_ReturnsFailure()
    {
        // Arrange
        var currentUserId = 1;
        var otherUserId = 2;

        var param = new FriendRequestParams
        {
            CurrentUserId = currentUserId,
            OtherUserId = otherUserId
        };

        A.CallTo(() => _mockUnitOfWork.Friendships.GetAsync(
            A<Expression<Func<Friendship, bool>>>._, A<string>.Ignored))
            .Returns<Friendship?>(null);

        // Act
        var result = await _friendshipService.AcceptFriend(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.FriendshipNotFound);
        A.CallTo(() => _mockUnitOfWork.Friendships.GetAsync(
            A<Expression<Func<Friendship, bool>>>._, A<string>.Ignored)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task AddFriend_WhenValid_ReturnsSuccess()
    {
        // Arrange
        var currentUserId = 1;
        var otherUserId = 2;
        var friend = new Account { Id = otherUserId };

        var param = new FriendRequestParams
        {
            CurrentUserId = currentUserId,
            OtherUserId = otherUserId
        };

        A.CallTo(() => _mockUnitOfWork.Accounts.GetByIdAsync(otherUserId, A<string>.Ignored))
            .Returns(friend);

        A.CallTo(() => _mockUnitOfWork.Friendships.IsFriend(currentUserId, otherUserId))
            .Returns(false);

        A.CallTo(() => _mockUnitOfWork.SaveAsync())
            .Returns(1);

        // Act
        var result = await _friendshipService.AddFriend(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be(ResponseMessages.FriendshipRequestSent);
        A.CallTo(() => _mockUnitOfWork.Accounts.GetByIdAsync(otherUserId, A<string>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _mockUnitOfWork.Friendships.IsFriend(currentUserId, otherUserId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _mockUnitOfWork.SaveAsync()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task AddFriend_WhenAccountNotFound_ReturnsFailure()
    {
        // Arrange
        var currentUserId = 1;
        var otherUserId = 2;

        var param = new FriendRequestParams
        {
            CurrentUserId = currentUserId,
            OtherUserId = otherUserId
        };

        A.CallTo(() => _mockUnitOfWork.Accounts.GetByIdAsync(otherUserId, null))
            .Returns<Account?>(null);

        // Act
        var result = await _friendshipService.AddFriend(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.AccountNotFound);
        A.CallTo(() => _mockUnitOfWork.Accounts.GetByIdAsync(otherUserId, A<string>.Ignored)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task AddFriend_WhenFriendshipAlreadyExists_ReturnsFailure()
    {
        // Arrange
        var currentUserId = 1;
        var otherUserId = 2;
        var friend = new Account { Id = otherUserId };

        var param = new FriendRequestParams
        {
            CurrentUserId = currentUserId,
            OtherUserId = otherUserId
        };

        A.CallTo(() => _mockUnitOfWork.Accounts.GetByIdAsync(otherUserId, A<string>.Ignored))
            .Returns(friend);

        A.CallTo(() => _mockUnitOfWork.Friendships.IsFriend(currentUserId, otherUserId))
            .Returns(true);

        // Act
        var result = await _friendshipService.AddFriend(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.FriendshipAlreadyExists);
        A.CallTo(() => _mockUnitOfWork.Accounts.GetByIdAsync(otherUserId, A<string>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _mockUnitOfWork.Friendships.IsFriend(currentUserId, otherUserId)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task CancelFriend_WhenValid_ReturnsSuccess()
    {
        // Arrange
        var currentUserId = 1;
        var otherUserId = 2;
        var friendship = new Friendship
        {
            Id = 1,
            RequesterId = currentUserId,
            RequesteeId = otherUserId,
            Status = FriendshipStatus.PENDING
        };

        var param = new FriendRequestParams
        {
            CurrentUserId = currentUserId,
            OtherUserId = otherUserId
        };

        A.CallTo(() => _mockUnitOfWork.Friendships.GetAsync(
            A<Expression<Func<Friendship, bool>>>._, A<string>.Ignored))
            .Returns(friendship);

        A.CallTo(() => _mockUnitOfWork.SaveAsync())
            .Returns(1);

        // Act
        var result = await _friendshipService.CancelFriend(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be(ResponseMessages.FriendshipRequestCancelled);
        A.CallTo(() => _mockUnitOfWork.Friendships.GetAsync(
            A<Expression<Func<Friendship, bool>>>._, A<string>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _mockUnitOfWork.SaveAsync()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task CancelFriend_WhenFriendshipNotFound_ReturnsFailure()
    {
        // Arrange
        var currentUserId = 1;
        var otherUserId = 2;

        var param = new FriendRequestParams
        {
            CurrentUserId = currentUserId,
            OtherUserId = otherUserId
        };

        A.CallTo(() => _mockUnitOfWork.Friendships.GetAsync(
            A<Expression<Func<Friendship, bool>>>._, A<string>.Ignored))
            .Returns<Friendship?>(null);

        // Act
        var result = await _friendshipService.CancelFriend(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.FriendshipNotFound);
        A.CallTo(() => _mockUnitOfWork.Friendships.GetAsync(
            A<Expression<Func<Friendship, bool>>>._, A<string>.Ignored)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task RemoveFriend_WhenValid_ReturnsSuccess()
    {
        // Arrange
        var currentUserId = 1;
        var otherUserId = 2;
        var friendship = new Friendship
        {
            Id = 1,
            RequesterId = currentUserId,
            RequesteeId = otherUserId,
            Status = FriendshipStatus.ACCEPTED
        };

        var param = new FriendRequestParams
        {
            CurrentUserId = currentUserId,
            OtherUserId = otherUserId
        };

        A.CallTo(() => _mockUnitOfWork.Friendships.GetAsync(
            A<Expression<Func<Friendship, bool>>>._, A<string>.Ignored))
            .Returns(friendship);

        A.CallTo(() => _mockUnitOfWork.SaveAsync())
            .Returns(1);

        // Act
        var result = await _friendshipService.RemoveFriend(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be(ResponseMessages.FriendshipRemoved);
        A.CallTo(() => _mockUnitOfWork.Friendships.GetAsync(
            A<Expression<Func<Friendship, bool>>>._, A<string>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _mockUnitOfWork.SaveAsync()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task RemoveFriend_WhenFriendshipNotFound_ReturnsFailure()
    {
        // Arrange
        var currentUserId = 1;
        var otherUserId = 2;

        var param = new FriendRequestParams
        {
            CurrentUserId = currentUserId,
            OtherUserId = otherUserId
        };

        A.CallTo(() => _mockUnitOfWork.Friendships.GetAsync(
            A<Expression<Func<Friendship, bool>>>._, A<string>.Ignored))
            .Returns<Friendship?>(null);

        // Act
        var result = await _friendshipService.RemoveFriend(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.FriendshipNotFound);
        A.CallTo(() => _mockUnitOfWork.Friendships.GetAsync(
            A<Expression<Func<Friendship, bool>>>._, A<string>.Ignored)).MustHaveHappenedOnceExactly();
    }
}
