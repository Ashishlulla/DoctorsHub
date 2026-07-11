using DoctorsHub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DoctorsHub.Application.DTOs.BusinessInsigts
{
    public class AppointmentStatusChartDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
        public int Count { get; set; }
    }
}
