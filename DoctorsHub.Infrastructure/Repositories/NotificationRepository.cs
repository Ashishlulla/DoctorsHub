using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Domain.Entities;
using DoctorsHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        //Private Feilds
        private readonly ApplicationDbContext _db;

        //Constructor
        public NotificationRepository(ApplicationDbContext db) 
        {
            _db = db;
        }

        public async Task<Notification> AddAsync(Notification notification)
        {
            await _db.Notifications.AddAsync(notification);
            

            return notification;

        }

        public async Task<Notification?> GetByIdAsync(int id)
        {
            return await _db.Notifications.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<List<Notification>> GetByUserIdAsync(string userId)
        {
            return await _db.Notifications.AsNoTracking().Where(n => n.UserId == userId).OrderByDescending(c => c.CreatedAt).ToListAsync();
        }

        public async Task<List<Notification>> GetUnreadByUserIdAsync(string userId)
        {
            return await _db.Notifications.AsNoTracking().Where(n => n.UserId == userId  && !n.IsRead).OrderByDescending(c => c.CreatedAt).ToListAsync();
        }

        

        public  Task UpdateAsync(Notification notification)
        {
            _db.Notifications.Update(notification);

            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
