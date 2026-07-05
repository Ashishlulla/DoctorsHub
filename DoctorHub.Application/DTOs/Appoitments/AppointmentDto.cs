using DoctorsHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.DTOs.Appoitments
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        
        public string DoctorName { get; set; } = string.Empty;
        
        public string PatientName { get; set; } = string.Empty;
        
        public DateOnly AppointmentDate { get; set; }
        
        public TimeSpan StartTime { get; set; }
        
        public TimeSpan EndTime { get; set; }
       
        public string Status { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;

       
    }
}
