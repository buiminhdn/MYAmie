using AutoMapper;
using BLL.Services;
using Common.DTOs.FeedbackDtos;
using Common.Pagination;
using Common.ViewModels.FeedbackVMs;
using DAL.Repository.Core;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Models.Accounts;
using Models.Feedbacks;
using Models.Places;
using MYAmie.Common.DTOs.FeedbackDtos;
using System.Linq.Expressions;
using Utility;
using Utility.Constants;

namespace Tests.Services;
public class FeedbackServiceTests
{
    private readonly IUnitOfWork _mockUnitOfWork;
    private readonly IMapper _mockMapper;
    private readonly ILogger<FeedbackService> _mockLogger;
    private readonly FeedbackService _feedbackService;

    public FeedbackServiceTests()
    {
        _mockUnitOfWork = A.Fake<IUnitOfWork>();
        _mockMapper = A.Fake<IMapper>();
        _mockLogger = A.Fake<ILogger<FeedbackService>>();
        _feedbackService = new FeedbackService(_mockUnitOfWork, _mockMapper, _mockLogger);
    }

    [Fact]
    public async Task GetAllFeedbacks_WhenValid_ReturnsPagedFeedbacksVM()
    {
        // Arrange
        var targetId = 1;
        var feedbacks = new List<Feedback>
            {
                new Feedback { Id = 1, TargetId = targetId, Rating = 5, Content = "Great!", Sender = new Account { Id = 2, FirstName = "John", LastName = "Doe" } },
                new Feedback { Id = 2, TargetId = targetId, Rating = 4, Content = "Good", Sender = new Account { Id = 3, FirstName = "Jane", LastName = "Doe" } }
            };

        var param = new FilterFeedbackParams
        {
            Id = targetId,
            PageNumber = 1,
            PageSize = 10
        };

        A.CallTo(() => _mockUnitOfWork.Feedbacks.GetAllAsync(
            A<Expression<Func<Feedback, bool>>>.Ignored,
            A<string>.Ignored))
            .Returns(feedbacks);

        var feedbackVMs = new List<FeedbackVM>
            {
                new FeedbackVM { Id = 1, Rating = 5, Content = "Great!", Name = "John Doe" },
                new FeedbackVM { Id = 2, Rating = 4, Content = "Good", Name = "Jane Doe" }
            };

        A.CallTo(() => _mockMapper.Map<IEnumerable<FeedbackVM>>(feedbacks))
            .Returns(feedbackVMs);

        var pagedFeedbacks = PagedList<FeedbackVM>.ToPagedList(feedbackVMs, param.PageNumber, param.PageSize);

        var pagedFeedbacksVM = new PagedFeedbacksVM
        {
            TotalFeedback = feedbacks.Count,
            AverageRating = Math.Round(feedbacks.Average(f => f.Rating), 1),
            Feedbacks = pagedFeedbacks,
            Pagination = pagedFeedbacks.PaginationData
        };

        // Act
        var result = await _feedbackService.GetFeedbacks(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.TotalFeedback.Should().Be(2);
        result.Data.AverageRating.Should().Be(4.5);
        result.Data.Feedbacks.Should().HaveCount(2);
        result.Data.Pagination.Should().NotBeNull();
        A.CallTo(() => _mockUnitOfWork.Feedbacks.GetAllAsync(
            A<Expression<Func<Feedback, bool>>>.Ignored,
            A<string>.Ignored)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task AddFeedback_WhenValid_ReturnsSuccess()
    {
        // Arrange
        var currentUserId = 1;
        var targetId = 2;
        var param = new AddFeedbackParams
        {
            CurrentUserId = currentUserId,
            TargetId = targetId,
            TargetType = FeedbackTargetType.PLACE,
            Rating = 5,
            Content = "Great!"
        };

        A.CallTo(() => _mockUnitOfWork.Feedbacks.GetAsync(
            A<Expression<Func<Feedback, bool>>>.Ignored,
            A<string>.Ignored))
            .Returns<Feedback?>(null);

        A.CallTo(() => _mockUnitOfWork.Places.GetByIdAsync(targetId, A<string>.Ignored))
            .Returns(new Place { Id = targetId });

        A.CallTo(() => _mockUnitOfWork.SaveAsync())
            .Returns(1);

        // Act
        var result = await _feedbackService.AddFeedback(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be(ResponseMessages.FeedbackAdded);
        A.CallTo(() => _mockUnitOfWork.Feedbacks.GetAsync(
            A<Expression<Func<Feedback, bool>>>.Ignored,
            A<string>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _mockUnitOfWork.SaveAsync()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task AddFeedback_WhenFeedbackAlreadyExists_ReturnsFailure()
    {
        // Arrange
        var currentUserId = 1;
        var targetId = 2;
        var param = new AddFeedbackParams
        {
            CurrentUserId = currentUserId,
            TargetId = targetId,
            TargetType = FeedbackTargetType.PLACE,
            Rating = 5,
            Content = "Great!"
        };

        A.CallTo(() => _mockUnitOfWork.Feedbacks.GetAsync(
            A<Expression<Func<Feedback, bool>>>.Ignored,
            A<string>.Ignored))
            .Returns(new Feedback());

        // Act
        var result = await _feedbackService.AddFeedback(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.FeedbackAlreadyExists);
        A.CallTo(() => _mockUnitOfWork.Feedbacks.GetAsync(
            A<Expression<Func<Feedback, bool>>>.Ignored,
            A<string>.Ignored)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeleteFeedback_WhenValid_ReturnsSuccess()
    {
        // Arrange
        var feedbackId = 1;
        var currentUserId = 1;
        var feedback = new Feedback { Id = feedbackId, SenderId = currentUserId };

        var param = new DeleteFeedbackParams
        {
            Id = feedbackId,
            CurrentUserId = currentUserId
        };

        A.CallTo(() => _mockUnitOfWork.Feedbacks.GetByIdAsync(feedbackId, A<string>.Ignored))
            .Returns(feedback);

        A.CallTo(() => _mockUnitOfWork.SaveAsync())
            .Returns(1);

        // Act
        var result = await _feedbackService.DeleteFeedback(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be(ResponseMessages.FeedbackDeleted);
        A.CallTo(() => _mockUnitOfWork.Feedbacks.GetByIdAsync(feedbackId, A<string>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _mockUnitOfWork.SaveAsync()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeleteFeedback_WhenFeedbackNotFound_ReturnsFailure()
    {
        // Arrange
        var feedbackId = 1;
        var currentUserId = 1;

        var param = new DeleteFeedbackParams
        {
            Id = feedbackId,
            CurrentUserId = currentUserId
        };

        A.CallTo(() => _mockUnitOfWork.Feedbacks.GetByIdAsync(feedbackId, A<string>.Ignored))
            .Returns<Feedback?>(null);

        // Act
        var result = await _feedbackService.DeleteFeedback(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.FeedbackNotFound);
        A.CallTo(() => _mockUnitOfWork.Feedbacks.GetByIdAsync(feedbackId, A<string>.Ignored)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task ResponseFeedback_WhenValid_ReturnsSuccess()
    {
        // Arrange
        var feedbackId = 1;
        var currentUserId = 2;
        var feedback = new Feedback { Id = feedbackId, TargetId = currentUserId };

        var param = new ResponseFeedbackParams
        {
            Id = feedbackId,
            CurrentUserId = currentUserId,
            Message = "Thank you!"
        };

        A.CallTo(() => _mockUnitOfWork.Feedbacks.GetByIdAsync(feedbackId, A<string>.Ignored))
            .Returns(feedback);

        A.CallTo(() => _mockUnitOfWork.SaveAsync())
            .Returns(1);

        // Act
        var result = await _feedbackService.ResponseFeedback(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be(ResponseMessages.FeedbackResponded);
        A.CallTo(() => _mockUnitOfWork.Feedbacks.GetByIdAsync(feedbackId, A<string>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _mockUnitOfWork.SaveAsync()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task ResponseFeedback_WhenFeedbackNotFound_ReturnsFailure()
    {
        // Arrange
        var feedbackId = 1;
        var currentUserId = 2;

        var param = new ResponseFeedbackParams
        {
            Id = feedbackId,
            CurrentUserId = currentUserId,
            Message = "Thank you!"
        };

        A.CallTo(() => _mockUnitOfWork.Feedbacks.GetByIdAsync(feedbackId, A<string>.Ignored))
            .Returns<Feedback?>(null);

        // Act
        var result = await _feedbackService.ResponseFeedback(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.FeedbackNotFound);
        A.CallTo(() => _mockUnitOfWork.Feedbacks.GetByIdAsync(feedbackId, A<string>.Ignored)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateFeedback_WhenValid_ReturnsSuccess()
    {
        // Arrange
        var feedbackId = 1;
        var currentUserId = 1;
        var feedback = new Feedback { Id = feedbackId, SenderId = currentUserId, CreatedDate = DateTimeUtils.TimeInEpoch() };

        var param = new UpdateFeedbackParams
        {
            Id = feedbackId,
            CurrentUserId = currentUserId,
            Rating = 5,
            Content = "Updated content"
        };

        A.CallTo(() => _mockUnitOfWork.Feedbacks.GetByIdAsync(feedbackId, A<string>.Ignored))
            .Returns(feedback);

        A.CallTo(() => _mockUnitOfWork.SaveAsync())
            .Returns(1);

        // Act
        var result = await _feedbackService.UpdateFeedback(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be(ResponseMessages.FeedbackUpdated);
        A.CallTo(() => _mockUnitOfWork.Feedbacks.GetByIdAsync(feedbackId, A<string>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _mockUnitOfWork.SaveAsync()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateFeedback_WhenFeedbackNotFound_ReturnsFailure()
    {
        // Arrange
        var feedbackId = 1;
        var currentUserId = 1;

        var param = new UpdateFeedbackParams
        {
            Id = feedbackId,
            CurrentUserId = currentUserId,
            Rating = 5,
            Content = "Updated content"
        };

        A.CallTo(() => _mockUnitOfWork.Feedbacks.GetByIdAsync(feedbackId, A<string>.Ignored))
            .Returns<Feedback?>(null);

        // Act
        var result = await _feedbackService.UpdateFeedback(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.FeedbackNotFound);
        A.CallTo(() => _mockUnitOfWork.Feedbacks.GetByIdAsync(feedbackId, A<string>.Ignored)).MustHaveHappenedOnceExactly();
    }
}
