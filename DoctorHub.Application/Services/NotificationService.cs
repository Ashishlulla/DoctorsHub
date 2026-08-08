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

        public Task<Notification> CreateAsync(string userId, string title, string message, NotificationType type)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notification>> GetByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notification>> GetUnreadByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task MarkAllAsReadAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task MarkAsReadAsync(int notificationId)
        {
            throw new NotImplementedException();
        }
    }
}
