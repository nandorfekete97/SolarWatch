using System.Net;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using SolarWatch.Models;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class SolarWatchController : Controller
{
    private static readonly string _apiKey = "995d8905109d192ea8c82037049651f7";
    
    [HttpGet("GetSunrise")]
    public string GetSunrise(string city, DateOnly date)
    {
        return GetSunInfo(city, date).GetSunrise();
    }

    [HttpGet("GetSunset")]
    public string GetSunset(string city, DateOnly date)
    {
        return GetSunInfo(city, date).GetSunset();
    }

    private SunInfo GetSunInfo(string city, DateOnly date)
    {
        string geoUri =
            $"http://api.openweathermap.org/geo/1.0/direct?q={city}&limit={1}&appid={_apiKey}";

        string result = GetResponseFromUri(geoUri);

        List<City> cityInfo = JsonSerializer.Deserialize<List<City>>(result);

        string sunInfoUri =
            $"https://api.sunrise-sunset.org/json?lat={cityInfo[0].lat}&lng={cityInfo[0].lon}&date={date}";

        string sunInfoResult = GetResponseFromUri(sunInfoUri);
        SunInfo sunInfo = JsonSerializer.Deserialize<SunInfo>(sunInfoResult);

        return sunInfo;
    }

    private string GetResponseFromUri(string uri)
    {
        WebRequest request = WebRequest.Create(uri);
        WebResponse response = request.GetResponse();

        using (var reader = new StreamReader(response.GetResponseStream()))
        {
            return reader.ReadToEnd();
        }
    }
}