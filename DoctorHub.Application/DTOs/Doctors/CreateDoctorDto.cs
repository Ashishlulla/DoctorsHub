
namespace DoctorsHub.Application.DTOs.Doctors
{
    public class CreateDoctorDto
    {
        public string FullName { get; set; } = string.Empty;
        public string PersonalEmail { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }
        public string Qualification { get; set; } = string.Empty;
        public string About { get; set; } = string.Empty;
        public int ExperienceYears { get; set; }
        public decimal ConsultationFee { get; set; }
        public string VisitDays { get; set; } = string.Empty;
        
        public int SpecializationId { get; set; }
    }
}
