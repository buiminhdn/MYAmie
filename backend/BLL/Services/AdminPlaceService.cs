using AutoMapper;
using BLL.Interfaces;
using Common.DTOs.AdminPlaceDtos;
using Common.Pagination;
using Common.Responses;
using Common.ViewModels.AdminPlaceVMs;
using DAL.Repository.Core;
using Microsoft.Extensions.Logging;
using Models.Places;
using System.Linq.Expressions;
using Utility;
using Utility.Constants;

namespace BLL.Services;
public class AdminPlaceService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AdminPlaceService> logger) : IAdminPlaceService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<AdminPlaceService> _logger = logger;

    public async Task<ApiResponse<PagedAdminPlacesVM>> GetPlacesByAdmin(AdminPlaceParams param)
    {
        try
        {
            if (param.Status != 0 && !Enum.IsDefined(typeof(PlaceStatus), param.Status))
                return ApiResponse<PagedAdminPlacesVM>.Failure(ResponseMessages.InvalidStatus);

            // Build the filter expression
            Expression<Func<Place, bool>> filter = place =>
                (param.Status == 0 || place.Status == param.Status) &&
                (string.IsNullOrEmpty(param.SearchTerm) ||
                 place.NormalizedInfo.Contains(param.SearchTerm.RemoveDiacritics()));

            // Get paged results with includes
            var places = await _unitOfWork.Places.GetAllAsync(filter, "Owner,City");

            var placeVMs = _mapper.Map<IEnumerable<AdminPlaceVM>>(places);

            var pagedPlaces = PagedList<AdminPlaceVM>.ToPagedList(placeVMs, param.PageNumber, param.PageSize);

            var pagedPlacesVM = new PagedAdminPlacesVM
            {
                Places = pagedPlaces,
                Pagination = pagedPlaces.PaginationData
            };

            return ApiResponse<PagedAdminPlacesVM>.Success(pagedPlacesVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving places by admin");
            return ApiResponse<PagedAdminPlacesVM>.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse> UpdatePlaceStatus(AdminPlaceStatusParams param)
    {
        try
        {
            if (param.Status != 0 && !Enum.IsDefined(typeof(PlaceStatus), param.Status))
                return ApiResponse.Failure(ResponseMessages.InvalidStatus);

            var place = await _unitOfWork.Places.GetByIdAsync(param.PlaceId);

            if (place == null)
                return ApiResponse.Failure(ResponseMessages.PlaceNotFound);

            place.Status = param.Status;
            place.UpdatedBy = param.CurrentUserId;
            place.UpdatedDate = DateTimeUtils.TimeInEpoch();

            await _unitOfWork.Places.UpdateAsync(place);

            return await _unitOfWork.SaveAsync() > 0 ?
                ApiResponse.Success(ResponseMessages.UpdatePlaceSuccess)
                : ApiResponse.Failure(ResponseMessages.UpdatePlaceFailed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating place status for PlaceId {userId}", param.PlaceId);
            return ApiResponse.Failure(ResponseMessages.UnexpectedError);
        }
    }
}
