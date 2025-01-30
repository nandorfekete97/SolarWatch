namespace SolarWatch.Services;

public interface ISolarService
{
    Task<string> GetSunriseAsync(string city, DateOnly date);
    Task<string> GetSunsetAsync(string city, DateOnly date);
}