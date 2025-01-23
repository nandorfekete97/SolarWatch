using Microsoft.AspNetCore.Mvc;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet("Get")]
    public IEnumerable<WeatherForecast> Get(DateOnly date, string city)
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = date.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                City = city
            })
            .ToArray();
    }
}