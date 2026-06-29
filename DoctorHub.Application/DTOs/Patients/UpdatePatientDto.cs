using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.DTOs.Patients
{
    public class UpdatePatientDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string BloodGroup { get; set; } = string.Empty;

    }
}
