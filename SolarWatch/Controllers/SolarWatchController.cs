using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Contracts;
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
    public async Task<string> GetSunrise([FromQuery] string city, [FromQuery]DateOnly date)
    {
        return await _solarService.GetSunriseAsync(city, date);
    }
    
    [HttpGet("GetSunset"), Authorize(Roles = "User, Admin")]
    public async Task<string> GetSunset([FromQuery] string city, [FromQuery]DateOnly date)
    {
        return await _solarService.GetSunsetAsync(city, date);
    }

    [HttpDelete("DeleteCity"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCity(string name)
    {
        bool deleted = await _solarService.DeleteCityByNameAsync(name);

        if (!deleted)
        {
            return NotFound(new { message = "City not found." });
        }

        return Ok(new { message = "City deleted successfully." });
    }

    [HttpPatch("UpdateCity/{cityId}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateCity(int cityId, [FromBody] CityUpdateDto updatedCity)
    {
        var city = await _solarService.UpdateCityAsync(cityId, updatedCity.NewCityName);

        if (city == null)
        {
            return NotFound(new { message = "City not found." });
        }

        return Ok(new { message = "City updated successfully." });
    }

    [HttpPatch("UpdateSunInfo/{sunInfoId}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateSunInfo(int sunInfoId, [FromBody] SunInfoUpdateDto updatedSunInfo)
    {
        var sunInfo = await _solarService.UpdateSunInfoAsync(sunInfoId, updatedSunInfo.newCityId);

        if (sunInfo == null)
        {
            return NotFound(new { message = "City not found." });
        }

        return Ok(new { message = "City updated successfully." });
    }
}