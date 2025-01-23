using System.Net;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class SolarWatchController : Controller
{
    private static readonly string _apiKey = "f10a7d6b7dadf0a0764d524896814108";
    
    [HttpGet("GetSunrise")]
    public XDocument GetSunrise(string city, DateOnly date)
    {
        string geoUri =
            $"http://api.openweathermap.org/geo/1.0/direct?q={city}&limit={1}&appid={_apiKey}";

        WebRequest request = WebRequest.Create(geoUri);
        WebResponse response = request.GetResponse();
        
        XDocument xdoc = XDocument.Load(response.GetResponseStream());
        return xdoc;
    }

    [HttpGet("GetSunset")]
    public string GetSunset(string city, DateOnly date)
    {
        return "19:45";
    }
}