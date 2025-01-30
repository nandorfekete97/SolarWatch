using System.Text.Json;
using SolarWatch.Models;
using SolarWatch.Services;

public class SolarService : ISolarService
{
    private static readonly string _apiKey = "995d8905109d192ea8c82037049651f7";
    private readonly HttpClient _httpClient;

    public SolarService(HttpClient httpClient)
    {
        _httpClient = httpClient;
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

    private async Task<SunInfo> GetSunInfoAsync(string city, DateOnly date)
    {
        string geoUri = $"http://api.openweathermap.org/geo/1.0/direct?q={city}&limit=1&appid={_apiKey}";

        string result = await GetResponseFromUriAsync(geoUri);
        var cityInfo = JsonSerializer.Deserialize<List<City>>(result);
        if (cityInfo == null || cityInfo.Count == 0)
        {
            throw new Exception("City not found.");
        }

        string sunInfoUri = $"https://api.sunrise-sunset.org/json?lat={cityInfo[0].lat}&lng={cityInfo[0].lon}&date={date}";
        string sunInfoResult = await GetResponseFromUriAsync(sunInfoUri);

        return JsonSerializer.Deserialize<SunInfo>(sunInfoResult);
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