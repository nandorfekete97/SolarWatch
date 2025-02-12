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
    
    [HttpGet("GetSunrise")]
    public async Task<string> GetSunrise(string city, DateOnly date)
    {
        return await _solarService.GetSunriseAsync(city, date);
    }
    
    [HttpGet("GetSunset")]
    public async Task<string> GetSunset(string city, DateOnly date)
    {
        return await _solarService.GetSunsetAsync(city, date);
    }
}