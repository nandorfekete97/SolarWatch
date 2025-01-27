using System;
using NUnit.Framework;
using SolarWatch.Controllers;

namespace SolarWatch.Tests.Controllers;

[TestFixture]
[TestOf(typeof(SolarWatchController))]
public class SolarWatchControllerTest
{

    [Test]
    public void TestGetSunrise()
    {
        SolarWatchController solarWatchController = new SolarWatchController();
        
        string result = solarWatchController.GetSunrise("Budapest", new DateOnly(2025, 6, 21));
        
        string expected = "2:47:04 AM UTC";
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void TestGetSunset()
    {
        SolarWatchController solarWatchController = new SolarWatchController();

        string result = solarWatchController.GetSunset("Budapest", new DateOnly(2025, 6, 21));

        string expected = "6:44:39 PM UTC";
        Assert.That(result, Is.EqualTo(expected));
    }
}