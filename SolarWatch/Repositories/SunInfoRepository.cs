using System.Collections;
using SolarWatch.Models;

namespace SolarWatch.Repositories;

public class SunInfoRepository : ISunInfoRepository
{
    private SolarWatchDbContext _solarWatchDbContext;

    public SunInfoRepository(SolarWatchDbContext solarWatchDbContext)
    {
        _solarWatchDbContext = solarWatchDbContext;
    }

    public SunInfo? GetSunInfo(int cityId, DateOnly date)
    {
        return _solarWatchDbContext.SunInfos.FirstOrDefault(sunInfo =>
            sunInfo.CityId == cityId && sunInfo.Date == date);
    }

    public IEnumerable<SunInfo> GetAll()
    {
        return _solarWatchDbContext.SunInfos;
    }

    public void AddSunInfo(SunInfo sunInfo)
    {
        _solarWatchDbContext.SunInfos.Add(sunInfo);
        _solarWatchDbContext.SaveChanges();
    }
    
    public void UpdateSunInfo(SunInfo sunInfo)
    {
        _solarWatchDbContext.Update(sunInfo);
        _solarWatchDbContext.SaveChanges();
    }
    
    public void DeleteSunInfo(SunInfo sunInfo)
    {
        _solarWatchDbContext.Remove(sunInfo);
        _solarWatchDbContext.SaveChanges();
    }
}