using BLL.Interfaces;
using Common.DTOs.FriendshipDtos;
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
public class FriendshipController(IFriendshipService friendshipService, IValidationFactory validator, ILogger<FriendshipController> logger) : ControllerBase
{
    private readonly IFriendshipService _friendshipService = friendshipService;
    private readonly IValidationFactory _validator = validator;
    private readonly ILogger<FriendshipController> _logger = logger;

    [HttpGet("get-all-friendsips")]
    public async Task<IActionResult> GetAllFriendships([FromQuery] FilterFriendshipParams param)
    {
        try
        {
            var response = await _friendshipService.GetFriendships(param);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetAllFriendships failed for Filter: {Filter}", param);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }

    [HttpGet("get-friendship")]
    public async Task<IActionResult> GetFriendship([FromQuery] FriendRequestParams param)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(param);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            var response = await _friendshipService.GetFriendship(param);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetFriendship failed for User: {UserId}, Friend: {FriendId}", param.CurrentUserId, param.OtherUserId);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }

    [HttpPost("add-friend")]
    [CustomAuthorize(Role.USER, Role.BUSINESS)]
    public async Task<IActionResult> AddFriend([FromBody] FriendRequestParams param)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(param);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            var response = await _friendshipService.AddFriend(param);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AddFriend failed for User: {UserId}, Friend: {FriendId}", param.CurrentUserId, param.OtherUserId);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }

    }

    [HttpPost("cancel-friend")]
    [CustomAuthorize(Role.USER, Role.BUSINESS)]
    public async Task<IActionResult> CancelFriend([FromBody] FriendRequestParams param)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(param);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            var response = await _friendshipService.CancelFriend(param);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CancelFriend failed for User: {UserId}, Friend: {FriendId}", param.CurrentUserId, param.OtherUserId);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }

    }

    [HttpPost("accept-friend")]
    [CustomAuthorize(Role.USER, Role.BUSINESS)]
    public async Task<IActionResult> AcceptFriend([FromBody] FriendRequestParams param)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(param);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            var response = await _friendshipService.AcceptFriend(param);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AcceptFriend failed for User: {UserId}, Friend: {FriendId}", param.CurrentUserId, param.OtherUserId);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }

    }

    [HttpPost("remove-friend")]
    [CustomAuthorize(Role.USER, Role.BUSINESS)]
    public async Task<IActionResult> RemoveFriend([FromBody] FriendRequestParams param)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(param);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            var response = await _friendshipService.RemoveFriend(param);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RemoveFriend failed for User: {UserId}, Friend: {FriendId}", param.CurrentUserId, param.OtherUserId);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }

    }
}
