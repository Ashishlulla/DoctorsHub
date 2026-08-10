using DoctorsHub.Domain.Entities;

namespace DoctorsHub.Application.Interfaces.RepositoryContracts
{
    public interface INotificationRepository
    {
        Task<Notification> AddAsync(Notification notification);

        Task<List<Notification>> GetAllNotificationsAsync();
        Task<List<Notification>> GetAllUnReadNotificationsAsync();

        Task<List<Notification>> GetByUserIdAsync(string userId);

        Task<List<Notification>> GetUnreadByUserIdAsync(string userId);

        Task<Notification?> GetByIdAsync(int id);

        Task UpdateAsync(Notification notification);

        Task SaveChangesAsync();
    }
}
