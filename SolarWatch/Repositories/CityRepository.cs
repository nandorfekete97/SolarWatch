using SolarWatch.Models;
using Microsoft.EntityFrameworkCore; // Needed for async DB operations

namespace SolarWatch.Repositories;

public class CityRepository : ICityRepository
{
    private SolarWatchDbContext _solarWatchDbContext;

    public CityRepository(SolarWatchDbContext solarWatchDbContext)
    {
        _solarWatchDbContext = solarWatchDbContext;
    }

    public async Task<City?> GetCityById(int id)
    {
        return await _solarWatchDbContext.Cities
            .FirstOrDefaultAsync(city => city.Id == id); 
    }

    public async Task<City?> GetCityByNameAsync(string name)
    {
        return await _solarWatchDbContext.Cities
            .FirstOrDefaultAsync(city => city.Name == name);
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

    public async Task UpdateCityAsync(City city)
    {
        _solarWatchDbContext.Update(city);
        await _solarWatchDbContext.SaveChangesAsync();
    }

    public async Task DeleteCityAsync(City city)
    {
        _solarWatchDbContext.Cities.Remove(city);
        await _solarWatchDbContext.SaveChangesAsync(); 
    }
}