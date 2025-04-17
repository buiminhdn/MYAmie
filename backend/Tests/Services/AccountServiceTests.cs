using AutoMapper;
using BLL.Services;
using Common.DTOs.AccountDtos;
using Common.ViewModels.ProfileVMs;
using DAL.Repository.Core;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Models.Accounts;
using Models.Businesses;
using Models.Categories;
using Models.Cities;
using System.Linq.Expressions;
using Utility;
using Utility.Constants;

namespace Tests.Services;
public class AccountServiceTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AccountService> _logger;
    private readonly IMapper _mapper;
    private readonly AccountService _accountService;

    public AccountServiceTests()
    {
        _unitOfWork = A.Fake<IUnitOfWork>();
        _logger = A.Fake<ILogger<AccountService>>();
        _mapper = A.Fake<IMapper>();

        _accountService = new AccountService(_unitOfWork, _logger, _mapper);
    }

    #region GetBusinessProfile

    [Fact]
    public async Task GetBusinessProfile_ShouldReturnProfile_WhenAccountExists()
    {
        // Arrange
        int userId = 1;
        var account = new Account { Id = userId };
        var profileVM = new BusinessProfileVM();

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(userId, "Categories,City,Business"))
         .Returns(Task.FromResult<Account?>(account));

        A.CallTo(() => _mapper.Map<BusinessProfileVM>(account))
         .Returns(profileVM);

        // Act
        var result = await _accountService.GetBusinessProfile(userId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(profileVM);
    }

    [Fact]
    public async Task GetBusinessProfile_ShouldReturnFailure_WhenAccountNotFound()
    {
        // Arrange
        int userId = 1;

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(userId, "Categories,City,Business"))
         .Returns(Task.FromResult<Account?>(null));

        // Act
        var result = await _accountService.GetBusinessProfile(userId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.AccountNotFound);
    }

    #endregion

    #region GetUserProfile

    [Fact]
    public async Task GetUserProfile_ShouldReturnProfile_WhenAccountExists()
    {
        // Arrange
        int userId = 1;
        var account = new Account { Id = userId };
        var profileVM = new UserProfileVM();

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(userId, "Categories,City"))
         .Returns(Task.FromResult<Account?>(account));

        A.CallTo(() => _mapper.Map<UserProfileVM>(account))
         .Returns(profileVM);

        // Act
        var result = await _accountService.GetUserProfile(userId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(profileVM);
    }

    [Fact]
    public async Task GetUserProfile_ShouldReturnFailure_WhenAccountNotFound()
    {
        // Arrange
        int userId = 1;

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(userId, "Categories,City"))
         .Returns(Task.FromResult<Account?>(null));

        // Act
        var result = await _accountService.GetUserProfile(userId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.AccountNotFound);
    }

    #endregion

    #region ResetPassword

    [Fact]
    public async Task ResetPassword_ShouldReturnSuccess_WhenPasswordUpdated()
    {
        // Arrange
        string email = "test@example.com";
        string newPassword = "NewPass123";
        var account = new Account { Id = 1, Email = email, IsEmailVerified = true };

        A.CallTo(() => _unitOfWork.Accounts.GetAsync(A<Expression<Func<Account, bool>>>._, null))
         .Returns(Task.FromResult<Account?>(account));

        A.CallTo(() => _unitOfWork.SaveAsync()).Returns(Task.FromResult(1));

        // Act
        var result = await _accountService.ResetPassword(email, newPassword);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be(ResponseMessages.ResetPasswordSuccess);
    }

    [Fact]
    public async Task ResetPassword_ShouldReturnFailure_WhenAccountNotFound()
    {
        // Arrange
        string email = "test@example.com";
        string newPassword = "NewPass123";

        A.CallTo(() => _unitOfWork.Accounts.GetAsync(A<Expression<Func<Account, bool>>>._, null))
         .Returns(Task.FromResult<Account?>(null));

        // Act
        var result = await _accountService.ResetPassword(email, newPassword);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.AccountNotFound);
    }

    [Fact]
    public async Task ResetPassword_ShouldReturnFailure_WhenEmailNotVerified()
    {
        // Arrange
        string email = "test@example.com";
        string newPassword = "NewPass123";
        var account = new Account { Id = 1, Email = email, IsEmailVerified = false };

        A.CallTo(() => _unitOfWork.Accounts.GetAsync(A<Expression<Func<Account, bool>>>._, null))
         .Returns(Task.FromResult<Account?>(account));

        // Act
        var result = await _accountService.ResetPassword(email, newPassword);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.AccountNotVerified);
    }

    [Fact]
    public async Task ResetPassword_ShouldReturnFailure_WhenDbUpdateFails()
    {
        // Arrange
        string email = "test@example.com";
        string newPassword = "NewPass123";
        //var account = new Account { Id = 1, Email = email, IsEmailVerified = true };

        A.CallTo(() => _unitOfWork.Accounts.GetAsync(A<Expression<Func<Account, bool>>>._, null))
         .Returns(Task.FromResult<Account?>(new Account { Id = 1, Email = email, IsEmailVerified = true }));

        A.CallTo(() => _unitOfWork.SaveAsync()).Returns(Task.FromResult(0));

        // Act
        var result = await _accountService.ResetPassword(email, newPassword);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.ResetPasswordFailed);
    }

    #endregion

    #region ChangePassword

    [Fact]
    public async Task ChangePassword_AccountNotFound_ReturnsFailure()
    {
        // Arrange
        var param = new ChangePasswordParams
        {
            CurrentUserId = 1,
            OldPassword = "oldPassword",
            NewPassword = "newPassword"
        };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(param.CurrentUserId, null))
            .Returns(Task.FromResult<Account?>(null));

        // Act
        var result = await _accountService.ChangePassword(param);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.AccountNotFound);
    }

    [Fact]
    public async Task ChangePassword_OldPasswordIncorrect_ReturnsFailure()
    {
        // Arrange
        var param = new ChangePasswordParams
        {
            CurrentUserId = 1,
            OldPassword = "wrongPassword",
            NewPassword = "newPassword"
        };

        var account = new Account
        {
            Id = param.CurrentUserId,
            IsEmailVerified = true,
            Password = PasswordUtils.Hash("correctPassword")
        };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(param.CurrentUserId, null))
            .Returns(Task.FromResult<Account?>(account));

        // Act
        var result = await _accountService.ChangePassword(param);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.OldPasswordIncorrect);
    }

    [Fact]
    public async Task ChangePassword_PasswordChangeSuccess_ReturnsSuccess()
    {
        // Arrange
        var param = new ChangePasswordParams
        {
            CurrentUserId = 1,
            OldPassword = "oldPassword",
            NewPassword = "newPassword"
        };

        var account = new Account
        {
            Id = param.CurrentUserId,
            IsEmailVerified = true,
            Password = PasswordUtils.Hash(param.OldPassword)
        };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(param.CurrentUserId, null))
            .Returns(Task.FromResult<Account?>(account));

        A.CallTo(() => _unitOfWork.SaveAsync())
            .Returns(Task.FromResult(1)); // Simulate successful save

        // Act
        var result = await _accountService.ChangePassword(param);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be(ResponseMessages.ChangePasswordSuccess);
        account.UpdatedBy.Should().Be(param.CurrentUserId);
    }

    #endregion

    #region UpdateProfile

    [Fact]
    public async Task UpdateProfile_AccountNotFound_ReturnsFailure()
    {
        // Arrange
        var param = new UpdateProfileParams
        {
            CurrentUserId = 1,
            CityId = 1,
            CategoryIds = new List<int> { 1, 2 },
            Images = "image1.jpg;image2.jpg",
            FirstName = "John",
            LastName = "Doe",
            ShortDescription = "Test Description",
            Description = "Long Description",
            //DateOfBirth = DateTime.UtcNow,
            Characteristics = new List<string> { "Char1", "Char2" }
        };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(param.CurrentUserId, A<string>._))
            .Returns(Task.FromResult<Account?>(null));

        // Act
        var result = await _accountService.UpdateProfile(param);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.AccountNotFound);
    }

    [Fact]
    public async Task UpdateProfile_AccountNotVerified_ReturnsFailure()
    {
        // Arrange
        var param = new UpdateProfileParams
        {
            CurrentUserId = 1,
            CityId = 1,
            CategoryIds = new List<int> { 1, 2 },
            Images = "image1.jpg;image2.jpg",
            FirstName = "John",
            LastName = "Doe",
            ShortDescription = "Test Description",
            Description = "Long Description",
            //DateOfBirth = DateTime.UtcNow,
            Characteristics = new List<string> { "Char1", "Char2" }
        };

        var account = new Account
        {
            Id = param.CurrentUserId,
            IsEmailVerified = false
        };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(param.CurrentUserId, A<string>._))
            .Returns(Task.FromResult<Account?>(account));

        // Act
        var result = await _accountService.UpdateProfile(param);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.AccountNotVerified);
    }

    [Fact]
    public async Task UpdateProfile_Success_ReturnsSuccess()
    {
        // Arrange
        var param = new UpdateProfileParams
        {
            CurrentUserId = 1,
            CityId = 1,
            CategoryIds = new List<int> { 1, 2 },
            Images = "image1.jpg;image2.jpg",
            FirstName = "John",
            LastName = "Doe",
            ShortDescription = "Test Description",
            Description = "Long Description",
            //DateOfBirth = DateTime.UtcNow,
            Characteristics = new List<string> { "Char1", "Char2" }
        };

        var account = new Account
        {
            Id = param.CurrentUserId,
            IsEmailVerified = true,
            CityId = 0 // Ensure CityId is different to trigger city update
        };

        var city = new City { Id = param.CityId };
        var categories = new List<Category>
    {
        new Category { Id = 1 },
        new Category { Id = 2 }
    };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(param.CurrentUserId, A<string>._))
            .Returns(Task.FromResult<Account?>(account));

        A.CallTo(() => _unitOfWork.Cities.GetByIdAsync(param.CityId, A<string>._))
            .Returns(Task.FromResult<City?>(city));

        A.CallTo(() => _unitOfWork.Categories.GetAllAsync(A<Expression<Func<Category, bool>>>._, A<string>._))
            .Returns(Task.FromResult(categories.AsEnumerable()));

        A.CallTo(() => _unitOfWork.SaveAsync())
            .Returns(Task.FromResult(1)); // Simulate successful save

        // Act
        var result = await _accountService.UpdateProfile(param);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be(ResponseMessages.UpdateProfileSuccess);
        account.City.Should().Be(city);
        account.Categories.Should().BeEquivalentTo(categories);
        account.Images.Should().Be(param.Images);
        account.FirstName.Should().Be(param.FirstName);
        account.LastName.Should().Be(param.LastName);
        account.ShortDescription.Should().Be(param.ShortDescription);
        account.Description.Should().Be(param.Description);
        account.NormalizedInfo.Should().Be($"{param.FirstName} {param.LastName} {account.Email} {param.ShortDescription}".RemoveDiacritics());
        account.UpdatedBy.Should().Be(param.CurrentUserId);
        account.Characteristics.Should().Be(string.Join(";", param.Characteristics));
    }

    #endregion

    #region UpdateBusinessProfile

    [Fact]
    public async Task UpdateBusinessProfile_AccountNotFound_ReturnsFailure()
    {
        // Arrange
        var param = new UpdateBusinessProfileParams
        {
            CurrentUserId = 1,
            CityId = 1,
            CategoryIds = new List<int> { 1, 2 },
            Images = "image1.jpg;image2.jpg",
            Name = "Business Name",
            ShortDescription = "Business Description",
            Description = "Long Business Description",
            Address = "123 Business St",
            OpenHour = 9,
            CloseHour = 17,
            Phone = "123-456-7890"
        };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(param.CurrentUserId, A<string>._))
            .Returns<Account?>(null);

        // Act
        var result = await _accountService.UpdateBusinessProfile(param);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.AccountNotFound);
    }

    [Fact]
    public async Task UpdateBusinessProfile_AccountNotVerified_ReturnsFailure()
    {
        // Arrange
        var param = new UpdateBusinessProfileParams
        {
            CurrentUserId = 1,
            CityId = 1,
            CategoryIds = new List<int> { 1, 2 },
            Images = "image1.jpg;image2.jpg",
            Name = "Business Name",
            ShortDescription = "Business Description",
            Description = "Long Business Description",
            Address = "123 Business St",
            OpenHour = 9,
            CloseHour = 17,
            Phone = "123-456-7890"
        };

        var account = new Account
        {
            Id = param.CurrentUserId,
            IsEmailVerified = false
        };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(param.CurrentUserId, A<string>._))
            .Returns(account);

        // Act
        var result = await _accountService.UpdateBusinessProfile(param);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.AccountNotVerified);
    }

    [Fact]
    public async Task UpdateBusinessProfile_CityNotFound_ReturnsFailure()
    {
        // Arrange
        var param = new UpdateBusinessProfileParams
        {
            CurrentUserId = 1,
            CityId = 1,
            CategoryIds = new List<int> { 1, 2 },
            Images = "image1.jpg;image2.jpg",
            Name = "Business Name",
            ShortDescription = "Business Description",
            Description = "Long Business Description",
            Address = "123 Business St",
            OpenHour = 9,
            CloseHour = 17,
            Phone = "123-456-7890"
        };

        var account = new Account
        {
            Id = param.CurrentUserId,
            IsEmailVerified = true,
            CityId = 0
        };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(param.CurrentUserId, A<string>._))
            .Returns(account);

        A.CallTo(() => _unitOfWork.Cities.GetByIdAsync(param.CityId, A<string>._))
            .Returns<City?>(null);

        // Act
        var result = await _accountService.UpdateBusinessProfile(param);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.CityNotFound);
    }

    [Fact]
    public async Task UpdateBusinessProfile_Success_ReturnsSuccessWithBusinessProfileVM()
    {
        // Arrange
        var param = new UpdateBusinessProfileParams
        {
            CurrentUserId = 1,
            CityId = 1,
            CategoryIds = new List<int> { 1, 2 },
            Images = "image1.jpg;image2.jpg",
            Name = "Business Name",
            ShortDescription = "Business Description",
            Description = "Long Business Description",
            Address = "123 Business St",
            OpenHour = 9,
            CloseHour = 17,
            Phone = "123-456-7890"
        };

        var account = new Account
        {
            Id = param.CurrentUserId,
            IsEmailVerified = true,
            CityId = 0,
            Business = new Business()
        };

        var city = new City { Id = param.CityId };
        var categories = new List<Category>
            {
                new Category { Id = 1 },
                new Category { Id = 2 }
            };

        var businessProfileVM = new BusinessProfileVM();

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(param.CurrentUserId, A<string>._))
            .Returns(account);

        A.CallTo(() => _unitOfWork.Cities.GetByIdAsync(param.CityId, A<string>._))
            .Returns(city);

        A.CallTo(() => _unitOfWork.Categories.GetAllAsync(A<Expression<Func<Category, bool>>>._, A<string>._))
            .Returns(categories);

        A.CallTo(() => _mapper.Map<BusinessProfileVM>(account))
            .Returns(businessProfileVM);

        A.CallTo(() => _unitOfWork.SaveAsync())
            .Returns(1);

        // Act
        var result = await _accountService.UpdateBusinessProfile(param);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be(ResponseMessages.UpdateProfileSuccess);
        result.Data.Should().Be(null);
    }

    [Fact]
    public async Task UpdateBusinessProfile_SaveFailed_ReturnsFailure()
    {
        // Arrange
        var param = new UpdateBusinessProfileParams
        {
            CurrentUserId = 1,
            CityId = 1,
            CategoryIds = new List<int> { 1, 2 },
            Images = "image1.jpg;image2.jpg",
            Name = "Business Name",
            ShortDescription = "Business Description",
            Description = "Long Business Description",
            Address = "123 Business St",
            OpenHour = 9,
            CloseHour = 17,
            Phone = "123-456-7890"
        };

        var account = new Account
        {
            Id = param.CurrentUserId,
            IsEmailVerified = true,
            CityId = 0,
            Business = new Business()
        };

        var city = new City { Id = param.CityId };
        var categories = new List<Category>
            {
                new Category { Id = 1 },
                new Category { Id = 2 }
            };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(param.CurrentUserId, A<string>._))
            .Returns(account);

        A.CallTo(() => _unitOfWork.Cities.GetByIdAsync(param.CityId, A<string>._))
            .Returns(city);

        A.CallTo(() => _unitOfWork.Categories.GetAllAsync(A<Expression<Func<Category, bool>>>._, A<string>._))
            .Returns(categories);

        A.CallTo(() => _unitOfWork.SaveAsync())
            .Returns(0);

        // Act
        var result = await _accountService.UpdateBusinessProfile(param);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.UpdateProfileFailed);
    }

    #endregion

    #region UpdateAvatarOrCover

    [Fact]
    public async Task UpdateAvatarOrCover_EmptyImagePath_ReturnsFailure()
    {
        // Arrange
        int accountId = 1;
        string imgPath = "";
        bool isAvatar = true;

        // Act
        var result = await _accountService.UpdateAvatarOrCover(accountId, imgPath, isAvatar);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.NoImagesProvided);
    }

    [Fact]
    public async Task UpdateAvatarOrCover_AccountNotFound_ReturnsFailure()
    {
        // Arrange
        int accountId = 1;
        string imgPath = "path/to/image.jpg";
        bool isAvatar = true;

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(accountId, A<string>._))
            .Returns<Account?>(null);

        // Act
        var result = await _accountService.UpdateAvatarOrCover(accountId, imgPath, isAvatar);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.AccountNotFound);
    }

    [Fact]
    public async Task UpdateAvatarOrCover_AccountNotVerified_ReturnsFailure()
    {
        // Arrange
        int accountId = 1;
        string imgPath = "path/to/image.jpg";
        bool isAvatar = true;

        var account = new Account
        {
            Id = accountId,
            IsEmailVerified = false
        };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(accountId, A<string>._))
            .Returns(account);

        // Act
        var result = await _accountService.UpdateAvatarOrCover(accountId, imgPath, isAvatar);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.AccountNotVerified);
    }

    [Fact]
    public async Task UpdateAvatarOrCover_UpdateCoverSuccess_ReturnsSuccessWithOldAndNewPaths()
    {
        // Arrange
        int accountId = 1;
        string imgPath = "path/to/image.jpg";
        bool isAvatar = false;
        string oldCoverPath = "old/path/to/cover.jpg";

        var account = new Account
        {
            Id = accountId,
            IsEmailVerified = true,
            Cover = oldCoverPath
        };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(accountId, A<string>._))
            .Returns(account);

        A.CallTo(() => _unitOfWork.SaveAsync())
            .Returns(1);

        // Act
        var result = await _accountService.UpdateAvatarOrCover(accountId, imgPath, isAvatar);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be(ResponseMessages.UpdateImageSuccess);
        result.Data.Should().NotBeNull();
        result.Data.OldPath.Should().Be(oldCoverPath);
        result.Data.NewPath.Should().Be(imgPath);
        account.Cover.Should().Be(imgPath);
        account.UpdatedBy.Should().Be(accountId);
    }

    [Fact]
    public async Task UpdateAvatarOrCover_UpdateAvatarSuccess_ReturnsSuccessWithOldAndNewPaths()
    {
        // Arrange
        int accountId = 1;
        string imgPath = "path/to/image.jpg";
        bool isAvatar = true;
        string oldAvatarPath = "old/path/to/avatar.jpg";

        var account = new Account
        {
            Id = accountId,
            IsEmailVerified = true,
            Avatar = oldAvatarPath
        };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(accountId, A<string>._))
            .Returns(account);

        A.CallTo(() => _unitOfWork.SaveAsync())
            .Returns(1);

        // Act
        var result = await _accountService.UpdateAvatarOrCover(accountId, imgPath, isAvatar);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be(ResponseMessages.UpdateImageSuccess);
        result.Data.Should().NotBeNull();
        result.Data.OldPath.Should().Be(oldAvatarPath);
        result.Data.NewPath.Should().Be(imgPath);
        account.Avatar.Should().Be(imgPath);
        account.UpdatedBy.Should().Be(accountId);
    }

    [Fact]
    public async Task UpdateAvatarOrCover_SaveFailed_ReturnsFailure()
    {
        // Arrange
        int accountId = 1;
        string imgPath = "path/to/image.jpg";
        bool isAvatar = true;

        var account = new Account
        {
            Id = accountId,
            IsEmailVerified = true
        };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(accountId, A<string>._))
            .Returns(account);

        A.CallTo(() => _unitOfWork.SaveAsync())
            .Returns(0);

        // Act
        var result = await _accountService.UpdateAvatarOrCover(accountId, imgPath, isAvatar);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.UpdateImageFailed);
    }

    #endregion

    #region UpdateLocation

    [Fact]
    public async Task UpdateLocation_ValidRequest_ReturnsSuccess()
    {
        // Arrange
        var param = new UpdateLocationParams
        {
            CurrentUserId = 1,
            Latitude = 40.7128m,
            Longitude = -74.0060m
        };

        var account = new Account
        {
            Id = param.CurrentUserId,
            IsEmailVerified = true
        };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(param.CurrentUserId, A<string>._))
            .Returns(account);
        A.CallTo(() => _unitOfWork.SaveAsync())
            .Returns(1); // Simulate successful save

        // Act
        var result = await _accountService.UpdateLocation(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be(ResponseMessages.UpdateLocationSuccess);
        account.Latitude.Should().Be(param.Latitude);
        account.Longitude.Should().Be(param.Longitude);
    }

    [Fact]
    public async Task UpdateLocation_AccountNotFound_ReturnsFailure()
    {
        // Arrange
        var param = new UpdateLocationParams { CurrentUserId = 1 };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(param.CurrentUserId, A<string>._))
            .Returns((Account?)null);

        // Act
        var result = await _accountService.UpdateLocation(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.AccountNotFound);
    }

    [Fact]
    public async Task UpdateLocation_AccountNotVerified_ReturnsFailure()
    {
        // Arrange
        var param = new UpdateLocationParams { CurrentUserId = 1 };
        var account = new Account { Id = param.CurrentUserId, IsEmailVerified = false };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(param.CurrentUserId, A<string>._))
            .Returns(account);

        // Act
        var result = await _accountService.UpdateLocation(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.AccountNotVerified);
    }

    [Fact]
    public async Task UpdateLocation_SaveFails_ReturnsFailure()
    {
        // Arrange
        var param = new UpdateLocationParams { CurrentUserId = 1 };
        var account = new Account { Id = param.CurrentUserId, IsEmailVerified = true };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(param.CurrentUserId, A<string>._))
            .Returns(account);
        A.CallTo(() => _unitOfWork.SaveAsync())
            .Returns(0); // Simulate failed save

        // Act
        var result = await _accountService.UpdateLocation(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.UpdateLocationFailed);
    }

    [Fact]
    public async Task UpdateLocation_ExceptionThrown_ReturnsFailure()
    {
        // Arrange
        var param = new UpdateLocationParams { CurrentUserId = 1 };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(param.CurrentUserId, A<string>._))
            .Throws<Exception>();

        // Act
        var result = await _accountService.UpdateLocation(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.UnexpectedError);
    }

    #endregion

    #region GetAvatarWName

    [Fact]
    public async Task GetAvatarWName_ValidUserId_ReturnsData()
    {
        // Arrange
        int userId = 1;
        var account = new Account
        {
            Id = userId,
            FirstName = "John",
            Avatar = "avatar.jpg"
        };

        var expected = new AvatarWNameVM
        {
            Id = userId,
            Name = "John",
            Avatar = "avatar.jpg"
        };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(userId, A<string>._))
            .Returns(account);

        // Act
        var result = await _accountService.GetAvatarWName(userId);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetAvatarWName_AccountNotFound_ReturnsFailure()
    {
        // Arrange
        int userId = 1;

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(userId, A<string>._))
            .Returns((Account?)null);

        // Act
        var result = await _accountService.GetAvatarWName(userId);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.AccountNotFound);
    }

    [Fact]
    public async Task GetAvatarWName_ExceptionThrown_ReturnsFailure()
    {
        // Arrange
        int userId = 1;

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(userId, A<string>._))
            .Throws<Exception>();

        // Act
        var result = await _accountService.GetAvatarWName(userId);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.UnexpectedError);
    }

    [Fact]
    public async Task GetAvatarWName_NullAvatar_ReturnsEmptyString()
    {
        // Arrange
        int userId = 1;
        var account = new Account
        {
            Id = userId,
            FirstName = "John",
            Avatar = null
        };

        var expected = new AvatarWNameVM
        {
            Id = userId,
            Name = "John",
            Avatar = null
        };

        A.CallTo(() => _unitOfWork.Accounts.GetByIdAsync(userId, A<string>._))
            .Returns(account);

        // Act
        var result = await _accountService.GetAvatarWName(userId);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Avatar.Should().BeNull();
    }

    #endregion
}