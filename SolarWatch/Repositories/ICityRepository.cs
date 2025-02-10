using SolarWatch.Models;

namespace SolarWatch.Repositories;

public interface ICityRepository
{
    public City GetCityByName(string name);
    public IEnumerable<City> GetAll();
    public void AddCity(City city);
    public void UpdateCity(City city);
    public void DeleteCity(City city);
}