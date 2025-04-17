using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Utility.Constants;
using WebAPI.Attributes;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting("tokenBucketLimiter")]
public class CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger) : ControllerBase
{
    private readonly ICategoryService _categoryService = categoryService;
    private readonly ILogger<CategoryController> _logger = logger;

    [HttpGet("get-all")]
    [Cache(3600)]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var response = await _categoryService.GetCategories();
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving categories.");
            return StatusCode(500, ResponseMessages.UnexpectedError);
        }
    }

}