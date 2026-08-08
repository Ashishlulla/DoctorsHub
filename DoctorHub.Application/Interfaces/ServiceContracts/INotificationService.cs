using DoctorsHub.Domain.Entities;
using DoctorsHub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.Interfaces.ServiceContracts
{
    public interface INotificationService
    {
        Task<Notification> CreateAsync(
            string userId,
            string title,
            string message,
            NotificationType type);

        Task<List<Notification>> GetByUserIdAsync(string userId);

        Task<List<Notification>> GetUnreadByUserIdAsync(string userId);

        Task MarkAsReadAsync(int notificationId);

        Task MarkAllAsReadAsync(string userId);
    }
}
