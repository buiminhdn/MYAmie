using AutoMapper;
using BLL.Services;
using Common.ViewModels.CityVMs;
using DAL.Repository.Core;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Models.Cities;
using System.Linq.Expressions;
using Utility.Constants;

namespace Tests.Services;
public class CityServiceTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CityService> _logger;
    private readonly IMapper _mapper;
    private readonly CityService _cityService;

    public CityServiceTests()
    {
        _unitOfWork = A.Fake<IUnitOfWork>();
        _logger = A.Fake<ILogger<CityService>>();
        _mapper = A.Fake<IMapper>();
        _cityService = new CityService(_unitOfWork, _logger, _mapper);
    }

    [Fact]
    public async Task GetAllCities_NoCitiesFound_ReturnsFailure()
    {
        // Arrange
        var cities = new List<City>();

        A.CallTo(() => _unitOfWork.Cities.GetAllAsync(A<Expression<Func<City, bool>>>._, A<string>._))
            .Returns(Task.FromResult(cities.AsEnumerable()));

        // Act
        var result = await _cityService.GetCities();

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.NoData);
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task GetAllCities_CitiesFound_ReturnsSuccess()
    {
        // Arrange
        var cities = new List<City>
    {
        new City { Id = 1, Name = "City 1" },
        new City { Id = 2, Name = "City 2" }
    };

        var cityVMs = new List<CityVM>
    {
        new CityVM { Id = 1, Name = "City 1" },
        new CityVM { Id = 2, Name = "City 2" }
    };

        A.CallTo(() => _unitOfWork.Cities.GetAllAsync(A<Expression<Func<City, bool>>>._, A<string>._))
            .Returns(Task.FromResult(cities.AsEnumerable()));

        A.CallTo(() => _mapper.Map<IEnumerable<CityVM>>(cities))
            .Returns(cityVMs);

        // Act
        var result = await _cityService.GetCities();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be(ResponseMessages.Success);
        result.Data.Should().BeEquivalentTo(cityVMs);
    }

    [Fact]
    public async Task GetAllCities_UnexpectedError_ReturnsFailure()
    {
        // Arrange
        A.CallTo(() => _unitOfWork.Cities.GetAllAsync(A<Expression<Func<City, bool>>>._, A<string>._))
            .Throws<Exception>(); // Simulate an unexpected error

        // Act
        var result = await _cityService.GetCities();

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.UnexpectedError);
        result.Data.Should().BeNull();
    }
}
