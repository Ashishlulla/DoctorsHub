namespace DoctorsHub.Application.DTOs.Departments
{
    public class DepartmentDetailsDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public List<DepartmentDoctorDto> Doctors { get; set; } = new();
    }

    public class DepartmentDoctorDto
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Qualification { get; set; } = string.Empty;

        public int ExperienceYears { get; set; }

        public decimal ConsultationFee { get; set; }

        public string SpecializationName { get; set; } = string.Empty;
    }
}