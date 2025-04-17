using BLL.Interfaces;
using Common.DTOs.FeedbackDtos;
using Common.Validators.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Models.Core;
using MYAmie.Common.DTOs.FeedbackDtos;
using Utility.Constants;
using WebAPI.Attributes;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting("tokenBucketLimiter")]
public class FeedbackController(IFeedbackService feedbackService, ILogger<FeedbackController> logger, IValidationFactory validator) : ControllerBase
{
    private readonly IFeedbackService _feedbackService = feedbackService;
    private readonly IValidationFactory _validator = validator;
    private readonly ILogger<FeedbackController> _logger = logger;

    [HttpGet("get-by-id")]
    public async Task<IActionResult> GetFeedbacks([FromQuery] FilterFeedbackParams param)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(param);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            var response = await _feedbackService.GetFeedbacks(param);

            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetFeedbacks failed for Filter: {Filter}", param);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }

    [HttpPost("add")]
    [CustomAuthorize(Role.USER)]
    public async Task<IActionResult> AddFeedback(AddFeedbackParams param)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(param);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            var response = await _feedbackService.AddFeedback(param);

            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AddFeedback failed for User: {UserId}", param.CurrentUserId);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }

    [HttpPost("update")]
    [CustomAuthorize(Role.USER)]
    public async Task<IActionResult> UpdateFeedback(UpdateFeedbackParams param)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(param);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            var response = await _feedbackService.UpdateFeedback(param);

            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateFeedback failed for FeedbackId: {FeedbackId}", param.Id);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }

    [HttpPost("delete")]
    [CustomAuthorize(Role.USER)]
    public async Task<IActionResult> DeleteFeedback(DeleteFeedbackParams param)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(param);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            var response = await _feedbackService.DeleteFeedback(param);

            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DeleteFeedback failed for FeedbackId: {FeedbackId}", param.Id);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }

    [HttpPost("response")]
    [CustomAuthorize(Role.USER, Role.BUSINESS)]
    public async Task<IActionResult> ResponseFeedback(ResponseFeedbackParams param)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(param);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            var response = await _feedbackService.ResponseFeedback(param);

            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ResponseFeedback failed for FeedbackId: {FeedbackId}", param.Id);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }

    }
}
