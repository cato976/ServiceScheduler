using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Discovery.v1;
using Google.Apis.Discovery.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace Calendar.API
{
    public class CalendarApi
    {
        // If modifying these scopes, delete your previously saved credentials
        static string[] Scopes = { CalendarService.Scope.CalendarReadonly };

        // at ~/.credentials/calendar-dotnet-quickstart.json
        static string ApplicationName = "Google Calendar API .NET Quickstart";

        public static async Task<int> GetNumberOfCalendars()
        {
            int numberOfCalendars = 0;
            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            //var service = new DiscoveryService(new BaseClientService.Initializer()
            //{
            //ApplicationName = ApplicationName,
            //HttpClientInitializer = credential
            //});

            // Define parameters of request.
            //EventsResource.ListRequest request = service.Events.List("primary");
            CalendarListResource.ListRequest request = service.CalendarList.List();
            //var request = service.Apis.List().Execute();
            //request.TimeMin = DateTime.Now;
            //request.ShowDeleted = false;
            //request.SingleEvents = true;
            //request.MaxResults = 10;
            //request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // List events.
            //Events events = request.Execute();
            //Console.WriteLine("Upcoming events:");
            var calendars = await request.ExecuteAsync();
            Console.WriteLine("Calendar Names");
            if (calendars.Items != null)
            {
                numberOfCalendars = calendars.Items.Count;
                foreach (var item in calendars.Items)
                {
                    Console.WriteLine($"{item.Id} - {item.Summary}");
                }
            }
            //if (events.Items != null && events.Items.Count > 0)
            //{
            //foreach (var eventItem in events.Items)
            //{
            //string when = eventItem.Start.DateTime.ToString();
            //if (String.IsNullOrEmpty(when))
            //{
            //when = eventItem.Start.Date;
            //}
            //Console.WriteLine("{0} ({1})", eventItem.Summary, when);
            //}
            //}
            //else
            //{
            //Console.WriteLine("No upcoming events found.");
            //}
            //Console.Read();
            return numberOfCalendars;
        }

        public static async Task<int> GetEvents(string calendarId)
        {
            int numberOfEvents = 0;
            UserCredential credential;

            if(string.IsNullOrEmpty(calendarId))
            {
                throw new ArgumentException("calendarId is null or empty");
            }

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            EventsResource.ListRequest request = service.Events.List(calendarId);
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
            var events = await request.ExecuteAsync();
            Console.WriteLine("Calendar Events");
            if (events.Items != null)
            {
                numberOfEvents = events.Items.Count;
                foreach (var item in events.Items)
                {
                    Console.WriteLine(item.Id);
                }
            }

            return numberOfEvents;
        }
    }
}
