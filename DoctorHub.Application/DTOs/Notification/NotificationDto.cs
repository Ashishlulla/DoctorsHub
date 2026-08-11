using DoctorsHub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DoctorsHub.Application.DTOs.Notification
{
    public class NotificationDto
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public int? AppointmentId { get; set; }

        public int? BillId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public NotificationType Type { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ReadAt { get; set; }
    }
}
