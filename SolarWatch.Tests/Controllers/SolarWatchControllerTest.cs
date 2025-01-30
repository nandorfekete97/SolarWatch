using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using SolarWatch.Controllers;
using SolarWatch.Services;

namespace SolarWatch.Tests.Controllers;

[TestFixture]
[TestOf(typeof(SolarWatchController))]
public class SolarWatchControllerTest
{
    private Mock<ISolarService> _solarServiceMock;
    private SolarWatchController _controller;

    [SetUp]
    public void SetUp()
    {
        _solarServiceMock = new Mock<ISolarService>();

        // Setup mocked responses
        _solarServiceMock
            .Setup(s => s.GetSunriseAsync("Budapest", It.IsAny<DateOnly>()))
            .ReturnsAsync("2:47:04 AM UTC");

        _solarServiceMock
            .Setup(s => s.GetSunsetAsync("Budapest", It.IsAny<DateOnly>()))
            .ReturnsAsync("6:44:39 PM UTC");

        _controller = new SolarWatchController(_solarServiceMock.Object);
    }

    [Test]
    public async Task TestGetSunrise()
    {
        string result = await _controller.GetSunrise("Budapest", new DateOnly(2025, 6, 21));
        Assert.That(result, Is.EqualTo("2:47:04 AM UTC"));
    }

    [Test]
    public async Task TestGetSunset()
    {
        string result = await _controller.GetSunset("Budapest", new DateOnly(2025, 6, 21));
        Assert.That(result, Is.EqualTo("6:44:39 PM UTC"));
    }
}