using AutoMapper;
using BLL.Services;
using Common.DTOs.AdminUserDtos;
using Common.ViewModels.AdminUserVMs;
using DAL.Repository.Core;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Models.Accounts;
using Models.Core;
using System.Linq.Expressions;
using Utility.Constants;

namespace Tests.Services;
public class AdminUserServiceTests
{
    private readonly IUnitOfWork _mockUnitOfWork;
    private readonly IMapper _mockMapper;
    private readonly ILogger<AdminUserService> _mockLogger;
    private readonly AdminUserService _adminUserService;

    public AdminUserServiceTests()
    {
        _mockUnitOfWork = A.Fake<IUnitOfWork>();
        _mockMapper = A.Fake<IMapper>();
        _mockLogger = A.Fake<ILogger<AdminUserService>>();
        _adminUserService = new AdminUserService(_mockUnitOfWork, _mockMapper, _mockLogger);
    }

    [Fact]
    public async Task GetAllUsersByAdmin_WhenValid_ReturnsPagedAdminUsersVM()
    {
        // Arrange
        var param = new AdminUserParams
        {
            PageNumber = 1,
            PageSize = 10,
            Status = AccountStatus.ACTIVATED,
            Role = Role.USER,
            SearchTerm = "test"
        };

        var users = new List<Account>
            {
                new Account { Id = 1, Role = Role.USER, Status = AccountStatus.ACTIVATED, CityId = 1, NormalizedInfo = "test" },
                new Account { Id = 2, Role = Role.USER, Status = AccountStatus.ACTIVATED, CityId = 1, NormalizedInfo = "test" }
            };

        var userVMs = new List<AdminUserVM>
            {
                new AdminUserVM { Id = 1 },
                new AdminUserVM { Id = 2 }
            };

        A.CallTo(() => _mockUnitOfWork.Accounts.GetAllAsync(
            A<Expression<Func<Account, bool>>>.Ignored,
            A<string>.Ignored))
            .Returns(users);

        A.CallTo(() => _mockMapper.Map<IEnumerable<AdminUserVM>>(users))
            .Returns(userVMs);

        // Act
        var result = await _adminUserService.GetUsersByAdmin(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Users.Should().HaveCount(2);
        result.Data.Pagination.Should().NotBeNull();
        A.CallTo(() => _mockUnitOfWork.Accounts.GetAllAsync(
            A<Expression<Func<Account, bool>>>.Ignored,
            A<string>.Ignored)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetAllUsersByAdmin_WhenInvalidStatus_ReturnsFailure()
    {
        // Arrange
        var param = new AdminUserParams
        {
            Status = (AccountStatus)999 // Invalid status
        };

        // Act
        var result = await _adminUserService.GetUsersByAdmin(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.InvalidStatus);
    }

    [Fact]
    public async Task GetAllUsersByAdmin_WhenInvalidRole_ReturnsFailure()
    {
        // Arrange
        var param = new AdminUserParams
        {
            Role = Role.ADMIN // Invalid role for this operation
        };

        // Act
        var result = await _adminUserService.GetUsersByAdmin(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.InvalidRole);
    }

    [Fact]
    public async Task UpdateUserPassword_WhenValid_ReturnsSuccess()
    {
        // Arrange
        var param = new AdminUserPasswordParams
        {
            UserId = 1,
            CurrentUserId = 2,
            Password = "newPassword123"
        };

        var account = new Account { Id = 1 };

        A.CallTo(() => _mockUnitOfWork.Accounts.GetByIdAsync(param.UserId, A<string>.Ignored))
            .Returns(account);

        A.CallTo(() => _mockUnitOfWork.SaveAsync())
            .Returns(1);

        // Act
        var result = await _adminUserService.UpdateUserPassword(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be(ResponseMessages.ChangePasswordSuccess);
        A.CallTo(() => _mockUnitOfWork.Accounts.GetByIdAsync(param.UserId, A<string>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _mockUnitOfWork.SaveAsync()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateUserPassword_WhenUserNotFound_ReturnsFailure()
    {
        // Arrange
        var param = new AdminUserPasswordParams
        {
            UserId = 1,
            CurrentUserId = 2,
            Password = "newPassword123"
        };

        A.CallTo(() => _mockUnitOfWork.Accounts.GetByIdAsync(param.UserId, A<string>.Ignored))
            .Returns<Account?>(null);

        // Act
        var result = await _adminUserService.UpdateUserPassword(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.UserNotFound);
        A.CallTo(() => _mockUnitOfWork.Accounts.GetByIdAsync(param.UserId, A<string>.Ignored)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateUserStatus_WhenValid_ReturnsSuccess()
    {
        // Arrange
        var param = new AdminUserStatusParams
        {
            UserId = 1,
            CurrentUserId = 2,
            Status = AccountStatus.SUSPENDED
        };

        var account = new Account { Id = 1 };

        A.CallTo(() => _mockUnitOfWork.Accounts.GetByIdAsync(param.UserId, A<string>.Ignored))
            .Returns(account);

        A.CallTo(() => _mockUnitOfWork.SaveAsync())
            .Returns(1);

        // Act
        var result = await _adminUserService.UpdateUserStatus(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be(ResponseMessages.UpdateStatusSuccess);
        A.CallTo(() => _mockUnitOfWork.Accounts.GetByIdAsync(param.UserId, A<string>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _mockUnitOfWork.SaveAsync()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateUserStatus_WhenInvalidStatus_ReturnsFailure()
    {
        // Arrange
        var param = new AdminUserStatusParams
        {
            Status = (AccountStatus)999 // Invalid status
        };

        // Act
        var result = await _adminUserService.UpdateUserStatus(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.InvalidStatus);
    }

    [Fact]
    public async Task UpdateUserStatus_WhenUserNotFound_ReturnsFailure()
    {
        // Arrange
        var param = new AdminUserStatusParams
        {
            UserId = 1,
            CurrentUserId = 2,
            Status = AccountStatus.SUSPENDED
        };

        A.CallTo(() => _mockUnitOfWork.Accounts.GetByIdAsync(param.UserId, A<string>.Ignored))
            .Returns<Account?>(null);

        // Act
        var result = await _adminUserService.UpdateUserStatus(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.UserNotFound);
        A.CallTo(() => _mockUnitOfWork.Accounts.GetByIdAsync(param.UserId, A<string>.Ignored)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateUserStatus_WhenSaveFails_ReturnsFailure()
    {
        // Arrange
        var param = new AdminUserStatusParams
        {
            UserId = 1,
            CurrentUserId = 2,
            Status = AccountStatus.SUSPENDED
        };

        var account = new Account { Id = 1 };

        A.CallTo(() => _mockUnitOfWork.Accounts.GetByIdAsync(param.UserId, A<string>.Ignored))
            .Returns(account);

        A.CallTo(() => _mockUnitOfWork.SaveAsync())
            .Returns(0);

        // Act
        var result = await _adminUserService.UpdateUserStatus(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.UpdateStatusFailed);
        A.CallTo(() => _mockUnitOfWork.SaveAsync()).MustHaveHappenedOnceExactly();
    }
}
