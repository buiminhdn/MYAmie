using BLL.Interfaces;
using Common.DTOs.AccountDtos;
using Common.DTOs.Core;
using Common.DTOs.EmailDtos;
using Common.Validators.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Models.Core;
using Utility;
using Utility.Constants;
using WebAPI.Services.Cache;
using WebAPI.Services.Files;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting("tokenBucketLimiter")]
public class AccountController(IAccountService accountService, IResponseCacheService cacheService, IValidationFactory validator, IFileService fileService, ILogger<AccountController> logger) : ControllerBase
{
    private readonly IResponseCacheService _cacheService = cacheService;
    private readonly IValidationFactory _validator = validator;
    private readonly IAccountService _accountService = accountService;
    private readonly IFileService _fileService = fileService;
    private readonly ILogger<AccountController> _logger = logger;

    [Authorize]
    [HttpGet("get-profile")]
    public async Task<IActionResult> GetProfile([FromQuery] BaseParams param)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(param);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            if (param.CurrentUserRole == Role.USER)
            {
                var response = await _accountService.GetUserProfile(param.CurrentUserId);
                return response.IsSuccess ? Ok(response) : BadRequest(response);
            }

            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving profile for UserId: {UserId}", param.CurrentUserId);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }

    [Authorize]
    [HttpGet("get-business-profile")]
    public async Task<IActionResult> GetBusinessProfile([FromQuery] BaseParams param)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(param);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            if (param.CurrentUserRole == Role.BUSINESS)
            {
                var response = await _accountService.GetBusinessProfile(param.CurrentUserId);
                return response.IsSuccess ? Ok(response) : BadRequest(response);
            }

            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving profile for UserId: {UserId}", param.CurrentUserId);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordParams param)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(param);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            string cacheKey = KeyUtils.ResetPassword(param.Email);

            var cacheResponse = await _cacheService.GetCacheResponseAsync(cacheKey);

            if (cacheResponse == null)
                return BadRequest(ResponseMessages.ExpiredVerificationCode);

            if (cacheResponse != param.Code)
                return BadRequest(ResponseMessages.ExpiredVerificationCode);

            var response = await _accountService.ResetPassword(param.Email, param.NewPassword);

            if (response.IsSuccess)
            {
                await _cacheService.RemoveCacheResponseAsync(cacheKey);
                return Ok(response.Message);
            }

            return BadRequest(response.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting password for Email: {Email}", param.Email);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassowrd(ChangePasswordParams param)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(param);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            var response = await _accountService.ChangePassword(param);

            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password for UserId: {UserId}", param.CurrentUserId);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }

    [Authorize]
    [HttpPut("update-profile")]
    public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileParams param, List<IFormFile> imageFiles)
    {
        try
        {
            // Validate input
            var validationResult = await _validator.ValidateAsync(param);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            if (imageFiles != null && imageFiles.Count > 10)
                return BadRequest(ResponseMessages.ImageLimitExceeded);

            var (newImagePaths, oldImagePaths) = await ProcessImagesAsync(imageFiles, param.Images);

            // Cập nhật giá trị mới vào param (giữ nguyên nếu không có ảnh mới)
            param.Images = newImagePaths.Count > 0 ? string.Join(";", newImagePaths) : param.Images;

            // Gọi service cập nhật tài khoản
            var response = await _accountService.UpdateProfile(param);

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
            _logger.LogError(ex, "Error updating profile for UserId: {UserId}", param.CurrentUserId);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }

    }

    [Authorize]
    [HttpPut("update-business-profile")]
    public async Task<IActionResult> UpdateBusinessProfile([FromForm] UpdateBusinessProfileParams param, List<IFormFile> imageFiles)
    {
        try
        {
            // Validate input
            var validationResult = await _validator.ValidateAsync(param);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            var (newImagePaths, oldImagePaths) = await ProcessImagesAsync(imageFiles, param.Images);

            // Cập nhật giá trị mới vào param (giữ nguyên nếu không có ảnh mới)
            param.Images = newImagePaths.Count > 0 ? string.Join(";", newImagePaths) : param.Images;

            // Gọi service cập nhật tài khoản
            var response = await _accountService.UpdateBusinessProfile(param);

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
            _logger.LogError(ex, "Error updating business profile for UserId: {UserId}", param.CurrentUserId);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }

    }

    [Authorize]
    [HttpPut("update-image")]
    public async Task<IActionResult> UpdateAvatarOrCover([FromForm] UpdateAvatarOrCoverParams param, IFormFile imageFile)
    {
        try
        {
            // Validate input
            var validationResult = await _validator.ValidateAsync(param);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            if (imageFile == null)
                return BadRequest("Vui lòng thêm ảnh");

            if (!_fileService.IsImageFile(imageFile))
                return BadRequest("Định dạng ảnh không hợp lệ");

            int maxWidth = param.Type == ImageTypeParam.Avatar ? ImageSizeConstants.MaxWidthAvatar : ImageSizeConstants.MaxWidthCover;
            int maxHeight = param.Type == ImageTypeParam.Avatar ? ImageSizeConstants.MaxHeightAvatar : ImageSizeConstants.MaxHeightCover;

            var saveImageResponse = await _fileService.SaveImageAsync(imageFile, maxWidth, maxHeight);

            if (!saveImageResponse.IsSuccess)
                return BadRequest(saveImageResponse.Message);

            string imagePath = saveImageResponse.Data;

            bool isAvatar = param.Type == ImageTypeParam.Avatar;

            var response = await _accountService.UpdateAvatarOrCover(param.CurrentUserId, imagePath, isAvatar);

            if (!response.IsSuccess)
            {
                _fileService.DeleteImage(imagePath);
                return BadRequest(response);
            }

            if (!string.IsNullOrEmpty(response.Data.OldPath))
                _fileService.DeleteImage(response.Data.OldPath);

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating avatar or cover for UserId: {UserId}, Type: {Type}", param.CurrentUserId, param.Type);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }

    [Authorize]
    [HttpPut("update-location")]
    public async Task<IActionResult> UpdateLocation(UpdateLocationParams param)
    {
        try
        {
            // Validate input
            var validationResult = await _validator.ValidateAsync(param);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            var response = await _accountService.UpdateLocation(param);

            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating location for UserId: {UserId}", param.CurrentUserId);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }


    [HttpGet("get-avatar-name")]
    public async Task<IActionResult> GetAvatarWName([FromQuery] int userId)
    {
        try
        {
            if (userId <= 0)
                return BadRequest("Id trống");

            var response = await _accountService.GetAvatarWName(userId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting avatar with name for UserId: {UserId}", userId);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }


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
