using System;
using Microsoft.Extensions.Configuration;

using Calendar.API;
using Google.Apis.Calendar.v3.Data;

namespace ServiceScheduler
{
    public class Program
    {
        private static string CalendarId;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var config = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            InitConfigValues(config);
            //int result = GetNumberOfCalendars().Result;
            //int result = Calendar.API.CalendarApi.GetEvents(CalendarId).Result;
            //Event result = Calendar.API.CalendarApi.CreateEvent(CalendarId).Result;
        }

        private static void InitConfigValues(IConfiguration config)
        {
            CalendarId = config.GetSection("CalendarId").GetSection("CalendarId").Value;
        }
    }
}
