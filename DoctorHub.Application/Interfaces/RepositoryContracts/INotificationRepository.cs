using DoctorsHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Application.Interfaces.RepositoryContracts
{
    public interface INotificationRepository
    {
        Task<Notification> AddAsync(Notification notification);

        Task<List<Notification>> GetAllNotificationsAsync();

        Task<List<Notification>> GetByUserIdAsync(string userId);

        Task<List<Notification>> GetUnreadByUserIdAsync(string userId);

        Task<Notification?> GetByIdAsync(int id);

        Task UpdateAsync(Notification notification);

        Task SaveChangesAsync();
    }
}
