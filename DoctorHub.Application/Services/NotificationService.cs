using DoctorsHub.Application.DTOs.Notification;
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

        public async Task<Notification> CreateAsync(CreateNotificationDto createNotificationdto)
        {
            var notification = new Notification 
            {
                UserId = createNotificationdto.UserId,
                Title = createNotificationdto.Title,
                Message = createNotificationdto.Message,
                Type = createNotificationdto.Type,
                CreatedAt = DateTime.UtcNow,
            };

            await _notificationRepository.AddAsync(notification);
            await _notificationRepository.SaveChangesAsync();

            return notification;
        }

        public async Task<List<Notification>> GetAllNoticationsAsync()
        {
            await _notificationRepository.GetAllNotificationsAsync();
        }

        public async Task<Notification?> GetByIdAsync(int id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);
            if (notification == null)
                throw new KeyNotFoundException($"No notification found with id = {id}");

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

            if (!notifications.Any())
                return;

            foreach (var notification in notifications) 
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;

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
