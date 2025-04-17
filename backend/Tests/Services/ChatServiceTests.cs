using BLL.Services;
using Common.DTOs.ChatDtos;
using Common.ViewModels.ChatVMs;
using DAL.Repository.Core;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Models.Accounts;
using Utility;
using Utility.Constants;

namespace Tests.Services;
public class ChatServiceTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ChatService> _logger;
    private readonly ChatService _chatService;

    public ChatServiceTests()
    {
        _unitOfWork = A.Fake<IUnitOfWork>();
        _logger = A.Fake<ILogger<ChatService>>();
        _chatService = new ChatService(_unitOfWork, _logger);
    }

    [Fact]
    public async Task GetConversations_ValidParams_ReturnsPagedConversations()
    {
        // Arrange
        var param = new PagedConversationParams
        {
            CurrentUserId = 1,
            PageNumber = 1
        };

        var account = new Account
        {
            Id = 1,
            IsEmailVerified = true
        };

        var conversationVMs = new List<ConversationVM>
    {
        new ConversationVM { Id = 1, SenderId = 1, Content = "Hello", SentAt = DateTimeUtils.EpochToTimeString(DateTimeUtils.TimeInEpoch()) },
        new ConversationVM { Id = 2, SenderId = 2, Content = "Hi", SentAt = DateTimeUtils.EpochToTimeString(DateTimeUtils.TimeInEpoch()) }
    };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(param.CurrentUserId, A<string>._))
            .Returns(account);

        A.CallTo(() => _unitOfWork.Messages.GetConversations(param.CurrentUserId))
            .Returns(Task.FromResult<IEnumerable<ConversationVM>>(conversationVMs));

        // Act
        var result = await _chatService.GetConversations(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Conversations.Should().HaveCount(2);
        result.Data.HasMore.Should().BeFalse();
        result.Data.PageNumber.Should().Be(1);
    }

    [Fact]
    public async Task GetConversations_AccountNotFound_ReturnsFailure()
    {
        // Arrange
        var param = new PagedConversationParams
        {
            CurrentUserId = 999, // Invalid user ID
            PageNumber = 1
        };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(param.CurrentUserId, A<string>._))
            .Returns<Account?>(null);

        // Act
        var result = await _chatService.GetConversations(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.AccountNotFound);
    }

    [Fact]
    public async Task GetConversations_AccountNotVerified_ReturnsFailure()
    {
        // Arrange
        var param = new PagedConversationParams
        {
            CurrentUserId = 1,
            PageNumber = 1
        };

        var account = new Account
        {
            Id = 1,
            IsEmailVerified = false // Account not verified
        };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(param.CurrentUserId, A<string>._))
            .Returns(account);

        // Act
        var result = await _chatService.GetConversations(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.AccountNotVerified);
    }

    [Fact]
    public async Task GetMessages_ValidParams_ReturnsPagedMessages()
    {
        // Arrange
        var param = new PagedMessageParams
        {
            CurrentUserId = 1,
            OtherUserId = 2,
            PageNumber = 1
        };

        var currentUser = new Account { Id = 1, IsEmailVerified = true };
        var otherUser = new Account { Id = 2, IsEmailVerified = true };

        var messages = new List<MessageVM>
    {
        new MessageVM { Id = 1, SenderId = 1, ReceiverId = 2, Content = "Hello" },
        new MessageVM { Id = 2, SenderId = 2, ReceiverId = 1, Content = "Hi" }
    };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(param.CurrentUserId, A<string>._))
            .Returns(currentUser);

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(param.OtherUserId, A<string>._))
            .Returns(otherUser);

        A.CallTo(() => _unitOfWork.Messages.GetMessages(param.CurrentUserId, param.OtherUserId))
                .Returns(Task.FromResult<IEnumerable<MessageVM>>(messages));

        // Act
        var result = await _chatService.GetMessages(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Messages.Should().HaveCount(2);
        result.Data.HasMore.Should().BeFalse();
        result.Data.PageNumber.Should().Be(1);
    }

    [Fact]
    public async Task GetMessages_AccountNotFound_ReturnsFailure()
    {
        // Arrange
        var param = new PagedMessageParams
        {
            CurrentUserId = 1,
            OtherUserId = 2,
            PageNumber = 1
        };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(param.CurrentUserId, A<string>._))
            .Returns<Account?>(null);

        // Act
        var result = await _chatService.GetMessages(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.AccountNotFound);
    }

    [Fact]
    public async Task GetMessages_AccountNotVerified_ReturnsFailure()
    {
        // Arrange
        var param = new PagedMessageParams
        {
            CurrentUserId = 1,
            OtherUserId = 2,
            PageNumber = 1
        };

        var currentUser = new Account { Id = 1, IsEmailVerified = false }; // Account not verified
        var otherUser = new Account { Id = 2, IsEmailVerified = true };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(param.CurrentUserId, A<string>._))
            .Returns(currentUser);

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(param.OtherUserId, A<string>._))
            .Returns(otherUser);

        // Act
        var result = await _chatService.GetMessages(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.AccountNotVerified);
    }
}
