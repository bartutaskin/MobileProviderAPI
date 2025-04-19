using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileProvider.Models.DTOs;
using MobileProvider.Services;

namespace MobileProvider.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class UsageController : ControllerBase
    {
        private readonly UsageService _usageService;

        public UsageController(UsageService usageService)
        {
            _usageService = usageService;
        }

        [HttpPost("usage")]
        [Authorize]
        public async Task<IActionResult> AddUsage(AddUsageDto dto)
        {
            var result = await _usageService.AddUsageAsync(dto);

            if (!result) 
                return BadRequest("Invalid request. Check subscriber number or usage type.");

            return Ok(new { message = "Usage recorded successfully." });
        }
    }
}
