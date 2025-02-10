using SolarWatch.Models;

namespace SolarWatch.Services;

public interface IJsonProcessor
{
    SunInfo Process(string jsonData);
}