using BLL.Services;
using DAL.Repository.Core;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Models.Accounts;
using Utility;
using Utility.Constants;

namespace Tests.Services;
public class TokenServiceTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AuthService> _logger;
    private readonly TokenService _tokenService;

    public TokenServiceTests()
    {
        _unitOfWork = A.Fake<IUnitOfWork>();
        _logger = A.Fake<ILogger<AuthService>>();
        _tokenService = new TokenService(_unitOfWork, _logger);
    }

    #region IsRefreshTokenValid

    [Fact]
    public async Task IsRefreshTokenValid_ShouldReturnSuccess_WhenTokenIsValid()
    {
        // Arrange
        var accountId = 1;
        var validToken = "valid_refresh_token";
        var account = new Account
        {
            Id = accountId,
            Email = "test@example.com",
            RefreshToken = validToken,
            RefreshTokenExpiryTime = DateTimeUtils.TimeInEpoch(DateTime.Now.AddDays(1))
        };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(accountId, A<string?>._))
            .Returns(Task.FromResult<Account?>(account));

        // Act
        var result = await _tokenService.IsRefreshTokenValid(accountId, validToken);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be(ResponseMessages.ValidRefreshToken);
        result.Data.Should().Be(account.Email);
    }

    [Fact]
    public async Task IsRefreshTokenValid_ShouldReturnFailure_WhenAccountNotFound()
    {
        // Arrange
        var accountId = 1;
        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(accountId, A<string?>._)).Returns(Task.FromResult<Account?>(null));

        // Act
        var result = await _tokenService.IsRefreshTokenValid(accountId, "some_token");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.AccountNotFound);
    }

    [Fact]
    public async Task IsRefreshTokenValid_ShouldReturnFailure_WhenTokenIsInvalid()
    {
        // Arrange
        var accountId = 1;
        var invalidToken = "invalid_refresh_token";
        var account = new Account
        {
            Id = accountId,
            RefreshToken = "valid_token",
            RefreshTokenExpiryTime = DateTimeUtils.TimeInEpoch(DateTime.Now.AddDays(-1)) // Expired
        };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(accountId, A<string?>._))
            .Returns(Task.FromResult<Account?>(account));

        // Act
        var result = await _tokenService.IsRefreshTokenValid(accountId, invalidToken);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.InvalidRefreshToken);
    }

    #endregion

    #region RevokeRefreshToken

    [Fact]
    public async Task RevokeRefreshToken_ShouldReturnSuccess_WhenTokenIsRevoked()
    {
        // Arrange
        var accountId = 1;
        var account = new Account { Id = accountId, RefreshToken = "valid_token" };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(accountId, A<string?>._)).Returns(Task.FromResult<Account?>(account));
        A.CallTo(() => _unitOfWork.Accounts.UpdateAsync(account)).Returns(Task.CompletedTask);
        A.CallTo(() => _unitOfWork.SaveAsync()).Returns(Task.FromResult(1));

        // Act
        var result = await _tokenService.RevokeRefreshToken(accountId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be(ResponseMessages.LogoutSuccess);
        account.RefreshToken.Should().BeNull();
    }

    [Fact]
    public async Task RevokeRefreshToken_ShouldReturnFailure_WhenAccountNotFound()
    {
        // Arrange
        var accountId = 1;
        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(accountId, A<string?>._)).Returns(Task.FromResult<Account?>(null));

        // Act
        var result = await _tokenService.RevokeRefreshToken(accountId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.AccountNotFound);
    }

    #endregion

    #region StoreRefreshToken

    [Fact]
    public async Task StoreRefreshToken_ShouldReturnSuccess_WhenTokenIsStored()
    {
        // Arrange
        var accountId = 1;
        var newToken = "new_refresh_token";
        var account = new Account { Id = accountId };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(accountId, A<string?>._)).Returns(Task.FromResult<Account?>(account));
        A.CallTo(() => _unitOfWork.Accounts.UpdateAsync(account)).Returns(Task.CompletedTask);
        A.CallTo(() => _unitOfWork.SaveAsync()).Returns(Task.FromResult(1));

        // Act
        var result = await _tokenService.StoreRefreshToken(accountId, newToken);

        // Assert
        result.IsSuccess.Should().BeTrue();
        account.RefreshToken.Should().Be(newToken);
    }

    [Fact]
    public async Task StoreRefreshToken_ShouldReturnFailure_WhenAccountNotFound()
    {
        // Arrange
        var accountId = 1;
        var newToken = "new_refresh_token";

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(accountId, A<string?>._)).Returns(Task.FromResult<Account?>(null));

        // Act
        var result = await _tokenService.StoreRefreshToken(accountId, newToken);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.AccountNotFound);
    }

    #endregion
}
