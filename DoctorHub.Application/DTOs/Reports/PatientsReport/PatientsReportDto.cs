namespace DoctorsHub.Application.DTOs.Reports.PatientsReport
{
    public class PatientsReportDto
    {
        public int Id { get; set; }
        public string PatientName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Year;
                if (DateOfBirth.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        public string Gender { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string BloodGroup { get; set; } = string.Empty;

    }
}
