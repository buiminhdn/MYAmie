using BLL.Interfaces;
using Common.DTOs.AdminUserDtos;
using Common.DTOs.EmailDtos;
using Common.Validators.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Models.Core;
using Utility;
using Utility.Constants;
using WebAPI.Attributes;
using WebAPI.Services.Cache;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting("tokenBucketLimiter")]
public class EmailController(IEmailService emailService, IResponseCacheService cacheService, IValidationFactory validator, ILogger<EmailController> logger) : ControllerBase
{
    private readonly IResponseCacheService _cacheService = cacheService;
    private readonly IValidationFactory _validator = validator;
    private readonly IEmailService _emailService = emailService;
    private readonly ILogger<EmailController> _logger = logger;

    [HttpPost("request-verification")]
    public async Task<IActionResult> RequestVerification(RequestVerifyParams param)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(param);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            int verificationCode = NumberUtils.GenerateVerificationCode();

            // Choose the correct cache key based on the verification type
            string cacheKey = param.Type == VerificationTypeParam.AccountConfirmation
                ? KeyUtils.EmailVerificationCode(param.Email)
                : KeyUtils.ResetPassword(param.Email);

            var response = await _emailService.SendVerifyEmail(param.Email, verificationCode.ToString(), param.Type);

            if (!response.IsSuccess)
            {
                return BadRequest(response.Message);
            }

            await _cacheService.SetCacheResponseAsync(cacheKey, verificationCode, TimeSpan.FromMinutes(30));
            return Ok(response.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RequestVerification failed for Email: {Email}", param.Email);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }

    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail(VerifyEmailParams param)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(param);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            string cacheKey = KeyUtils.EmailVerificationCode(param.Email);

            var cacheResponse = await _cacheService.GetCacheResponseAsync(cacheKey);

            if (cacheResponse == null)
                return BadRequest(ResponseMessages.ExpiredVerificationCode);

            if (cacheResponse != param.Code)
                return BadRequest(ResponseMessages.ExpiredVerificationCode);

            var response = await _emailService.SetEmailVerified(param.Email);

            if (response.IsSuccess)
            {
                await _cacheService.RemoveCacheResponseAsync(cacheKey);
                return Ok(response.Message);
            }

            return BadRequest(response.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "VerifyEmail failed for Email: {Email}, Code: {Code}", param.Email, param.Code);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }

    [HttpPost("add-marketing-email")]
    [CustomAuthorize(Role.ADMIN)]
    public async Task<IActionResult> CreateMarketingEmail([FromBody] AddMarketingEmailParams param)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(param);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            // Create and store the marketing email via the EmailService
            var response = await _emailService.AddMarketingEmail(param.Subject, param.Body);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating marketing email.");
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }

    [HttpGet("get-marketing-emails")]
    [CustomAuthorize(Role.ADMIN)]
    public async Task<IActionResult> GetMarketingEmails([FromQuery] EmailMarketingParams param)
    {
        try
        {
            var response = await _emailService.GetEmailMarketings(param);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all marketing emails for AdminId: {AdminId}", param.CurrentUserId);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }

    [HttpDelete("delete-marketing-email")]
    [CustomAuthorize(Role.ADMIN)]
    public async Task<IActionResult> DeleteMarketingEmail(DeleteMarketingEmailParams param)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(param);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            var response = await _emailService.DeleteMarketingEmail(param.Id);

            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DeleteMarketingEmail failed for Email ID: {ID}", param.Id);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }

    }
}
