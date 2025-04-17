using AutoMapper;
using BLL.Services;
using Common.ViewModels.CategoryVMs;
using DAL.Repository.Core;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Models.Categories;
using System.Linq.Expressions;
using Utility.Constants;

namespace Tests.Services;
public class CategoryServiceTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoryService> _logger;
    private readonly IMapper _mapper;
    private readonly CategoryService _categoryService;

    public CategoryServiceTests()
    {
        _unitOfWork = A.Fake<IUnitOfWork>();
        _logger = A.Fake<ILogger<CategoryService>>();
        _mapper = A.Fake<IMapper>();
        _categoryService = new CategoryService(_unitOfWork, _logger, _mapper);
    }

    [Fact]
    public async Task GetAllCategories_NoCategoriesFound_ReturnsFailure()
    {
        // Arrange
        var categories = new List<Category>();

        A.CallTo(() => _unitOfWork.Categories.GetAllAsync(A<Expression<Func<Category, bool>>>._, A<string>._))
            .Returns(Task.FromResult(categories.AsEnumerable()));

        // Act
        var result = await _categoryService.GetCategories();

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.NoData);
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task GetAllCategories_CategoriesFound_ReturnsSuccess()
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Category 1" },
            new Category { Id = 2, Name = "Category 2" }
        };

        var categoryVMs = new List<CategoryVM>
        {
            new CategoryVM { Id = 1, Name = "Category 1" },
            new CategoryVM { Id = 2, Name = "Category 2" }
        };

        A.CallTo(() => _unitOfWork.Categories.GetAllAsync(A<Expression<Func<Category, bool>>>._, A<string>._))
            .Returns(Task.FromResult(categories.AsEnumerable()));

        A.CallTo(() => _mapper.Map<IEnumerable<CategoryVM>>(categories))
            .Returns(categoryVMs);

        // Act
        var result = await _categoryService.GetCategories();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be(ResponseMessages.Success);
        result.Data.Should().BeEquivalentTo(categoryVMs);
    }

    [Fact]
    public async Task GetAllCategories_UnexpectedError_ReturnsFailure()
    {
        // Arrange
        A.CallTo(() => _unitOfWork.Categories.GetAllAsync(A<Expression<Func<Category, bool>>>._, A<string>._))
            .Throws<Exception>(); // Simulate an unexpected error

        // Act
        var result = await _categoryService.GetCategories();

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(ResponseMessages.UnexpectedError);
        result.Data.Should().BeNull();
    }
}
