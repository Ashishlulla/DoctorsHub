using DoctorsHub.Domain.Enums;

namespace DoctorsHub.Application.DTOs.Reports.AppointmentsReport
{
    public class AppointmentReportFilteredDto
    {
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public int DoctorId { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Completed;
    }
}
