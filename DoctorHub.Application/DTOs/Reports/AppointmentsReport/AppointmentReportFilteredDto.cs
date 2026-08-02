using DoctorsHub.Domain.Enums;
using System.Text.Json.Serialization;

namespace DoctorsHub.Application.DTOs.Reports.AppointmentsReport
{
    public class AppointmentReportFilteredDto
    {
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public int? DoctorId { get; set; }
        public int? PatientId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AppointmentStatus? Status { get; set; } = AppointmentStatus.Completed;
    }
}
