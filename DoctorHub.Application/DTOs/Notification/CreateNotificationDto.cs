using DoctorsHub.Domain.Enums;


namespace DoctorsHub.Application.DTOs.Notification
{
    public class CreateNotificationDto
    {
        public string UserId { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public NotificationType Type { get; set; }
    }
}
