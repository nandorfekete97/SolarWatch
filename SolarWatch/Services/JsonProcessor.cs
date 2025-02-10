using System.Text.Json;
using SolarWatch.Models;

namespace SolarWatch.Services;

public class JsonProcessor : IJsonProcessor
{
    public SunInfo Process(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement results = json.RootElement.GetProperty("results");

        return new SunInfo
        {
            Sunrise = results.GetProperty("sunrise").GetString(),
            Sunset = results.GetProperty("sunset").GetString(),
            tzid = json.RootElement.GetProperty("tzid").GetString()
        };
    }
}