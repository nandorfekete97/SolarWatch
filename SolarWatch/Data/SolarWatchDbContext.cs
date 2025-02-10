using Microsoft.EntityFrameworkCore;
using SolarWatch.Models;

namespace SolarWatch;

public class SolarWatchDbContext : DbContext
{
    public DbSet<City> Cities { get; set; }
    public DbSet<SunInfo> SunInfos { get; set; }

    public SolarWatchDbContext(DbContextOptions options) : base(options)
    {
    }
}