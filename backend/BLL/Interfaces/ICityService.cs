using Common.Responses;
using Common.ViewModels.CityVMs;

namespace BLL.Interfaces;
public interface ICityService
{
    Task<ApiResponse<IEnumerable<CityVM>>> GetCities();
}
