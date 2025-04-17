using AutoMapper;
using BLL.Interfaces;
using Common.Responses;
using Common.ViewModels.CityVMs;
using DAL.Repository.Core;
using Microsoft.Extensions.Logging;
using Utility.Constants;

namespace BLL.Services;
public class CityService(IUnitOfWork unitOfWork, ILogger<CityService> logger, IMapper mapper) : ICityService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<CityService> _logger = logger;
    private readonly IMapper _mapper = mapper;
    public async Task<ApiResponse<IEnumerable<CityVM>>> GetCities()
    {
        try
        {
            var cities = await _unitOfWork.Cities.GetAllAsync();

            if (!cities.Any())
                return ApiResponse<IEnumerable<CityVM>>.Failure(ResponseMessages.NoData);

            var cityVMs = _mapper.Map<IEnumerable<CityVM>>(cities);

            return ApiResponse<IEnumerable<CityVM>>.Success(cityVMs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cities");
            return ApiResponse<IEnumerable<CityVM>>.Failure(ResponseMessages.UnexpectedError);
        }
    }
}
