using System.Diagnostics;
using System.Text.Json;
using SolarWatch;
using SolarWatch.Models;
using SolarWatch.Repositories;
using SolarWatch.Services;

public class SolarService : ISolarService
{
    private static readonly string _apiKey = "995d8905109d192ea8c82037049651f7";
    private readonly HttpClient _httpClient;
    private ISunInfoRepository _sunInfoRepository;
    private ICityRepository _cityRepository;
    private IJsonProcessor _jsonProcessor;

    public SolarService(HttpClient httpClient, ISunInfoRepository sunInfoRepository, ICityRepository cityRepository, IJsonProcessor jsonProcessor)
    {
        _httpClient = httpClient;
        _sunInfoRepository = sunInfoRepository;
        _cityRepository = cityRepository;
        _jsonProcessor = jsonProcessor;
    }

    public async Task<string> GetSunriseAsync(string city, DateOnly date)
    {
        var sunInfo = await GetSunInfoAsync(city, date);
        return sunInfo.GetSunrise();
    }

    public async Task<string> GetSunsetAsync(string city, DateOnly date)
    {
        var sunInfo = await GetSunInfoAsync(city, date);
        return sunInfo.GetSunset();
    }

    private async Task<SunInfo> GetSunInfoAsync(string cityName, DateOnly date)
    {
        City city = await _cityRepository.GetCityByNameAsync(cityName);
        
        if (city == null)
        {
            string geoUri = $"http://api.openweathermap.org/geo/1.0/direct?q={cityName}&limit=1&appid={_apiKey}";

            string result = await GetResponseFromUriAsync(geoUri);
            
            List<City> cities = new List<City>();
                
            cities = JsonSerializer.Deserialize<List<City>>(result);
            if (cities == null || cities.Count == 0)
            {
                throw new Exception("City not found.");
            }

            city = cities[0];
            _cityRepository.AddCity(city);
        }

        SunInfo sunInfo = _sunInfoRepository.GetSunInfo(city.Id, date);

        if (sunInfo == null)
        {
            string sunInfoUri = $"https://api.sunrise-sunset.org/json?lat={city.Latitude}&lng={city.Longitude}&date={date}";
            string sunInfoResult = await GetResponseFromUriAsync(sunInfoUri);
            
            sunInfo = _jsonProcessor.Process(sunInfoResult);
            sunInfo.CityId = city.Id;
            sunInfo.Date = date;
            
            _sunInfoRepository.AddSunInfo(sunInfo);
        }
        
        return sunInfo;
    }

    public async Task<bool> DeleteCityByNameAsync(string name)
    {
        var city = await _cityRepository.GetCityByNameAsync(name);
        if (city == null)
        {
            return false;
        }

        try
        {
            await _cityRepository.DeleteCityAsync(city);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error deleting city: {e.Message}");
            return false;
        }
    }

    public async Task<City> UpdateCityAsync(int cityId, string newName)
    {
        var city = await _cityRepository.GetCityById(cityId);

        if (city == null)
        {
            return null;
        }

        // Update the fields only if they are not null (partial update)
        if (!string.IsNullOrEmpty(newName))
        {
            city.Name = newName;
        }

        // Save the updated city using the repository
        await _cityRepository.UpdateCityAsync(city);

        return city;
    }

    public async Task<SunInfo> UpdateSunInfoAsync(int sunInfoId, int newCityId)
    {
        var sunInfo = await _sunInfoRepository.GetSunInfoById(sunInfoId);

        if (sunInfo == null)
        {
            return null;
        }

        if (newCityId > 0)
        {
            sunInfo.CityId = newCityId;
        }

        await _sunInfoRepository.UpdateSunInfoAsync(sunInfo);

        return sunInfo;
    }

    private async Task<string> GetResponseFromUriAsync(string uri)
    {
        // using is needed, because _httpClient.GetAsync(uri) returns an HttpResponseMessage object, which contains network resources - resources we don't want to leak out, or hold onto longer, then necessary; using uses IDisposable to clean up resources
        using var response = await _httpClient.GetAsync(uri);
        // code below throws an exception if status code IS NOT between 200-299
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadAsStringAsync();
    }
}