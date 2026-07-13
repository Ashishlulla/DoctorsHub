using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.DTOs.BusinessInsigts.RevenueAnalyticsDto
{
    public class TopRevenueGeneratingDoctorDto
    {
        public string DoctorName { get; set; } = string.Empty;
        public decimal RevenueGenerated { get; set; }
    }
}
