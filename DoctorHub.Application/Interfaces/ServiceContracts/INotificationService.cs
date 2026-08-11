using DoctorsHub.Application.DTOs.Notification;


namespace DoctorsHub.Application.Interfaces.ServiceContracts
{
    public interface INotificationService
    {
        Task<NotificationDto> CreateAsync(CreateNotificationDto createNotificationDto);

        Task CreateAppointmentNotificationAsync(CreateNotificationDto createNotificationDto);

        Task<List<NotificationDto>> GetAllNoticationsAsync();

        Task<List<NotificationDto>> GetAllUnreadNotificationsAsync();

        Task<List<NotificationDto>> GetByUserIdAsync(string userId);

        Task<List<NotificationDto>> GetUnreadByUserIdAsync(string userId);

        Task<NotificationDto?> GetByIdAsync(int id);

        Task MarkAsReadAsync(int notificationId);

        Task MarkAllAsReadAsync(string userId);
    }
}
