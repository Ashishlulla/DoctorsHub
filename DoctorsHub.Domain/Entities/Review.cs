using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Domain.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public string PatientId { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;

        //Navigation Property
        public Doctor Doctor { get; set; } = null!;
    }
}
