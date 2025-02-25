using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SolarWatch.IntegrationTests.Factories;

public class SolarWatchWebApplicationFactory : WebApplicationFactory<Program>
{
    //Create a new db name for each SolarWatchWebApplicationFactory. This is to prevent tests failing from changes done in db by a previous test. 
    private readonly string _dbName = Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            //Get the previous DbContextOptions registrations 
            var solarWatchDbContextDescriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<SolarWatchDbContext>));
            
            var solarWatchDbContextDescriptor2 = services.Single(d => d.ServiceType == typeof(SolarWatchDbContext));
            
            //Remove the previous DbContextOptions registrations
            services.Remove(solarWatchDbContextDescriptor);
            
            services.Remove(solarWatchDbContextDescriptor2);
            
            //Add new DbContextOptions for our two contexts, this time with inmemory db 
            services.AddDbContext<SolarWatchDbContext>(options =>
            {
                options.UseInMemoryDatabase(_dbName);
            });
            
            //We will need to initialize our in memory databases. 
            //Since DbContexts are scoped services, we create a scope
            using var scope = services.BuildServiceProvider().CreateScope();
            
            //We use this scope to request the registered dbcontexts, and initialize the schemas
            var solarContext = scope.ServiceProvider.GetRequiredService<SolarWatchDbContext>();
            solarContext.Database.EnsureDeleted();
            solarContext.Database.EnsureCreated();

            //Here we could do more initializing if we wished (e.g. adding admin user)
        });
    }
}