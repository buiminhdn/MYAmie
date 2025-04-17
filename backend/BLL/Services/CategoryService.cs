using AutoMapper;
using BLL.Interfaces;
using Common.Responses;
using Common.ViewModels.CategoryVMs;
using DAL.Repository.Core;
using Microsoft.Extensions.Logging;
using Utility.Constants;

namespace BLL.Services;
public class CategoryService(IUnitOfWork unitOfWork, ILogger<CategoryService> logger, IMapper mapper) : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<CategoryService> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task<ApiResponse<IEnumerable<CategoryVM>>> GetCategories()
    {
        try
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();

            if (!categories.Any())
                return ApiResponse<IEnumerable<CategoryVM>>.Failure(ResponseMessages.NoData);

            var categoryVMs = _mapper.Map<IEnumerable<CategoryVM>>(categories);

            return ApiResponse<IEnumerable<CategoryVM>>.Success(categoryVMs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting categories");
            return ApiResponse<IEnumerable<CategoryVM>>.Failure(ResponseMessages.UnexpectedError);
        }
    }
}
