using BLL.Interfaces;
using Common.DTOs.AdminPlaceDtos;
using Common.Validators.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Utility.Constants;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting("tokenBucketLimiter")]
public class AdminPlaceController(IValidationFactory validator, IAdminPlaceService adminPlaceService, ILogger<AdminPlaceController> logger) : ControllerBase
{
    private readonly IValidationFactory _validator = validator;
    private readonly IAdminPlaceService _adminPlaceService = adminPlaceService;
    private readonly ILogger<AdminPlaceController> _logger = logger;

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll([FromQuery] AdminPlaceParams param)
    {
        try
        {
            var response = await _adminPlaceService.GetPlacesByAdmin(param);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all places for AdminId: {AdminId}", param.CurrentUserId);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }

    [HttpPut("update-status")]
    public async Task<IActionResult> UpdateStatus(AdminPlaceStatusParams param)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(param);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var response = await _adminPlaceService.UpdatePlaceStatus(param);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating place status for PlaceId: {PlaceId} by AdminId: {AdminId}", param.PlaceId, param.CurrentUserId);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }
}
