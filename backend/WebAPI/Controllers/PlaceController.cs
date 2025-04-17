using BLL.Interfaces;
using Common.DTOs.PlaceDtos;
using Common.Validators.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Models.Core;
using Utility.Constants;
using WebAPI.Attributes;
using WebAPI.Services.Files;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting("tokenBucketLimiter")]
public class PlaceController(IPlaceService placeService, IValidationFactory validator, IFileService fileService, ILogger<PlaceController> logger) : ControllerBase
{
    private readonly IPlaceService _placeService = placeService;
    private readonly IValidationFactory _validator = validator;
    private readonly IFileService _fileService = fileService;
    private readonly ILogger<PlaceController> _logger = logger;

    #region Query

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll([FromQuery] FilterPlaceParams param)
    {
        try
        {
            var response = await _placeService.GetPlaces(param);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetAll failed for Filter: {Filter}", param);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }

    }

    [HttpGet("get-by-id")]
    public async Task<IActionResult> GetById([FromQuery] int id)
    {
        try
        {
            if (id <= 0) return BadRequest("Id trống");

            var response = await _placeService.GetById(id);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetById failed for Id: {Id}", id);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }

    }

    [HttpGet("get-for-user")]
    public async Task<IActionResult> GetForUser([FromQuery] UserPlaceParams param)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(param);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            var response = await _placeService.GetUserPlaces(param);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetForUser failed for User: {UserId}", param.UserId);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }

    }

    #endregion

    #region Command

    [HttpPost("add")]
    [CustomAuthorize(Role.USER, Role.ADMIN)]
    public async Task<IActionResult> AddPlace([FromForm] UpsertPlaceParams param, List<IFormFile> imageFiles)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(param);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            if (imageFiles != null && imageFiles.Count > 10)
                return BadRequest(ResponseMessages.ImageLimitExceeded);

            var (newImagePaths, _) = await ProcessImagesAsync(imageFiles, null);

            param.Images = string.Join(";", newImagePaths);

            var response = await _placeService.AddPlace(param);

            if (!response.IsSuccess)
            {
                // Nếu thất bại, xóa ảnh mới đã lưu
                if (newImagePaths.Count > 0)
                    _fileService.DeleteImages(newImagePaths);

                return BadRequest(response);
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AddPlace failed for User: {UserId}, Place: {PlaceName}", param.CurrentUserId, param.Name);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }

    }

    [HttpPut("update")]
    [CustomAuthorize(Role.USER, Role.ADMIN)]
    public async Task<IActionResult> UpdatePlace([FromForm] UpsertPlaceParams param, List<IFormFile> imageFiles)
    {
        try
        {
            if (param.Id <= 0)
                return BadRequest("Id trống");

            var validationResult = await _validator.ValidateAsync(param);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            var (newImagePaths, oldImagePaths) = await ProcessImagesAsync(imageFiles, param.Images);

            param.Images = newImagePaths.Count > 0 ? string.Join(";", newImagePaths) : param.Images;

            var response = await _placeService.UpdatePlace(param);

            if (!response.IsSuccess)
            {
                // Nếu thất bại, xóa ảnh mới đã lưu
                if (newImagePaths.Count > 0)
                    _fileService.DeleteImages(newImagePaths);

                return BadRequest(response);
            }

            // Nếu thành công, xóa ảnh cũ
            if (oldImagePaths.Count > 0)
                _fileService.DeleteImages(oldImagePaths);

            return Ok(response);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdatePlace failed for User: {UserId}, PlaceId: {PlaceId}", param.CurrentUserId, param.Id);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }

    }

    [HttpDelete("delete")]
    [CustomAuthorize(Role.USER, Role.ADMIN)]
    public async Task<IActionResult> DeletePlace(DeletePlaceParams param)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(param);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            var response = await _placeService.DeletePlace(param);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            if (!string.IsNullOrEmpty(response.Data))
            {
                var images = response.Data.Split(';', StringSplitOptions.RemoveEmptyEntries);
                _fileService.DeleteImages(images);
            }

            return Ok(response);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DeletePlace failed for User: {UserId}, PlaceId: {PlaceId}", param.CurrentUserId, param.Id);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }

    }

    #endregion



    #region Utility Methods

    // Helper method to process images
    private async Task<(List<string> NewImagePaths, List<string> OldImagePaths)> ProcessImagesAsync(List<IFormFile> imageFiles, string existingImages)
    {
        var newImagePaths = new List<string>();
        var oldImagePaths = new List<string>();

        if (imageFiles == null || imageFiles.Count == 0)
            return (newImagePaths, oldImagePaths);

        var validImageFiles = imageFiles.Where(_fileService.IsImageFile);

        foreach (var file in validImageFiles)
        {
            var saveImageResponse = await _fileService.SaveImageAsync(file, ImageSizeConstants.DefaultMaxWidth, ImageSizeConstants.DefaultMaxHeight);
            if (saveImageResponse.IsSuccess)
            {
                newImagePaths.Add(saveImageResponse.Data);
            }
            else
            {
                _logger.LogError("Failed to save image: {FileName}. Error: {ErrorMessage}", file.FileName, saveImageResponse.Message);
            }
        }

        if (!string.IsNullOrEmpty(existingImages))
        {
            oldImagePaths = [.. existingImages.Split(';', StringSplitOptions.RemoveEmptyEntries)];
        }

        return (newImagePaths, oldImagePaths);
    }

    #endregion
}
