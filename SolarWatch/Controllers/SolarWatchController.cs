using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Models;
using SolarWatch.Services;

[ApiController]
[Route("[controller]")]
public class SolarWatchController : ControllerBase
{
    private readonly ISolarService _solarService;

    public SolarWatchController(ISolarService solarService)
    {
        _solarService = solarService;
    }
    
    [HttpGet("GetSunrise"), Authorize(Roles = "Admin")]
    public async Task<string> GetSunrise(string city, DateOnly date)
    {
        return await _solarService.GetSunriseAsync(city, date);
    }
    
    [HttpGet("GetSunset"), Authorize(Roles = "User, Admin")]
    public async Task<string> GetSunset(string city, DateOnly date)
    {
        return await _solarService.GetSunsetAsync(city, date);
    }

    [HttpDelete("DeleteCity")]
    public IActionResult DeleteCity(string name)
    {
        bool deleted = _solarService.DeleteCityByName(name).Result;

        if (!deleted)
        {
            return NotFound("City not found");
        }

        return Ok("City deleted successfully.");
    }
}