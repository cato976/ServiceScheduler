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
            var numberOfEvents = CalendarApi.GetEvents(CalendarId).Result;
            numberOfEvents.Count.ShouldBeGreaterThanOrEqualTo(1);
        }

        [Test]
        public void CreateEvent()
        {
            var startTime = DateTime.Now.AddHours(2);
            var duration = new TimeSpan(0, 3, 0, 0);
            var @event = CalendarApi.CreateEvent(CalendarId, "test summary", "test description", startTime, duration).Result;
            @event.ShouldNotBeNull();
            var events = CalendarApi.GetEvents(CalendarId);
            var theEvents = events.Result;
            var theEvent = theEvents.Find(id => id.Id == @event.Id);
            theEvent.Id.ShouldEqual(@event.Id);
            theEvent.Description.ShouldEqual(@event.Description);
            theEvent.Summary.ShouldEqual(@event.Summary);
            CalendarApi.DeleteEvent(CalendarId, @event.Id);
        }

        private static void InitConfigValues(IConfiguration config)
        {
            CalendarId = config.GetSection("CalendarId").GetSection("CalendarId").Value;
        }
    }
}
