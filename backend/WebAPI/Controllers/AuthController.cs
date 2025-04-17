using BLL.Interfaces;
using Common.DTOs.AuthDtos;
using Common.Responses;
using Common.Validators.Core;
using Common.ViewModels.AuthVMs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using Utility.Constants;
using WebAPI.Services.Auth;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting("tokenBucketLimiter")]
public class AuthController(IValidationFactory validator, IAuthService authService, ITokenService tokenService, IJwtTokenService jwtTokenService, ILogger<AuthController> logger) : ControllerBase
{
    private readonly IValidationFactory _validator = validator;
    private readonly IAuthService _authService = authService;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly ILogger<AuthController> _logger = logger;

    [HttpPost("sign-up")]
    public async Task<IActionResult> Signup(SignUpParams param)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(param);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            var response = await _authService.SignUp(param);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during signup for Email: {Email}", param.Email);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }

    [HttpPost("sign-up-business")]
    public async Task<IActionResult> SignupBusiness(SignUpBusinessParams param)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(param);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            var response = await _authService.SignUpBusiness(param);
            return response.IsSuccess ? Ok(response) : BadRequest(response);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during signup for Email: {Email}", param.Email);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> Login(SignInParams param)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(param);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.GetErrorMessages());

            var response = await _authService.SignIn(param);

            if (!response.IsSuccess)
                return Unauthorized(response);

            var accessToken = _jwtTokenService.GenerateAccessToken(GenerateClaims(response.Data));
            var refreshToken = _jwtTokenService.GenerateRefreshToken(param.Email);

            if (!(await _tokenService.StoreRefreshToken(response.Data.Id, refreshToken)).IsSuccess)
                return BadRequest(ApiResponse.Failure(ResponseMessages.LoginRequired));

            response.Data.AccessToken = accessToken;
            response.Data.RefreshToken = refreshToken;

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for Email: {Email}", param.Email);
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }

    #region Private Methods

    private static List<Claim> GenerateClaims(AuthAccountVM account) =>
    [
        new(ClaimTypes.NameIdentifier, account.Id.ToString()),
        new(ClaimTypes.Role, account.Role.ToString()),
        new(ClaimTypes.Name, account.Id.ToString())
    ];

    #endregion

}
