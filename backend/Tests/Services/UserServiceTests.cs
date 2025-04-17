using AutoMapper;
using BLL.Services;
using Common.DTOs.UserDtos;
using Common.Pagination;
using Common.ViewModels.BusinessVMs;
using Common.ViewModels.UserVMs;
using DAL.Repository.Core;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Models.Accounts;
using Models.Businesses;
using Models.Categories;
using Models.Cities;
using Models.Core;
using Models.Feedbacks;
using System.Linq.Expressions;
using Utility.Constants;

namespace Tests.Services;
public class UserServiceTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _unitOfWork = A.Fake<IUnitOfWork>();
        _mapper = A.Fake<IMapper>();
        _logger = A.Fake<ILogger<UserService>>();
        _userService = new UserService(_unitOfWork, _mapper, _logger);
    }

    #region GetUsers

    //[Fact]
    //public async Task GetUsers_ValidParams_ReturnsPagedUsers()
    //{
    //    // Arrange
    //    var param = new FilterUserParams
    //    {
    //        CurrentUserId = 1,
    //        PageNumber = 1,
    //        PageSize = 10
    //    };

    //    var accounts = new List<Account>
    //{
    //    new Account { Id = 2, Role = Role.USER },
    //    new Account { Id = 3, Role = Role.USER }
    //};

    //    var friendships = new List<Friendship>
    //{
    //    new Friendship { RequesterId = 1, RequesteeId = 2, Status = FriendshipStatus.ACCEPTED }
    //};

    //    var userVMs = new List<UserVM>
    //{
    //    new UserVM { Id = 2, FriendStatus = FriendshipStatus.ACCEPTED, IsRequester = true },
    //    new UserVM { Id = 3, FriendStatus = FriendshipStatus.NONE, IsRequester = false }
    //};

    //    // Create a paged list with the correct total count
    //    var pagedUsers = new PagedList<UserVM>(userVMs, userVMs.Count, param.PageNumber ?? 1, param.PageSize ?? 10);
    //    var expectedResult = new PagedUsersVM
    //    {
    //        Users = pagedUsers,
    //        Pagination = pagedUsers.PaginationData
    //    };

    //    A.CallTo(() => _unitOfWork.Accounts.GetUsersWithinDistanceAsync(param))
    //        .Returns(accounts);
    //    A.CallTo(() => _unitOfWork.Friendships.GetAllAsync(A<Expression<Func<Friendship, bool>>>.Ignored, null))
    //        .Returns(friendships);
    //    A.CallTo(() => _mapper.Map<IEnumerable<UserVM>>(accounts, A<Action<IMappingOperationOptions>>.Ignored))
    //        .Returns(userVMs);

    //    // Act
    //    var result = await _userService.GetUsers(param);

    //    // Assert
    //    result.Should().NotBeNull();
    //    result.IsSuccess.Should().BeTrue();
    //    result.Data.Users.Count().Should().Be(2);
    //    result.Data.Pagination.TotalCount.Should().Be(2);
    //    result.Data.Pagination.TotalPages.Should().Be(1);
    //}

    [Fact]
    public async Task GetUsers_ExceptionThrown_ReturnsFailure()
    {
        // Arrange
        var param = new FilterUserParams();
        A.CallTo(() => _unitOfWork.Accounts.GetUsersWithinDistanceAsync(param))
            .Throws(new Exception());

        // Act
        var result = await _userService.GetUsers(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.UnexpectedError);
    }

    #endregion

    #region GetBusinesses

    [Fact]
    public async Task GetBusinesses_ValidParams_ReturnsPagedBusinesses()
    {
        // Arrange
        var param = new FilterBusinessParams
        {
            SearchTerm = "test",
            CityId = 1,
            CategoryId = 2,
            PageNumber = 1,
            PageSize = 10
        };

        var businesses = new List<Account>
        {
            new Account { Id = 1, Role = Role.BUSINESS, CityId = 1 },
            new Account { Id = 2, Role = Role.BUSINESS, CityId = 1 }
        };

        var businessVMs = new List<BusinessVM>
        {
            new BusinessVM { Id = 1 },
            new BusinessVM { Id = 2 }
        };

        var pagedBusinesses = PagedList<BusinessVM>.ToPagedList(businessVMs, param.PageNumber, param.PageSize);
        var expectedResult = new PagedBusinessesVM
        {
            Businesses = pagedBusinesses,
            Pagination = pagedBusinesses.PaginationData
        };

        A.CallTo(() => _unitOfWork.Accounts.GetAllAsync(A<Expression<Func<Account, bool>>>.Ignored, "City,Business"))
            .Returns(businesses);
        A.CallTo(() => _unitOfWork.Feedbacks.GetAllAsync(A<Expression<Func<Feedback, bool>>>.Ignored, null))
            .Returns(new List<Feedback>());

        // Setup mapper to return different values for each call
        A.CallTo(() => _mapper.Map<BusinessVM>(A<Account>.That.Matches(a => a.Id == 1)))
            .Returns(businessVMs[0]);
        A.CallTo(() => _mapper.Map<BusinessVM>(A<Account>.That.Matches(a => a.Id == 2)))
            .Returns(businessVMs[1]);

        // Act
        var result = await _userService.GetBusinesses(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetBusinesses_WithFeedback_ReturnsAverageRating()
    {
        // Arrange
        var param = new FilterBusinessParams();
        var businesses = new List<Account> { new Account { Id = 1, Role = Role.BUSINESS } };
        var feedbacks = new List<Feedback>
    {
        new Feedback { TargetId = 1, TargetType = FeedbackTargetType.BUSINESS, Rating = 4 },
        new Feedback { TargetId = 1, TargetType = FeedbackTargetType.BUSINESS, Rating = 5 }
    };

        A.CallTo(() => _unitOfWork.Accounts.GetAllAsync(A<Expression<Func<Account, bool>>>.Ignored, "City,Business"))
            .Returns(businesses);
        A.CallTo(() => _unitOfWork.Feedbacks.GetAllAsync(A<Expression<Func<Feedback, bool>>>.Ignored, null))
            .Returns(feedbacks);

        // Return BusinessVM with calculated average
        A.CallTo(() => _mapper.Map<BusinessVM>(A<Account>.Ignored))
            .Returns(new BusinessVM
            {
                Id = 1,
                AverageRating = (int)Math.Round(feedbacks.Average(f => f.Rating)), // Should be 5
                TotalFeedback = feedbacks.Count
            });

        // Act
        var result = await _userService.GetBusinesses(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Businesses.First().AverageRating.Should().Be(4);
        result.Data.Businesses.First().TotalFeedback.Should().Be(2);
    }

    [Fact]
    public async Task GetBusinesses_ExceptionThrown_ReturnsFailure()
    {
        // Arrange
        var param = new FilterBusinessParams();
        A.CallTo(() => _unitOfWork.Accounts.GetAllAsync(A<Expression<Func<Account, bool>>>.Ignored, "City,Business"))
            .Throws(new Exception());

        // Act
        var result = await _userService.GetBusinesses(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.UnexpectedError);
    }

    #endregion

    #region GetUserById

    [Fact]
    public async Task GetUserById_ValidId_ReturnsUserDetail()
    {
        // Arrange
        int id = 1;
        var account = new Account
        {
            Id = id,
            Role = Role.USER,
            City = new City { Name = "Test City" },
            Categories = new List<Category> { new Category { Name = "Test" } }
        };

        var expectedResult = new UserDetailVM { Id = id, City = "Test City" };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(id, "City,Categories"))
            .Returns(account);
        A.CallTo(() => _mapper.Map<UserDetailVM>(account))
            .Returns(expectedResult);

        // Act
        var result = await _userService.GetUserById(id);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetUserById_InvalidId_ReturnsNotFound()
    {
        // Arrange
        int id = 999;
        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(id, "City,Categories"))
            .Returns((Account?)null);

        // Act
        var result = await _userService.GetUserById(id);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.UserNotFound);
    }

    [Fact]
    public async Task GetUserById_WrongRole_ReturnsNotFound()
    {
        // Arrange
        int id = 1;
        var account = new Account { Id = id, Role = Role.BUSINESS };
        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(id, "City,Categories"))
            .Returns(account);

        // Act
        var result = await _userService.GetUserById(id);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.UserNotFound);
    }

    #endregion

    #region GetBusinessById

    [Fact]
    public async Task GetBusinessById_ValidId_ReturnsBusinessDetail()
    {
        // Arrange
        int id = 1;
        var account = new Account
        {
            Id = id,
            Role = Role.BUSINESS,
            Business = new Business(),
            City = new City { Name = "Test City" },
            Categories = new List<Category> { new Category { Name = "Test" } }
        };

        var expectedResult = new BusinessDetailVM { Id = id, City = "Test City" };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(id, "City,Categories,Business"))
            .Returns(account);
        A.CallTo(() => _mapper.Map<BusinessDetailVM>(account))
            .Returns(expectedResult);

        // Act
        var result = await _userService.GetBusinessById(id);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetBusinessById_InvalidId_ReturnsNotFound()
    {
        // Arrange
        int id = 999;
        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(id, "City,Categories,Business"))
            .Returns((Account?)null);

        // Act
        var result = await _userService.GetBusinessById(id);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.BusinessNotFound);
    }

    [Fact]
    public async Task GetBusinessById_WrongRole_ReturnsNotFound()
    {
        // Arrange
        int id = 1;
        var account = new Account { Id = id, Role = Role.USER };
        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(id, "City,Categories,Business"))
            .Returns(account);

        // Act
        var result = await _userService.GetBusinessById(id);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.BusinessNotFound);
    }

    #endregion

}
