using Microsoft.AspNetCore.Mvc;
using SolarWatch.Models;

namespace SolarWatch.Repositories;

public interface ICityRepository
{
    public Task<City?> GetCityById(int id);
    public Task<City?> GetCityByNameAsync(string name);
     public IEnumerable<City> GetAll();
    public void AddCity(City city);
    public Task UpdateCityAsync(City city);
    public Task DeleteCityAsync(City city);
}