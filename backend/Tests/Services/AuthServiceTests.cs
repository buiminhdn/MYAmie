using AutoMapper;
using BLL.Services;
using Common.DTOs.AuthDtos;
using Common.ViewModels.AuthVMs;
using DAL.Repository.Core;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Models.Accounts;
using Models.Categories;
using Models.Cities;
using System.Linq.Expressions;
using Utility;
using Utility.Constants;

namespace Tests.Services;
public class AuthServiceTests
{
    private readonly AuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AuthService> _logger;
    private readonly IMapper _mapper;

    public AuthServiceTests()
    {
        _unitOfWork = A.Fake<IUnitOfWork>();
        _logger = A.Fake<ILogger<AuthService>>();
        _mapper = A.Fake<IMapper>();

        _authService = new AuthService(_unitOfWork, _logger, _mapper);
    }

    #region SignIn Tests

    [Fact]
    public async Task SignIn_ShouldReturnSuccess_WhenCredentialsAreValid()
    {
        // Arrange
        var signInParams = new SignInParams { Email = "test@example.com", Password = "password123" };
        var account = new Account
        {
            Email = signInParams.Email,
            Password = PasswordUtils.Hash(signInParams.Password),
            IsEmailVerified = true,
            Status = AccountStatus.ACTIVATED
        };
        var authAccountVM = new AuthAccountVM { Email = signInParams.Email };

        A.CallTo(() => _unitOfWork.Accounts.GetAsync(A<Expression<Func<Account, bool>>>._, A<string?>._))
            .Returns(Task.FromResult<Account?>(account));

        A.CallTo(() => _mapper.Map<AuthAccountVM>(account)).Returns(authAccountVM);

        // Act
        var result = await _authService.SignIn(signInParams);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(authAccountVM);
        result.Message.Should().Be(ResponseMessages.LoginSuccess);
    }

