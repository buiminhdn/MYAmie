using Common.DTOs.AdminPlaceDtos;
using Common.Responses;
using Common.ViewModels.AdminPlaceVMs;

namespace BLL.Interfaces;
public interface IAdminPlaceService
{
    Task<ApiResponse<PagedAdminPlacesVM>> GetPlacesByAdmin(AdminPlaceParams param);
    Task<ApiResponse> UpdatePlaceStatus(AdminPlaceStatusParams param);
}
