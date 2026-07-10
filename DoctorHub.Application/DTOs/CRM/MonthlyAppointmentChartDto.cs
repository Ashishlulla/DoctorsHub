using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.DTOs.CRM
{
    public class MonthlyAppointmentChartDto
    {
        public string Month { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
