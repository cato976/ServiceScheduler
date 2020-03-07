using NUnit.Framework;
using Should;
using Microsoft.Extensions.Configuration;
using System;

using Calendar.API;

namespace ServiceScheduler.Tests
{
    [TestFixture]
    public class CalendarTests
    {
        private static string CalendarId;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var config = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
            InitConfigValues(config);
        }

        [Test]
        public void GetNumberOfCalendars()
        {
            int numberOfCalendars = CalendarApi.GetNumberOfCalendars().Result;
            numberOfCalendars.ShouldBeGreaterThanOrEqualTo(1);
        }

        [Test]
        public void Get10Events()
        {
            int numberOfEvents = CalendarApi.GetEvents(CalendarId).Result;
            numberOfEvents.ShouldBeGreaterThanOrEqualTo(1);
        }

        private static void InitConfigValues(IConfiguration config)
        {
            CalendarId = config.GetSection("CalendarId").GetSection("CalendarId").Value;
        }
    }
}
