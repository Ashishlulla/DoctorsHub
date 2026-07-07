using DoctorsHub.Domain.Entities;
using DoctorsHub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

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

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
        public string Reason { get; set; } = string.Empty;

       
    }
}
