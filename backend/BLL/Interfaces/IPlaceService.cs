using Common.DTOs.PlaceDtos;
using Common.Responses;
using Common.ViewModels.PlaceVMs;

namespace BLL.Interfaces;
public interface IPlaceService
{
    Task<ApiResponse<PagedPlacesVM>> GetPlaces(FilterPlaceParams param);
    Task<ApiResponse<PagedPlacesVM>> GetUserPlaces(UserPlaceParams param);
    Task<ApiResponse<PlaceDetailVM>> GetById(int id);
    Task<ApiResponse> AddPlace(UpsertPlaceParams param);
    Task<ApiResponse> UpdatePlace(UpsertPlaceParams param);
    Task<ApiResponse<string>> DeletePlace(DeletePlaceParams param);
}
