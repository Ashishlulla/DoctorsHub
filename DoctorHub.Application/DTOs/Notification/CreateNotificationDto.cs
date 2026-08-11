using DoctorsHub.Domain.Enums;
using System.Text.Json.Serialization;


namespace DoctorsHub.Application.DTOs.Notification
{
    public class CreateNotificationDto
    {
        public string UserId { get; set; } = string.Empty;

        public int? AppointmentId { get; set; }

        public int? BillId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public NotificationType Type { get; set; }
    }
}
