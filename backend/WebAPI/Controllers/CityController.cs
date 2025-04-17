using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Utility.Constants;
using WebAPI.Attributes;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting("tokenBucketLimiter")]
public class CityController(ICityService cityService, ILogger<CityController> logger) : ControllerBase
{
    private readonly ICityService _cityService = cityService;
    private readonly ILogger<CityController> _logger = logger;

    [HttpGet("get-all")]
    [Cache(3600)]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var response = await _cityService.GetCities();
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving cities.");
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }
}
