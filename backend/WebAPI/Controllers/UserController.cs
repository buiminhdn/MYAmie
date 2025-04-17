using BLL.Interfaces;
using Common.DTOs.UserDtos;
using Common.Validators.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Utility.Constants;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting("tokenBucketLimiter")]
public class UserController(IUserService userService, IValidationFactory validator, ILogger<UserController> logger) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly IValidationFactory _validator = validator;
    private readonly ILogger<UserController> _logger = logger;

    [HttpGet("get-users")]
    public async Task<IActionResult> GetUsers([FromQuery] FilterUserParams param)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(param);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            var response = await _userService.GetUsers(param);

            return Ok(response);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetUsers failed with parameters: {Param}", param);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }

    }

    [HttpGet("get-user-by-id")]
    public async Task<IActionResult> GetUserById([FromQuery] int id)
    {
        try
        {
            if (id <= 0)
                return BadRequest(ResponseMessages.InvalidId);

            var response = await _userService.GetUserById(id);

            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetUserById failed for Id: {Id}", id);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }

    [HttpGet("get-businesses")]
    public async Task<IActionResult> GetBusinesses([FromQuery] FilterBusinessParams param)
    {
        try
        {
            var response = await _userService.GetBusinesses(param);
            return Ok(response);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetBusinesses failed with parameters: {Param}", param);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }

    }

    [HttpGet("get-business-by-id")]
    public async Task<IActionResult> GetBusinessById([FromQuery] int id)
    {
        try
        {
            if (id <= 0)
                return BadRequest(ResponseMessages.InvalidId);

            var response = await _userService.GetBusinessById(id);

            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetBusinessById failed for Id: {Id}", id);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }
}
