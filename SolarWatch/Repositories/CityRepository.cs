using SolarWatch.Models;

namespace SolarWatch.Repositories;

public class CityRepository : ICityRepository
{
    private SolarWatchDbContext _solarWatchDbContext;

    public CityRepository(SolarWatchDbContext solarWatchDbContext)
    {
        _solarWatchDbContext = solarWatchDbContext;
    }

    public City? GetCityByName(string name)
    {
        return _solarWatchDbContext.Cities.FirstOrDefault(city => city.Name == name);
    }

    public IEnumerable<City> GetAll()
    {
        return _solarWatchDbContext.Cities.ToList();
    }

    public void AddCity(City city)
    {
        _solarWatchDbContext.Cities.Add(city);
        _solarWatchDbContext.SaveChanges();
    }

    public void UpdateCity(City city)
    {
        _solarWatchDbContext.Update(city);
        _solarWatchDbContext.SaveChanges();
    }

    public void DeleteCity(City city)
    {
        _solarWatchDbContext.Remove(city);
        _solarWatchDbContext.SaveChanges();
    }
}