    [Fact]
    public async Task SignIn_ShouldReturnFailure_WhenAccountNotFound()
    {
        // Arrange
        var signInParams = new SignInParams { Email = "test@example.com", Password = "password123" };

        A.CallTo(() => _unitOfWork.Accounts.GetAsync(A<Expression<Func<Account, bool>>>._, A<string?>._))
            .Returns(Task.FromResult<Account?>(null));

        // Act
        var result = await _authService.SignIn(signInParams);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.AccountNotFound);
    }

    [Fact]
    public async Task SignIn_ShouldReturnFailure_WhenEmailNotVerified()
    {
        // Arrange
        var signInParams = new SignInParams { Email = "test@example.com", Password = "password123" };
        var account = new Account { Email = signInParams.Email, IsEmailVerified = false };

        A.CallTo(() => _unitOfWork.Accounts.GetAsync(A<Expression<Func<Account, bool>>>._, A<string?>._))
            .Returns(Task.FromResult<Account?>(account));

        // Act
        var result = await _authService.SignIn(signInParams);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.AccountNotVerified);
    }

    [Fact]
    public async Task SignIn_ShouldReturnFailure_WhenAccountSuspended()
    {
        // Arrange
        var signInParams = new SignInParams { Email = "test@example.com", Password = "password123" };
        var account = new Account { Email = signInParams.Email, Status = AccountStatus.SUSPENDED, IsEmailVerified = true };

        A.CallTo(() => _unitOfWork.Accounts.GetAsync(A<Expression<Func<Account, bool>>>._, A<string?>._))
            .Returns(Task.FromResult<Account?>(account));

        // Act
        var result = await _authService.SignIn(signInParams);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.AccountSuspended);
    }

    [Fact]
    public async Task SignIn_ShouldReturnFailure_WhenPasswordIsIncorrect()
    {
        // Arrange
        var signInParams = new SignInParams { Email = "test@example.com", Password = "wrongpassword" };
        var account = new Account
        {
            Email = signInParams.Email,
            Password = PasswordUtils.Hash("password123"),
            IsEmailVerified = true,
            Status = AccountStatus.ACTIVATED
        };

        A.CallTo(() => _unitOfWork.Accounts.GetAsync(A<Expression<Func<Account, bool>>>._, A<string?>._))
            .Returns(Task.FromResult<Account?>(account));

        // Act
        var result = await _authService.SignIn(signInParams);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.InvalidCredentials);
    }

    #endregion

    #region SignUp Tests

    [Fact]
    public async Task SignUp_ShouldReturnSuccess_WhenNewAccountIsCreated()
    {
        // Arrange
        var signUpParams = new SignUpParams
        {
            Email = "test@example.com",
            Password = "password123",
            FirstName = "John",
            LastName = "Doe"
        };

        A.CallTo(() => _unitOfWork.Accounts.IsExistAsync(A<Expression<Func<Account, bool>>>._))
            .Returns(Task.FromResult(false));

        A.CallTo(() => _unitOfWork.Accounts.AddAsync(A<Account>._)).Returns(Task.CompletedTask);
        A.CallTo(() => _unitOfWork.SaveAsync()).Returns(Task.FromResult(1));

        // Act
        var result = await _authService.SignUp(signUpParams);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be(ResponseMessages.SignupSuccess);
    }

    [Fact]
    public async Task SignUp_ShouldReturnFailure_WhenEmailAlreadyExists()
    {
        // Arrange
        var signUpParams = new SignUpParams { Email = "test@example.com" };

        A.CallTo(() => _unitOfWork.Accounts.IsExistAsync(A<Expression<Func<Account, bool>>>._))
            .Returns(Task.FromResult(true));

        // Act
        var result = await _authService.SignUp(signUpParams);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.EmailAlreadyExists);
    }

    [Fact]
    public async Task SignUp_ShouldReturnFailure_WhenSaveFails()
    {
        // Arrange
        var signUpParams = new SignUpParams
        {
            Email = "test@example.com",
            Password = "password123"
        };

        A.CallTo(() => _unitOfWork.Accounts.IsExistAsync(A<Expression<Func<Account, bool>>>._))
            .Returns(Task.FromResult(false));

        A.CallTo(() => _unitOfWork.Accounts.AddAsync(A<Account>._)).Returns(Task.CompletedTask);
        A.CallTo(() => _unitOfWork.SaveAsync()).Returns(Task.FromResult(0)); // Simulate failure

        // Act
        var result = await _authService.SignUp(signUpParams);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.SignupFailed);
    }

    #endregion

    #region SignUpBusiness Tests

    [Fact]
    public async Task SignUpBusiness_EmailAlreadyExists_ReturnsFailure()
    {
        // Arrange
        var param = new SignUpBusinessParams { Email = "test@example.com" };
        A.CallTo(() => _unitOfWork.Accounts.IsExistAsync(A<Expression<Func<Account, bool>>>._)).Returns(Task.FromResult(true));

        // Act
        var result = await _authService.SignUpBusiness(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.EmailAlreadyExists);
    }

    [Fact]
    public async Task SignUpBusiness_CityNotFound_ReturnsFailure()
    {
        // Arrange
        var param = new SignUpBusinessParams { Email = "test@example.com", CityId = 1 };
        A.CallTo(() => _unitOfWork.Accounts.IsExistAsync(A<Expression<Func<Account, bool>>>._)).Returns(Task.FromResult(false));
        A.CallTo(() => _unitOfWork.Cities.GetByIdAsync(param.CityId, A<string?>._)).Returns(Task.FromResult<City?>(null));

        // Act
        var result = await _authService.SignUpBusiness(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.CityNotFound);
    }

    [Fact]
    public async Task SignUpBusiness_CategoryNotFound_ReturnsFailure()
    {
        // Arrange
        var param = new SignUpBusinessParams { Email = "test@example.com", CityId = 1, CategoryIds = new List<int> { 1, 2 } };
        var city = new City { Id = 1, Name = "Sample City" };

        A.CallTo(() => _unitOfWork.Accounts.IsExistAsync(A<Expression<Func<Account, bool>>>._)).Returns(Task.FromResult(false));
        A.CallTo(() => _unitOfWork.Cities.GetByIdAsync(param.CityId, A<string>._)).Returns(Task.FromResult<City?>(city));
        A.CallTo(() => _unitOfWork.Categories.GetAllAsync(A<Expression<Func<Category, bool>>>._, A<string>._))
       .Returns(Task.FromResult<IEnumerable<Category>>([new Category { Id = 1 }]));


        // Act
        var result = await _authService.SignUpBusiness(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.CategoryNotFound);
    }

    [Fact]
    public async Task SignUpBusiness_SaveSuccess_ReturnsSuccess()
    {
        // Arrange
        var param = new SignUpBusinessParams { Email = "test@example.com", CityId = 1, CategoryIds = [1, 2], Password = "123456" };
        var city = new City { Id = 1, Name = "Sample City" };
        var categories = new List<Category> { new() { Id = 1 }, new() { Id = 2 } };

        A.CallTo(() => _unitOfWork.Accounts.IsExistAsync(A<Expression<Func<Account, bool>>>._)).Returns(Task.FromResult(false));
        A.CallTo(() => _unitOfWork.Cities.GetByIdAsync(param.CityId, A<string>._)).Returns(Task.FromResult<City?>(city));
        A.CallTo(() => _unitOfWork.Categories.GetAllAsync(A<Expression<Func<Category, bool>>>._, A<string>._)).Returns(Task.FromResult<IEnumerable<Category>>(categories));
        A.CallTo(() => _unitOfWork.SaveAsync()).Returns(Task.FromResult(1));

        // Act
        var result = await _authService.SignUpBusiness(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be(ResponseMessages.SignupSuccess);
    }

    [Fact]
    public async Task SignUpBusiness_SaveFails_ReturnsFailure()
    {
        // Arrange
        var param = new SignUpBusinessParams { Email = "test@example.com", CityId = 1, CategoryIds = [1, 2], Password = "123456" };
        var city = new City { Id = 1, Name = "Sample City" };
        var categories = new List<Category> { new() { Id = 1 }, new() { Id = 2 } };

        A.CallTo(() => _unitOfWork.Accounts.IsExistAsync(A<Expression<Func<Account, bool>>>._)).Returns(Task.FromResult(false));
        A.CallTo(() => _unitOfWork.Cities.GetByIdAsync(param.CityId, A<string>._)).Returns(Task.FromResult<City?>(city));
        A.CallTo(() => _unitOfWork.Categories.GetAllAsync(A<Expression<Func<Category, bool>>>._, A<string>._)).Returns(Task.FromResult<IEnumerable<Category>>(categories));
        A.CallTo(() => _unitOfWork.SaveAsync()).Returns(Task.FromResult(0));

        // Act
        var result = await _authService.SignUpBusiness(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.SignupFailed);
    }

    [Fact]
    public async Task SignUpBusiness_ThrowsException_ReturnsUnexpectedError()
    {
        // Arrange
        var param = new SignUpBusinessParams { Email = "test@example.com" };
        A.CallTo(() => _unitOfWork.Accounts.IsExistAsync(A<Expression<Func<Account, bool>>>._)).Throws<Exception>();

        // Act
        var result = await _authService.SignUpBusiness(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.UnexpectedError);
    }

    #endregion
}
