using BLL.Interfaces;
using Common.DTOs.Core;
using Common.DTOs.TokenDtos;
using Common.Responses;
using Common.Validators.Core;
using Common.ViewModels.TokenVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using Utility.Constants;
using WebAPI.Services.Auth;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting("tokenBucketLimiter")]
public class TokenController(IValidationFactory validator, ITokenService tokenService, IJwtTokenService jwtTokenService, ILogger<TokenController> logger) : ControllerBase
{
    private readonly IValidationFactory _validator = validator;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly ILogger<TokenController> _logger = logger;

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(RefreshTokenParams param)
    {
        int accountId = 0;

        try
        {
            var validationResult = await _validator.ValidateAsync(param);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            var principal = _jwtTokenService.GetPrincipalFromExpiredToken(param.AccessToken);
            if (principal == null)
                return BadRequest(ResponseMessages.LoginRequired);

            var accountIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (accountIdClaim == null || !int.TryParse(accountIdClaim.Value, out accountId) || accountId <= 0)
                return BadRequest(ResponseMessages.LoginRequired);

            // Return an Email
            var response = await _tokenService.IsRefreshTokenValid(accountId, param.RefreshToken);

            if (!response.IsSuccess)
                return Unauthorized(response.Message);

            var newToken = _jwtTokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken(response.Data);

            if (!(await _tokenService.StoreRefreshToken(accountId, newRefreshToken)).IsSuccess)
                return BadRequest(ApiResponse.Failure(ResponseMessages.StoreRefreshTokenFailed));

            return Ok(new TokenAuthVM
            {
                AccessToken = newToken,
                RefreshToken = newRefreshToken
            });

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RefreshToken failed for AccountId: {AccountId}", accountId);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }

    [Authorize]
    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken(BaseParams param)
    {
        try
        {
            var response = await _tokenService.RevokeRefreshToken(param.CurrentUserId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RevokeToken failed for UserId: {UserId}", param.CurrentUserId);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }

    }
}
