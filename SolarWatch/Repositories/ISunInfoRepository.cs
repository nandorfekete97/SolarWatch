using SolarWatch.Models;

namespace SolarWatch.Repositories;

public interface ISunInfoRepository
{
    public SunInfo GetSunInfo(int cityId, DateOnly date);
    public IEnumerable<SunInfo> GetAll();
    public void AddSunInfo(SunInfo sunInfo);
    public void UpdateSunInfo(SunInfo sunInfo);
    public void DeleteSunInfo(SunInfo sunInfo);
}