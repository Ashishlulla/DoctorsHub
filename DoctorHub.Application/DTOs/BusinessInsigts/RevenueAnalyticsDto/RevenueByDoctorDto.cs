using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.DTOs.BusinessInsigts.RevenueAnalyticsDto
{
    public class RevenueByDoctorDto
    {
        public string DoctorName { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
    }
}
