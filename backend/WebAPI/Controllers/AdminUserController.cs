using BLL.Interfaces;
using Common.DTOs.AdminUserDtos;
using Common.Validators.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Models.Core;
using Utility.Constants;
using WebAPI.Attributes;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting("tokenBucketLimiter")]
public class AdminUserController(IValidationFactory validator, IAdminUserService adminUserService, ILogger<AdminUserController> logger) : ControllerBase
{
    private readonly IValidationFactory _validator = validator;
    private readonly IAdminUserService _adminUserService = adminUserService;
    private readonly ILogger<AdminUserController> _logger = logger;

    [HttpGet("get-all")]
    [CustomAuthorize(Role.ADMIN)]
    public async Task<IActionResult> GetAll([FromQuery] AdminUserParams param)
    {
        try
        {
            var response = await _adminUserService.GetUsersByAdmin(param);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all users for AdminId: {AdminId}", param.CurrentUserId);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }

    [HttpPut("update-status")]
    [CustomAuthorize(Role.ADMIN)]
    public async Task<IActionResult> UpdateStatus(AdminUserStatusParams param)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(param);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var response = await _adminUserService.UpdateUserStatus(param);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user status for UserId: {UserId} by AdminId: {AdminId}", param.UserId, param.CurrentUserId);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }

    [HttpPut("update-password")]
    [CustomAuthorize(Role.ADMIN)]
    public async Task<IActionResult> UpdatePassword(AdminUserPasswordParams param)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(param);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var response = await _adminUserService.UpdateUserPassword(param);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating password for UserId: {UserId} by AdminId: {AdminId}", param.UserId, param.CurrentUserId);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }
}
