using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SolarWatch;
using SolarWatch.Repositories;
using SolarWatch.Services;
using SolarWatch.Services.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Call separate methods for better organization
AddServices();
ConfigureSwagger();
AddDbContexts();
AddAuthentication();
AddIdentity();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var authenticationSeeder = scope.ServiceProvider.GetRequiredService<AuthenticationSeeder>();
authenticationSeeder.AddRoles();
authenticationSeeder.AddAdmin();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseAuthentication();
app.MapControllers();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// ==== Local Methods for Organization ====

void AddServices()
{
    builder.Services.AddScoped<ICityRepository, CityRepository>();
    builder.Services.AddScoped<ISunInfoRepository, SunInfoRepository>();
    builder.Services.AddScoped<IJsonProcessor, JsonProcessor>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddScoped<AuthenticationSeeder>();

    builder.Services.AddHttpClient();
    builder.Services.AddHttpClient<ISolarService, SolarService>();
}

void ConfigureSwagger()
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(option =>
    {
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "API Title", Version = "v1" });
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[]{}
            }
        });
    });
}

void AddDbContexts()
{
    builder.Services.AddDbContext<SolarWatchDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration["ConnectionStrings:SolarWatchDb"]);
    });
}

void AddAuthentication()
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    var secretKey = builder.Configuration["JwtSettings:IssuerSigningKey"];

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["ValidIssuer"],
                ValidAudience = jwtSettings["ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            };
        });
}

void AddIdentity()
{
    builder.Services.AddIdentityCore<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.User.RequireUniqueEmail = true;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<SolarWatchDbContext>();
}

public partial class Program { }