using System.Text.Json.Serialization;

namespace SolarWatch.Models;

public class City
{
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("lat")]
    public double Latitude { get; set; }
    
    [JsonPropertyName("lon")]
    public double Longitude { get; set; }
    
    [JsonPropertyName("country")]
    public string Country { get; set; }
}