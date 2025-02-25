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

namespace SolarWatch
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Connect frontend
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            AddServices(builder);
            ConfigureSwagger(builder);
            AddDbContexts(builder);
            AddAuthentication(builder);
            AddIdentity(builder);

            var app = builder.Build();

            app.UseCors("AllowReactApp");

            // Seed roles and admin user
            using var scope = app.Services.CreateScope();
            var authenticationSeeder = scope.ServiceProvider.GetRequiredService<AuthenticationSeeder>();
            authenticationSeeder.AddRoles();
            authenticationSeeder.AddAdmin();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }

        // Local methods are now static
        static void AddServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddScoped<ICityRepository, CityRepository>();
            builder.Services.AddScoped<ISunInfoRepository, SunInfoRepository>();
            builder.Services.AddScoped<IJsonProcessor, JsonProcessor>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<AuthenticationSeeder>();

            builder.Services.AddHttpClient();
            builder.Services.AddHttpClient<ISolarService, SolarService>();
        }

        static void ConfigureSwagger(WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "SolarWatch API", Version = "v1" });
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

        static void AddDbContexts(WebApplicationBuilder builder)
        {
            if (!builder.Environment.IsEnvironment("Testing"))
            {
                builder.Services.AddDbContext<SolarWatchDbContext>(options =>
                {
                    options.UseSqlServer(
                        builder.Configuration["ConnectionStrings:SolarWatchDb"]);
                });
            }
        }

        static void AddAuthentication(WebApplicationBuilder builder)
        {
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var secretKey = builder.Configuration["JwtSettings:IssuerSigningKey"];

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(secretKey)
                        ),
                    };
                });
        }

        static void AddIdentity(WebApplicationBuilder builder)
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
    }
}
