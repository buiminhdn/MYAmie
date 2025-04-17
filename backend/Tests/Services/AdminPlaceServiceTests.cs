using AutoMapper;
using BLL.Services;
using Common.DTOs.AdminPlaceDtos;
using Common.ViewModels.AdminPlaceVMs;
using DAL.Repository.Core;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Models.Places;
using System.Linq.Expressions;
using Utility.Constants;

namespace Tests.Services;
public class AdminPlaceServiceTests
{
    private readonly IUnitOfWork _mockUnitOfWork;
    private readonly IMapper _mockMapper;
    private readonly ILogger<AdminPlaceService> _mockLogger;
    private readonly AdminPlaceService _adminPlaceService;

    public AdminPlaceServiceTests()
    {
        _mockUnitOfWork = A.Fake<IUnitOfWork>();
        _mockMapper = A.Fake<IMapper>();
        _mockLogger = A.Fake<ILogger<AdminPlaceService>>();
        _adminPlaceService = new AdminPlaceService(_mockUnitOfWork, _mockMapper, _mockLogger);
    }

    [Fact]
    public async Task GetAllPlacesByAdmin_WhenValid_ReturnsPagedAdminPlacesVM()
    {
        // Arrange
        var param = new AdminPlaceParams
        {
            PageNumber = 1,
            PageSize = 10,
            Status = PlaceStatus.ACTIVATED,
            SearchTerm = "test"
        };

        var places = new List<Place>
            {
                new Place { Id = 1, Status = PlaceStatus.ACTIVATED, NormalizedInfo = "test" },
                new Place { Id = 2, Status = PlaceStatus.ACTIVATED, NormalizedInfo = "test" }
            };

        var placeVMs = new List<AdminPlaceVM>
            {
                new AdminPlaceVM { Id = 1 },
                new AdminPlaceVM { Id = 2 }
            };

        A.CallTo(() => _mockUnitOfWork.Places.GetAllAsync(
            A<Expression<Func<Place, bool>>>.Ignored,
            A<string>.Ignored))
            .Returns(places);

        A.CallTo(() => _mockMapper.Map<IEnumerable<AdminPlaceVM>>(places))
            .Returns(placeVMs);

        // Act
        var result = await _adminPlaceService.GetPlacesByAdmin(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Places.Should().HaveCount(2);
        result.Data.Pagination.Should().NotBeNull();
        A.CallTo(() => _mockUnitOfWork.Places.GetAllAsync(
            A<Expression<Func<Place, bool>>>.Ignored,
            A<string>.Ignored)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetAllPlacesByAdmin_WhenInvalidStatus_ReturnsFailure()
    {
        // Arrange
        var param = new AdminPlaceParams
        {
            Status = (PlaceStatus)999 // Invalid status
        };

        // Act
        var result = await _adminPlaceService.GetPlacesByAdmin(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.InvalidStatus);
    }

    [Fact]
    public async Task UpdatePlaceStatus_WhenValid_ReturnsSuccess()
    {
        // Arrange
        var param = new AdminPlaceStatusParams
        {
            PlaceId = 1,
            CurrentUserId = 2,
            Status = PlaceStatus.SUSPENDED
        };

        var place = new Place { Id = 1 };

        A.CallTo(() => _mockUnitOfWork.Places.GetByIdAsync(param.PlaceId, A<string>.Ignored))
            .Returns(place);

        A.CallTo(() => _mockUnitOfWork.SaveAsync())
            .Returns(1);

        // Act
        var result = await _adminPlaceService.UpdatePlaceStatus(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be(ResponseMessages.UpdatePlaceSuccess);
        A.CallTo(() => _mockUnitOfWork.Places.GetByIdAsync(param.PlaceId, A<string>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _mockUnitOfWork.SaveAsync()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdatePlaceStatus_WhenInvalidStatus_ReturnsFailure()
    {
        // Arrange
        var param = new AdminPlaceStatusParams
        {
            Status = (PlaceStatus)999 // Invalid status
        };

        // Act
        var result = await _adminPlaceService.UpdatePlaceStatus(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.InvalidStatus);
    }

    [Fact]
    public async Task UpdatePlaceStatus_WhenPlaceNotFound_ReturnsFailure()
    {
        // Arrange
        var param = new AdminPlaceStatusParams
        {
            PlaceId = 1,
            CurrentUserId = 2,
            Status = PlaceStatus.SUSPENDED
        };

        A.CallTo(() => _mockUnitOfWork.Places.GetByIdAsync(param.PlaceId, A<string>.Ignored))
            .Returns<Place?>(null);

        // Act
        var result = await _adminPlaceService.UpdatePlaceStatus(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.PlaceNotFound);
        A.CallTo(() => _mockUnitOfWork.Places.GetByIdAsync(param.PlaceId, A<string>.Ignored)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdatePlaceStatus_WhenSaveFails_ReturnsFailure()
    {
        // Arrange
        var param = new AdminPlaceStatusParams
        {
            PlaceId = 1,
            CurrentUserId = 2,
            Status = PlaceStatus.SUSPENDED
        };

        var place = new Place { Id = 1 };

        A.CallTo(() => _mockUnitOfWork.Places.GetByIdAsync(param.PlaceId, A<string>.Ignored))
            .Returns(place);

        A.CallTo(() => _mockUnitOfWork.SaveAsync())
            .Returns(0); // Simulate save failure

        // Act
        var result = await _adminPlaceService.UpdatePlaceStatus(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse("because the save operation failed");
        result.Message.Should().Be(ResponseMessages.UpdatePlaceFailed);
        A.CallTo(() => _mockUnitOfWork.SaveAsync()).MustHaveHappenedOnceExactly();
    }
}
