using SolarWatch.Models;

namespace SolarWatch.Repositories;

public interface ISunInfoRepository
{
    public Task<SunInfo?> GetSunInfoById(int id);
    public SunInfo GetSunInfo(int cityId, DateOnly date);
    public IEnumerable<SunInfo> GetAll();
    public void AddSunInfo(SunInfo sunInfo);
    public Task UpdateSunInfoAsync(SunInfo sunInfo);
    public void DeleteSunInfo(SunInfo sunInfo);
}