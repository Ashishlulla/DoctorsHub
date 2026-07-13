using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.DTOs.BusinessInsigts.RevenueAnalyticsDto
{
    public class AverageRevenueGeneratedByDoctorDto
    {
        public string DoctorName { get; set; } = string.Empty;
        public decimal AverageRevenue { get; set; }
    }
}
