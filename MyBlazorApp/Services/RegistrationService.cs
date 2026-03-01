using MyBlazorApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlazorApp.Services
{
    public class RegistrationService
    {
        private readonly List<Registration> _registrations = new();
        private int _nextId = 1;

        public Task<(bool Success, string? Error)> RegisterAsync(Registration reg)
        {
            // Validate input
            var context = new System.ComponentModel.DataAnnotations.ValidationContext(reg);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            if (!System.ComponentModel.DataAnnotations.Validator.TryValidateObject(reg, context, results, true))
            {
                var msg = string.Join("; ", results.Select(r => r.ErrorMessage));
                return Task.FromResult<(bool, string?)>((false, msg));
            }

            // Attempt to update event capacity
            var success = EventDataService.TryRegisterAttendee(reg.EventId);
            if (!success) return Task.FromResult<(bool, string?)>((false, "Event is full or does not exist."));

            reg.Id = _nextId++;
            reg.RegisteredAt = System.DateTime.UtcNow;
            _registrations.Add(reg);
            return Task.FromResult<(bool, string?)>((true, null));
        }

        public Task<List<Registration>> GetRegistrationsAsync()
        {
            return Task.FromResult(_registrations.ToList());
        }

        public Task<List<Registration>> GetRegistrationsForEventAsync(int eventId)
        {
            var list = _registrations.Where(r => r.EventId == eventId).ToList();
            return Task.FromResult(list);
        }
    }
}
