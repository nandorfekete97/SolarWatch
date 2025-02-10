using System.Text.Json.Serialization;

namespace SolarWatch.Models;

public class SunInfo
{
    public int Id { get; set; }
    
    [JsonPropertyName("sunrise")]
    public string Sunrise { get; set; }
    
    [JsonPropertyName("sunset")]
    public string Sunset { get; set; }
    public string tzid { get; set; }
    
    public int CityId { get; set; }
    
    public DateOnly Date { get; set; }

    public string GetSunrise()
    {
        return this.Sunrise + " " + tzid;
    }

    public string GetSunset()
    {
        return this.Sunset + " " + tzid;
    }
}