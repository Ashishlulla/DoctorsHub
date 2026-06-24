using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public string PatientId { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } = "Pending";

        //Navigation Property
        public Doctor Doctor { get; set; } = null!;

    }
}
