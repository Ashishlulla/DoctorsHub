using DoctorsHub.Domain.Identity;

namespace DoctorsHub.Domain.Entities
{
    public class Doctor
    {
        public int Id { get; set; }

        public required string UserId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string PersonalEmail { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public string Qualification { get; set; } = string.Empty;

        public string About { get; set; } = string.Empty;

        public int ExperienceYears { get; set; }

        public decimal ConsultationFee { get; set; }

        public string VisitDays { get; set; } = string.Empty;

        public int SpecializationId { get; set; }


        // Navigation Properties
        public ApplicationUser? User { get; set; }

        public Specialization Specialization { get; set; } = null!;

        public List<Appointment> Appointments { get; set; } = new();

        public ICollection<Department> Departments { get; set; } = new List<Department>();
        
        public List<ScheduleSlot> ScheduleSlots { get; set; } = new();
    }
}