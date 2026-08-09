using DoctorsHub.Application.DTOs.Notification;
using DoctorsHub.Domain.Entities;
using DoctorsHub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.Interfaces.ServiceContracts
{
    public interface INotificationService
    {
        Task<Notification> CreateAsync(CreateNotificationDto createNotificationDto);

        Task<List<Notification>> GetByUserIdAsync(string userId);

        Task<List<Notification>> GetUnreadByUserIdAsync(string userId);

        Task<Notification?> GetByIdAsync(int id);

        Task MarkAsReadAsync(int notificationId);

        Task MarkAllAsReadAsync(string userId);
    }
}
