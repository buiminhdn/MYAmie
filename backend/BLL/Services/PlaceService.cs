using AutoMapper;
using BLL.Interfaces;
using Common.DTOs.PlaceDtos;
using Common.Pagination;
using Common.Responses;
using Common.ViewModels.PlaceVMs;
using DAL.Repository.Core;
using Microsoft.Extensions.Logging;
using Models.Core;
using Models.Places;
using System.Linq.Expressions;
using Utility;
using Utility.Constants;

namespace BLL.Services;
public class PlaceService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PlaceService> logger) : IPlaceService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<PlaceService> _logger = logger;

    #region Query

    public async Task<ApiResponse<PagedPlacesVM>> GetPlaces(FilterPlaceParams param)
    {
        try
        {
            var searchTerm = param.SearchTerm?.RemoveDiacritics() ?? string.Empty;

            Expression<Func<Place, bool>> filter = p =>
                p.Status == PlaceStatus.ACTIVATED &&
                (string.IsNullOrEmpty(searchTerm) || p.NormalizedInfo.Contains(searchTerm)) &&
                (!param.CityId.HasValue || param.CityId == 0 || (param.CityId > 0 && p.CityId == param.CityId.Value)) &&
                (!param.CategoryId.HasValue || param.CategoryId == 0 || (param.CategoryId > 0 && p.Categories.Any(c => c.Id == param.CategoryId.Value)));

            // Fetch data using the repository's GetAll method
            var places = await _unitOfWork.Places.GetAllAsync(filter, "City,Owner");

            var placeVMs = _mapper.Map<IEnumerable<PlaceVM>>(places);

            var pagedPlaces = PagedList<PlaceVM>.ToPagedList(placeVMs, param.PageNumber, param.PageSize);

            var pagedPlacesVM = new PagedPlacesVM
            {
                Places = pagedPlaces,
                Pagination = pagedPlaces.PaginationData
            };

            return ApiResponse<PagedPlacesVM>.Success(pagedPlacesVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving places with params: {@Param}", param);
            return ApiResponse<PagedPlacesVM>.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse<PagedPlacesVM>> GetUserPlaces(UserPlaceParams param)
    {
        try
        {
            Expression<Func<Place, bool>> filter = p =>
                p.Status == PlaceStatus.ACTIVATED && p.OwnerId == param.UserId;

            var places = await _unitOfWork.Places.GetAllAsync(filter, "City,Owner");

            var placeVMs = _mapper.Map<IEnumerable<PlaceVM>>(places);

            var pagedPlaces = PagedList<PlaceVM>.ToPagedList(placeVMs, param.PageNumber, param.PageSize);

            var pagedPlacesVM = new PagedPlacesVM
            {
                Places = pagedPlaces,
                Pagination = pagedPlaces.PaginationData
            };

            return ApiResponse<PagedPlacesVM>.Success(pagedPlacesVM);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error retrieving places with params: {@Param}", param);
            return ApiResponse<PagedPlacesVM>.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse<PlaceDetailVM>> GetById(int id)
    {
        try
        {
            var place = await _unitOfWork.Places.GetByIdAsync(id, "City,Owner,Categories");

            if (place == null)
                return ApiResponse<PlaceDetailVM>.Failure(ResponseMessages.PlaceNotFound);

            if (place.Status != PlaceStatus.ACTIVATED)
                return ApiResponse<PlaceDetailVM>.Failure(ResponseMessages.PlaceNotFound);

            var placeVM = _mapper.Map<PlaceDetailVM>(place);

            return ApiResponse<PlaceDetailVM>.Success(placeVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving place with ID: {Id}", id);
            return ApiResponse<PlaceDetailVM>.Failure(ResponseMessages.UnexpectedError);
        }
    }

    #endregion

    #region Command

    public async Task<ApiResponse> AddPlace(UpsertPlaceParams param)
    {
        try
        {
            var city = await _unitOfWork.Cities.GetByIdAsync(param.CityId);
            if (city == null)
                return ApiResponse.Failure(ResponseMessages.CityNotFound);

            var categories = await _unitOfWork.Categories.GetAllAsync(c => param.CategoryIds.Contains(c.Id));
            if (categories.Count() != param.CategoryIds.Count)
                return ApiResponse.Failure(ResponseMessages.CategoryNotFound);

            var place = new Place
            {
                Name = param.Name,
                ShortDescription = param.ShortDescription,
                Description = param.Description,
                NormalizedInfo = $"{param.Name} {param.ShortDescription}".RemoveDiacritics(),
                Address = param.Address,
                City = city,
                Categories = categories.ToList(),
                OwnerId = param.CurrentUserId,
                CreatedBy = param.CurrentUserId,
                Images = param.Images,
            };

            await _unitOfWork.Places.AddAsync(place);

            return await _unitOfWork.SaveAsync() > 0
                ? ApiResponse.Success(ResponseMessages.AddPlaceSuccess)
                : ApiResponse.Failure(ResponseMessages.AddPlaceFailed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding place with params: {@Param}", param);
            return ApiResponse.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse> UpdatePlace(UpsertPlaceParams param)
    {
        try
        {
            var place = await _unitOfWork.Places.GetByIdAsync(param.Id, "Categories");

            if (place == null)
                return ApiResponse.Failure(ResponseMessages.PlaceNotFound);

            if (!place.IsOwner(param.CurrentUserId) && param.CurrentUserRole != Role.ADMIN)
                return ApiResponse.Failure(ResponseMessages.PlaceNotFound);

            if (place.Status != PlaceStatus.ACTIVATED)
                return ApiResponse.Failure(ResponseMessages.PlaceNotFound);

            if (place.CityId != param.CityId)
            {
                var city = await _unitOfWork.Cities.GetByIdAsync(param.CityId);

                if (city == null)
                    return ApiResponse.Failure(ResponseMessages.CityNotFound);

                place.City = city;
            }

            var categories = await _unitOfWork.Categories.GetAllAsync(c => param.CategoryIds.Contains(c.Id));

            if (categories.Count() != param.CategoryIds.Count)
                return ApiResponse<PlaceDetailVM>.Failure(ResponseMessages.CategoryNotFound);

            place.Name = param.Name;
            place.ShortDescription = param.ShortDescription;
            place.Description = param.Description;
            place.NormalizedInfo = $"{param.Name} {param.ShortDescription}".RemoveDiacritics();
            place.Address = param.Address;
            place.UpdatedBy = param.CurrentUserId;
            place.UpdatedDate = DateTimeUtils.TimeInEpoch();
            place.Images = param.Images;

            // Update categories only if they have changed
            if (!place.Categories.Select(c => c.Id).OrderBy(id => id).SequenceEqual(param.CategoryIds.OrderBy(id => id)))
            {
                place.Categories = categories.ToList();
            }

            await _unitOfWork.Places.UpdateAsync(place);

            return await _unitOfWork.SaveAsync() > 0
                ? ApiResponse.Success(ResponseMessages.AddPlaceSuccess)
                : ApiResponse.Failure(ResponseMessages.AddPlaceFailed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating place with params: {@Param}", param);
            return ApiResponse.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse<string>> DeletePlace(DeletePlaceParams param)
    {
        try
        {
            var place = await _unitOfWork.Places.GetByIdAsync(param.Id);

            if (place == null)
                return ApiResponse<string>.Failure(ResponseMessages.PlaceNotFound);

            if (!place.IsOwner(param.CurrentUserId) && param.CurrentUserRole != Role.ADMIN)
                return ApiResponse<string>.Failure(ResponseMessages.PlaceNotFound);

            string imagePaths = place.Images;

            await _unitOfWork.Places.DeleteAsync(place);

            return await _unitOfWork.SaveAsync() > 0
                    ? ApiResponse<string>.Success(imagePaths, ResponseMessages.DeletePlaceSuccess)
                    : ApiResponse<string>.Failure(ResponseMessages.DeletePlaceFailed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting place with ID: {Id}", param.Id);
            return ApiResponse<string>.Failure(ResponseMessages.UnexpectedError);
        }
    }



    #endregion
}
