using DoctorsHub.Domain.Identity;

namespace DoctorHub.Application.DTOs.Doctors
{
    public class DoctorDto
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public required ApplicationUser User { get; set; }
        
        public string PhoneNumber {  get; set; } = string.Empty;

        public string Qualification { get; set; } = string.Empty;

        public int ExperienceYears { get; set; }

        public decimal ConsultationFee { get; set; }

        public string SpecializationName { get; set; } = string.Empty;

        public int SpecializationId { get; set; }

        public double AverageRating { get; set; }

        public string VisitDays { get; set; } = string.Empty;

        public string About { get; set; } = string.Empty;

        //public DateTime CreatedAt { get; set; }

        //public DateTime UpdatedAt { get; set; }
    }
}
