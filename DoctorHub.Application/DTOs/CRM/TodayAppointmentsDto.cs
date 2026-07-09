using DoctorsHub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DoctorsHub.Application.DTOs.CRM
{
    public class TodayAppointmentsDto
    {
        public int id { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public DateOnly AppointmentDate { get; set; }
        public TimeSpan startTime { get; set; }
        public TimeSpan EndTime { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
        public string reason { get; set; } = string.Empty;
        public string notes { get; set; } = string.Empty;
    }
}
