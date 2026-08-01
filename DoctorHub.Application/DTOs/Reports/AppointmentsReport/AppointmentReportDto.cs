using DoctorsHub.Domain.Enums;

namespace DoctorsHub.Application.DTOs.Reports.AppointmentsReport
{
    public class AppointmentReportDto
    {
        public int Id { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public DateOnly AppointmentDate { get; set; }
        public string AppointmentTime { get; set; } = string.Empty;
        
        
        public AppointmentStatus Status { get; set; }
    }
}
