using MyBlazorApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlazorApp.Services
{
    public static class EventDataService
    {
        private static readonly List<Event> _cachedEvents = CreateSampleEvents();

        public static ValueTask<List<Event>> GetEventsAsync()
        {
            // Return a shallow copy to prevent accidental modifications
            return new ValueTask<List<Event>>(Task.FromResult(_cachedEvents.ToList()));
        }

        public static ValueTask<Event?> GetEventByIdAsync(int id)
        {
            var ev = _cachedEvents.FirstOrDefault(e => e.Id == id);
            return new ValueTask<Event?>(ev);
        }

        public static bool TryRegisterAttendee(int eventId)
        {
            var ev = _cachedEvents.FirstOrDefault(e => e.Id == eventId);
            if (ev == null) return false;
            if (ev.Capacity <= 0) return false;
            if (ev.RegisteredAttendees >= ev.Capacity) return false;
            ev.RegisteredAttendees++;
            return true;
        }

        private static List<Event> CreateSampleEvents()
        {
            return new List<Event>
            {
                new Event
                {
                    Id = 1,
                    Name = "Annual Tech Conference 2026",
                    Description = "Join industry leaders for cutting-edge technology discussions and networking.",
                    Date = new DateTime(2026, 04, 15, 09, 00, 0),
                    Location = "San Francisco Convention Center",
                    Category = "Technology",
                    Capacity = 500,
                    RegisteredAttendees = 342
                },
                new Event
                {
                    Id = 2,
                    Name = "Summer Networking Gala",
                    Description = "Exclusive evening event for professionals to connect and build relationships.",
                    Date = new DateTime(2026, 05, 22, 18, 30, 0),
                    Location = "Downtown Ballroom",
                    Category = "Networking",
                    Capacity = 200,
                    RegisteredAttendees = 156
                },
                new Event
                {
                    Id = 3,
                    Name = "Product Launch Party",
                    Description = "Celebrate the launch of our latest innovative product.",
                    Date = new DateTime(2026, 03, 20, 14, 00, 0),
                    Location = "Innovation Hub",
                    Category = "Product",
                    Capacity = 150,
                    RegisteredAttendees = 150
                },
                new Event
                {
                    Id = 4,
                    Name = "Team Retreat & Workshop",
                    Description = "Strategic planning and team building for companies.",
                    Date = new DateTime(2026, 06, 10, 08, 30, 0),
                    Location = "Mountain Resort",
                    Category = "Workshop",
                    Capacity = 80,
                    RegisteredAttendees = 45
                },
                new Event
                {
                    Id = 5,
                    Name = "Business Development Summit",
                    Description = "Explore new business opportunities and partnerships.",
                    Date = new DateTime(2026, 07, 08, 10, 00, 0),
                    Location = "Executive Plaza",
                    Category = "Business",
                    Capacity = 300,
                    RegisteredAttendees = 198
                }
            };
        }
    }
}
