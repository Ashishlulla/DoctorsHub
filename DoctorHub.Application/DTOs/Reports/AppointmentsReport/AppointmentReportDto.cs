using DoctorsHub.Domain.Enums;
using System.Text.Json.Serialization;

namespace DoctorsHub.Application.DTOs.Reports.AppointmentsReport
{
    public class AppointmentReportDto
    {
        public int Id { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public DateOnly AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

 
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AppointmentStatus Status { get; set; }
    }
}
