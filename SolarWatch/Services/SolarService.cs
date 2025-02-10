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
        City city = _cityRepository.GetCityByName(cityName);
        
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

    private async Task<string> GetResponseFromUriAsync(string uri)
    {
        // using is needed, because _httpClient.GetAsync(uri) returns an HttpResponseMessage object, which contains network resources - resources we don't want to leak out, or hold onto longer, then necessary; using uses IDisposable to clean up resources
        using var response = await _httpClient.GetAsync(uri);
        // code below throws an exception if status code IS NOT between 200-299
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadAsStringAsync();
    }
}