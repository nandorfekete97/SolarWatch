using SolarWatch.Models;

namespace SolarWatch.Services;

public interface ISolarService
{
    Task<string> GetSunriseAsync(string city, DateOnly date);
    Task<string> GetSunsetAsync(string city, DateOnly date);
    Task<bool> DeleteCityByNameAsync(string name);
    Task<City> UpdateCityAsync(int cityId, string  cityName);
    Task<SunInfo> UpdateSunInfoAsync(int sunInfoId, int newCityId);
}