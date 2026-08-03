namespace DoctorsHub.Application.DTOs.Reports.DoctorsReport
{
    public class DoctorsReportDto
    {
        public string DoctorName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;
        
        public string Qualification { get; set; } = string.Empty;
        
        public string Specialization { get; set; } = string.Empty;

        public int Experience { get; set; }

        public decimal ConsultationFee { get; set; }

    }

}
