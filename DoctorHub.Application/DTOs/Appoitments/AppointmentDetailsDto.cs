using DoctorsHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.DTOs.Appoitments
{
     public class AppointmentDetailsDto
    {
        public int Id { get; set; }

        public DateTime AppointmentDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public string Reason { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public Doctor Doctor { get; set; } = null!;

        public Patient Patient { get; set; } = null!;
    }
}
