using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorHub.Application.DTOs.Doctors
{
    public class UpdateDoctorDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Qualification { get; set; } = string.Empty;

        public string About { get; set; } = string.Empty;

        public int ExperienceYears { get; set; }

        public decimal ConsultationFee { get; set; }

        public string VisitDays { get; set; } = string.Empty;

        public int SpecializationId { get; set; }   
    }
}
