namespace SolarWatch.Models;

public class SunInfo
{
    public Results results { get; set; }
    public string tzid { get; set; }

    public string GetSunrise()
    {
        return results.sunrise + " " + tzid;
    }

    public string GetSunset()
    {
        return results.sunset + " " + tzid;
    }
    
    public class Results
    {
        public string sunrise { get; set; }
        public string sunset { get; set; }
    }
}