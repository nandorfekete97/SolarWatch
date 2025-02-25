using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using SolarWatch.Contracts;
using SolarWatch.IntegrationTests.Factories;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace SolarWatch.IntegrationTests.Tests;

[Collection("IntegrationTests")]

public class MyControllerIntegrationTest : IClassFixture<SolarWatchWebApplicationFactory>
{
    private readonly HttpClient _client;
    private string _authToken;

    public MyControllerIntegrationTest(SolarWatchWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetSunset_ReturnsOk_WhenUserIsAuthenticated()
    {
        await RegisterAndLoginTestUser();
        var response = await GetAuthenticatedResponseAsync("/SolarWatch/GetSunset", "Berlin", "2025-02-23");
        Assert.True(response.IsSuccessStatusCode, "GetSunset request failed");
        var sunsetTime = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(sunsetTime), "Sunset time should not be empty");
    }

    [Fact]
    public async Task GetSunrise_ReturnsForbidden_WhenUserIsNotAdmin()
    {
        await RegisterAndLoginTestUser();
        var response = await GetAuthenticatedResponseAsync("/SolarWatch/GetSunrise", "Berlin", "2025-02-23");
        Assert.Equal(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
    }

    private async Task RegisterAndLoginTestUser()
    {
        var registerResponse = await _client.PostAsJsonAsync("/Auth/Register",
            new RegistrationRequest("testuser@example.com", "testuser", "Test123!"));
        registerResponse.EnsureSuccessStatusCode();
        var loginResponse = await _client.PostAsJsonAsync("/Auth/Login",
            new AuthRequest("testuser@example.com", "Test123!"));
        loginResponse.EnsureSuccessStatusCode();
        var authResponse = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();
        _authToken = authResponse?.Token ?? string.Empty;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authToken);
    }

    private async Task<HttpResponseMessage> GetAuthenticatedResponseAsync(string endpoint, string city, string date)
    {
        return await _client.GetAsync($"{endpoint}?city={city}&date={date}");
    }
}