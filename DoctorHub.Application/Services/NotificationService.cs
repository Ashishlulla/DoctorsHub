using AutoMapper;
using DoctorsHub.Application.DTOs.Notification;
using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using DoctorsHub.Domain.Entities;

namespace DoctorsHub.Application.Services
{
    public class NotificationService :INotificationService
    {

        //Private Feilds
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        //Constructor
        public NotificationService(INotificationRepository notificationRepository, IMapper mapper) 
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public async Task<NotificationDto> CreateAsync(CreateNotificationDto createNotificationdto)
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

            return _mapper.Map<NotificationDto>(notification);
        }

        public async Task<List<NotificationDto>> GetAllNoticationsAsync()
        {
            List<Notification> notifications  = await _notificationRepository.GetAllNotificationsAsync();

            return _mapper.Map<List<NotificationDto>>(notifications);
        }

        public async Task<NotificationDto?> GetByIdAsync(int id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);
            if (notification == null)
                throw new KeyNotFoundException($"No notification found with id = {id}");

            return  _mapper.Map<NotificationDto>(notification);
        }

        public async Task<List<NotificationDto>> GetByUserIdAsync(string userId)
        {
            var notifications = await _notificationRepository.GetByUserIdAsync(userId);

            return _mapper.Map<List<NotificationDto>>(notifications);
        }

        public async Task<List<NotificationDto>> GetUnreadByUserIdAsync(string userId)
        {
            var notifications= await _notificationRepository.GetUnreadByUserIdAsync(userId);

            return _mapper.Map<List<NotificationDto>>(notifications);
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
