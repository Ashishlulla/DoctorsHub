using DoctorsHub.Domain.Enums;

namespace DoctorsHub.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }

        public int DoctorId { get; set; }

        public int PatientId { get; set; }
        public DateOnly AppointmentDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public string Reason { get; set; } = string.Empty;

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public Doctor Doctor { get; set; } = null!;

        public Patient Patient { get; set; } = null!;
    }
}
