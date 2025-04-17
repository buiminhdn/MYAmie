using BLL.Interfaces;
using Common.DTOs.ChatDtos;
using Common.Validators.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Utility.Constants;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting("tokenBucketLimiter")]
public class ChatController(IChatService chatService, IValidationFactory validator, ILogger<ChatController> logger) : ControllerBase
{
    private readonly IChatService _chatService = chatService;
    private readonly IValidationFactory _validator = validator;
    private readonly ILogger<ChatController> _logger = logger;

    [Authorize]
    [HttpGet("get-conversations")]
    public async Task<IActionResult> GetConversations([FromQuery] PagedConversationParams param)
    {
        try
        {
            var response = await _chatService.GetConversations(param);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving conversations for UserID {userId}.", param.CurrentUserId);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }

    [Authorize]
    [HttpGet("get-messages")]
    public async Task<IActionResult> GetMessages([FromQuery] PagedMessageParams param)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(param);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            var response = await _chatService.GetMessages(param);
            return Ok(response);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving messages for UserID {userId}.", param.CurrentUserId);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }
}
