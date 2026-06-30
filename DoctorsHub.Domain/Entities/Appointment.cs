using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }

        public int DoctorId { get; set; }

        public int PatientId { get; set; }

        public DateTime AppointmentDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public string Reason { get; set; } = string.Empty;

        public string Status { get; set; } = "Scheduled";

        public string? Notes { get; set; }

        // Navigation Properties
        public Doctor Doctor { get; set; } = null!;

        public Patient Patient { get; set; } = null!;
    }
}
