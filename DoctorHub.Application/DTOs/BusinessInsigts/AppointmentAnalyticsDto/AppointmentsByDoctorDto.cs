using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.DTOs.BusinessInsigts.AppointmentAnalyticsDto
{
    public  class AppointmentsByDoctorDto
    {
        public string DoctorName { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
