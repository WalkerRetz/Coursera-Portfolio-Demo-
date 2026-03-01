using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MyBlazorApp.Models
{
    public class Event : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;

        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(200)]
        public string Location { get; set; } = string.Empty;

        [StringLength(100)]
        public string Category { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int Capacity { get; set; }

        [Range(0, int.MaxValue)]
        public int RegisteredAttendees { get; set; }

        // Cross-field validation (e.g., registered should not exceed capacity)
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Capacity < 0)
            {
                yield return new ValidationResult("Capacity must be non-negative.", new[] { nameof(Capacity) });
            }

            if (RegisteredAttendees < 0)
            {
                yield return new ValidationResult("Registered attendees must be non-negative.", new[] { nameof(RegisteredAttendees) });
            }

            if (Capacity > 0 && RegisteredAttendees > Capacity)
            {
                yield return new ValidationResult("Registered attendees cannot exceed capacity.", new[] { nameof(RegisteredAttendees), nameof(Capacity) });
            }
        }
    }
}
