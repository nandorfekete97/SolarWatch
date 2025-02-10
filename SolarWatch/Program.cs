using Microsoft.EntityFrameworkCore;
using SolarWatch;
using SolarWatch.Repositories;
using SolarWatch.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<ISolarService, SolarService>();

builder.Services.AddDbContext<SolarWatchDbContext>(options =>
{
    options.UseSqlServer(
        "Server=localhost,1433;Database=SolarWatch;User Id=sa;Password=WeWhoWrestleWithGod33$;Encrypt=false;");
});

builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ISunInfoRepository, SunInfoRepository>();
builder.Services.AddScoped<IJsonProcessor, JsonProcessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();