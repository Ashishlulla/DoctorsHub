using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using DoctorsHub.Domain.Entities;
using DoctorsHub.Domain.Enums;

namespace DoctorsHub.Application.Services
{
    public class NotificationService :INotificationService
    {

        //Private Feilds
        private readonly INotificationRepository _notificationRepository;

        //Constructor
        public NotificationService(INotificationRepository notificationRepository) 
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<Notification> CreateAsync(string userId, string title, string message, NotificationType type)
        {
            var notification = new Notification 
            {
                UserId = userId,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.UtcNow,
            };

            await _notificationRepository.AddAsync(notification);
            await _notificationRepository.SaveChangesAsync();

            return notification;
        }

        public async Task<List<Notification>> GetByUserIdAsync(string userId)
        {
            return await _notificationRepository.GetByUserIdAsync(userId);
        }

        public async Task<List<Notification>> GetUnreadByUserIdAsync(string userId)
        {
            return await _notificationRepository.GetUnreadByUserIdAsync(userId);
        }

        public async Task MarkAllAsReadAsync(string userId)
        {
            var notifications = await _notificationRepository.GetUnreadByUserIdAsync(userId);

            if (notifications == null)
                return;

            foreach (var notification in notifications) 
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;

                await _notificationRepository.UpdateAsync(notification);
            }

            await _notificationRepository.SaveChangesAsync();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);

            if (notification == null)
                return;

            if (notification.IsRead)
                return;

            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;

            await _notificationRepository.UpdateAsync(notification);
            await _notificationRepository.SaveChangesAsync();
        }
    }
}
