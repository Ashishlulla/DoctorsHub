namespace DoctorsHub.Application.DTOs.Reports.PatientsReport
{
    public class PatientsReportFilteredDto
    {
        public string? PatientName { get; set; } = string.Empty;
        public string? Gender { get; set; }
        public string? BloodGroup { get; set; }
    }
}
