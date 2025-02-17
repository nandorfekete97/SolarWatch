using System.Collections;
using Microsoft.EntityFrameworkCore;
using SolarWatch.Models;

namespace SolarWatch.Repositories;

public class SunInfoRepository : ISunInfoRepository
{
    private SolarWatchDbContext _solarWatchDbContext;

    public SunInfoRepository(SolarWatchDbContext solarWatchDbContext)
    {
        _solarWatchDbContext = solarWatchDbContext;
    }

    public async Task<SunInfo?> GetSunInfoById(int sunInfoId)
    {
        return await _solarWatchDbContext.SunInfos.FirstOrDefaultAsync(sunInfo => sunInfo.Id == sunInfoId);
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
    
    public async Task UpdateSunInfoAsync(SunInfo sunInfo)
    {
        _solarWatchDbContext.Update(sunInfo);
        await _solarWatchDbContext.SaveChangesAsync();
    }
    
    public void DeleteSunInfo(SunInfo sunInfo)
    {
        _solarWatchDbContext.Remove(sunInfo);
        _solarWatchDbContext.SaveChanges();
    }
}