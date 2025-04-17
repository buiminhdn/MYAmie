using AutoMapper;
using BLL.Services;
using Common.DTOs.PlaceDtos;
using Common.ViewModels.CategoryVMs;
using Common.ViewModels.CityVMs;
using Common.ViewModels.PlaceVMs;
using DAL.Repository.Core;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Models.Accounts;
using Models.Categories;
using Models.Cities;
using Models.Core;
using Models.Places;
using System.Linq.Expressions;
using Utility.Constants;

namespace Tests.Services;
public class PlaceServiceTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<PlaceService> _logger;
    private readonly PlaceService _placeService;

    public PlaceServiceTests()
    {
        _unitOfWork = A.Fake<IUnitOfWork>();
        _mapper = A.Fake<IMapper>();
        _logger = A.Fake<ILogger<PlaceService>>();
        _placeService = new PlaceService(_unitOfWork, _mapper, _logger);
    }

    #region GetAllPlaces

    [Fact]
    public async Task GetAllPlaces_WithValidParams_ReturnsPagedPlaces()
    {
        // Arrange
        var filterParams = new FilterPlaceParams
        {
            SearchTerm = "cafe",
            CityId = 1,
            CategoryId = 2,
            PageNumber = 1,
            PageSize = 10
        };

        var places = new List<Place>
    {
        new Place { Id = 1, Name = "Cafe A", Status = PlaceStatus.ACTIVATED },
        new Place { Id = 2, Name = "Cafe B", Status = PlaceStatus.ACTIVATED }
    };

        var placeVMs = new List<PlaceVM>
    {
        new PlaceVM { Id = 1, Name = "Cafe A" },
        new PlaceVM { Id = 2, Name = "Cafe B" }
    };

        A.CallTo(() => _unitOfWork.Places.GetAllAsync(A<Expression<Func<Place, bool>>>._, A<string>._))
            .Returns(places);

        A.CallTo(() => _mapper.Map<IEnumerable<PlaceVM>>(places))
            .Returns(placeVMs);

        // Act
        var result = await _placeService.GetPlaces(filterParams);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Places.Should().HaveCount(2);
        result.Data.Pagination.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAllPlaces_NoPlacesFound_ReturnsEmptyList()
    {
        // Arrange
        var filterParams = new FilterPlaceParams
        {
            SearchTerm = "cafe",
            CityId = 1,
            CategoryId = 2,
            PageNumber = 1,
            PageSize = 10
        };

        var places = new List<Place>();

        A.CallTo(() => _unitOfWork.Places.GetAllAsync(A<Expression<Func<Place, bool>>>._, A<string>._))
            .Returns(places);

        A.CallTo(() => _mapper.Map<IEnumerable<PlaceVM>>(places))
            .Returns(new List<PlaceVM>());

        // Act
        var result = await _placeService.GetPlaces(filterParams);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Places.Should().BeEmpty();
    }

    #endregion

    #region GetAllUserPlaces

    [Fact]
    public async Task GetAllUserPlaces_WithValidParams_ReturnsPagedPlaces()
    {
        // Arrange
        var userParams = new UserPlaceParams
        {
            UserId = 1,
            PageNumber = 1,
            PageSize = 10
        };

        var places = new List<Place>
    {
        new Place { Id = 1, Name = "Cafe A", Status = PlaceStatus.ACTIVATED, OwnerId = 1 },
        new Place { Id = 2, Name = "Cafe B", Status = PlaceStatus.ACTIVATED, OwnerId = 1 }
    };

        var placeVMs = new List<PlaceVM>
    {
        new PlaceVM { Id = 1, Name = "Cafe A" },
        new PlaceVM { Id = 2, Name = "Cafe B" }
    };

        A.CallTo(() => _unitOfWork.Places.GetAllAsync(A<Expression<Func<Place, bool>>>._, A<string>._))
            .Returns(places);

        A.CallTo(() => _mapper.Map<IEnumerable<PlaceVM>>(places))
            .Returns(placeVMs);

        // Act
        var result = await _placeService.GetUserPlaces(userParams);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Places.Should().HaveCount(2);
        result.Data.Pagination.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAllUserPlaces_NoPlacesFound_ReturnsEmptyList()
    {
        // Arrange
        var userParams = new UserPlaceParams
        {
            UserId = 1,
            PageNumber = 1,
            PageSize = 10
        };

        var places = new List<Place>();

        A.CallTo(() => _unitOfWork.Places.GetAllAsync(A<Expression<Func<Place, bool>>>._, A<string>._))
            .Returns(places);

        A.CallTo(() => _mapper.Map<IEnumerable<PlaceVM>>(places))
            .Returns(new List<PlaceVM>());

        // Act
        var result = await _placeService.GetUserPlaces(userParams);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Places.Should().BeEmpty();
    }

    #endregion

    #region GetById

    [Fact]
    public async Task GetById_ValidId_ReturnsPlaceDetail()
    {
        // Arrange
        var placeId = 1;
        var place = new Place
        {
            Id = placeId,
            Name = "Cafe A",
            Status = PlaceStatus.ACTIVATED,
            City = new City { Id = 1, Name = "City A" },
            Owner = new Account { Id = 1, FirstName = "Owner A" },
            Categories = [new Category { Id = 1, Name = "Cafe" }]
        };

        var placeVM = new PlaceDetailVM
        {
            Id = placeId,
            Name = "Cafe A",
            City = new CityVM { Id = 1, Name = "City A" },
            OwnerId = 1,
            OwnerName = "Owner A",
            Categories = new List<CategoryVM> { new CategoryVM { Id = 1, Name = "Cafe" } }
        };

        A.CallTo(() => _unitOfWork.Places.GetByIdAsync(placeId, "City,Owner,Categories"))
            .Returns(place);

        A.CallTo(() => _mapper.Map<PlaceDetailVM>(place))
            .Returns(placeVM);

        // Act
        var result = await _placeService.GetById(placeId);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(placeVM);
    }

    [Fact]
    public async Task GetById_InvalidId_ReturnsNotFound()
    {
        // Arrange
        var placeId = 999;

        A.CallTo(() => _unitOfWork.Places.GetByIdAsync(placeId, "City,Owner,Categories"))
            .Returns<Place?>(null);

        // Act
        var result = await _placeService.GetById(placeId);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.PlaceNotFound);
    }

    #endregion

    #region AddPlace

    [Fact]
    public async Task AddPlace_ValidParams_ReturnsSuccess()
    {
        // Arrange
        var param = new UpsertPlaceParams
        {
            Name = "Cafe A",
            ShortDescription = "A cozy cafe",
            Description = "A cozy cafe with great coffee",
            CityId = 1,
            CategoryIds = new List<int> { 1, 2 },
            CurrentUserId = 1,
            Images = "image1.jpg;image2.jpg"
        };

        var city = new City { Id = 1, Name = "City A" };
        var categories = new List<Category>
    {
        new Category { Id = 1, Name = "Cafe" },
        new Category { Id = 2, Name = "Restaurant" }
    };

        A.CallTo(() => _unitOfWork.Cities.GetByIdAsync(param.CityId, A<string>._))
            .Returns(city);

        A.CallTo(() => _unitOfWork.Categories.GetAllAsync(A<Expression<Func<Category, bool>>>._, A<string>._))
            .Returns(categories);

        A.CallTo(() => _unitOfWork.SaveAsync())
            .Returns(1); // Simulate successful save

        // Act
        var result = await _placeService.AddPlace(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be(ResponseMessages.AddPlaceSuccess);
    }

    [Fact]
    public async Task AddPlace_CityNotFound_ReturnsFailure()
    {
        // Arrange
        var param = new UpsertPlaceParams
        {
            CityId = 999, // Invalid city ID
            CategoryIds = new List<int> { 1, 2 },
            CurrentUserId = 1
        };

        A.CallTo(() => _unitOfWork.Cities.GetByIdAsync(param.CityId, A<string>._))
            .Returns<City?>(null);

        // Act
        var result = await _placeService.AddPlace(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.CityNotFound);
    }

    [Fact]
    public async Task AddPlace_CategoryNotFound_ReturnsFailure()
    {
        // Arrange
        var param = new UpsertPlaceParams
        {
            CityId = 1,
            CategoryIds = new List<int> { 1, 999 }, // Invalid category ID
            CurrentUserId = 1
        };

        var city = new City { Id = 1, Name = "City A" };
        var categories = new List<Category> { new Category { Id = 1, Name = "Cafe" } };

        A.CallTo(() => _unitOfWork.Cities.GetByIdAsync(param.CityId, A<string>._))
            .Returns(city);

        A.CallTo(() => _unitOfWork.Categories.GetAllAsync(A<Expression<Func<Category, bool>>>._, A<string>._))
            .Returns(categories);

        // Act
        var result = await _placeService.AddPlace(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.CategoryNotFound);
    }

    #endregion

    #region UpdatePlace

    [Fact]
    public async Task UpdatePlace_ValidParams_ReturnsSuccess()
    {
        // Arrange
        var param = new UpsertPlaceParams
        {
            Id = 1,
            Name = "Updated Cafe",
            ShortDescription = "Updated description",
            CityId = 1,
            CategoryIds = new List<int> { 1, 2 },
            CurrentUserId = 1,
            CurrentUserRole = Role.USER
        };

        var place = new Place
        {
            Id = 1,
            Name = "Cafe A",
            Status = PlaceStatus.ACTIVATED,
            CityId = 1,
            OwnerId = 1,
            Categories = new List<Category> { new Category { Id = 1, Name = "Cafe" } },
            CreatedBy = 1,
        };

        var city = new City { Id = 1, Name = "City A" };
        var categories = new List<Category>
    {
        new Category { Id = 1, Name = "Cafe" },
        new Category { Id = 2, Name = "Restaurant" }
    };

        var placeVM = new PlaceDetailVM { Id = 1, Name = "Updated Cafe" };

        A.CallTo(() => _unitOfWork.Places.GetByIdAsync(param.Id, "Categories"))
            .Returns(place);

        A.CallTo(() => _unitOfWork.Cities.GetByIdAsync(param.CityId, A<string>._))
            .Returns(city);

        A.CallTo(() => _unitOfWork.Categories.GetAllAsync(A<Expression<Func<Category, bool>>>._, A<string>._))
            .Returns(categories);

        A.CallTo(() => _mapper.Map<PlaceDetailVM>(place))
            .Returns(placeVM);

        A.CallTo(() => _unitOfWork.SaveAsync())
            .Returns(1);

        // Act
        var result = await _placeService.UpdatePlace(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be(ResponseMessages.AddPlaceSuccess);
    }

    [Fact]
    public async Task UpdatePlace_PlaceNotFound_ReturnsFailure()
    {
        // Arrange
        var param = new UpsertPlaceParams { Id = 999 }; // Invalid place ID

        A.CallTo(() => _unitOfWork.Places.GetByIdAsync(param.Id, "Categories"))
            .Returns<Place?>(null);

        // Act
        var result = await _placeService.UpdatePlace(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.PlaceNotFound);
    }

    #endregion

    #region DeletePlace

    [Fact]
    public async Task DeletePlace_ValidParams_ReturnsSuccess()
    {
        // Arrange
        var param = new DeletePlaceParams
        {
            Id = 1,
            CurrentUserId = 1,
            CurrentUserRole = Role.USER
        };

        var place = new Place
        {
            Id = 1,
            OwnerId = 1,
            Images = "image1.jpg;image2.jpg",
            CreatedBy = 1,
        };

        A.CallTo(() => _unitOfWork.Places.GetByIdAsync(param.Id, A<string>._))
            .Returns(place);

        A.CallTo(() => _unitOfWork.SaveAsync())
            .Returns(1); // Simulate successful save

        // Act
        var result = await _placeService.DeletePlace(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be(ResponseMessages.DeletePlaceSuccess);
        result.Data.Should().Be(place.Images);
    }

    [Fact]
    public async Task DeletePlace_PlaceNotFound_ReturnsFailure()
    {
        // Arrange
        var param = new DeletePlaceParams { Id = 999 }; // Invalid place ID

        A.CallTo(() => _unitOfWork.Places.GetByIdAsync(param.Id, A<string>._))
            .Returns<Place?>(null);

        // Act
        var result = await _placeService.DeletePlace(param);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.PlaceNotFound);
    }

    #endregion
}


