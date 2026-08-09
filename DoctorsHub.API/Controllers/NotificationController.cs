using DoctorsHub.Application.DTOs.Notification;
using DoctorsHub.Application.Interfaces.ServiceContracts;

using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        //Private Feilds
        private readonly INotificationService _notificationService;

        //Constructor
        public NotificationController(INotificationService notificationService) 
        {
            _notificationService = notificationService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllNotificationssync() 
        {
            var notifications = await _notificationService.GetAllNoticationsAsync();
            
            return Ok(notifications);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id) 
        {
            var notification = await _notificationService.GetByIdAsync(id);

            if(notification == null)
                return NotFound();

            return Ok(notification);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDto createNotificationDto) 
        {
            var notification = await _notificationService.CreateAsync(createNotificationDto);

            return Ok(notification);
        }

        [HttpGet("User/{UserId}")]
        public async Task<IActionResult> GetByUserIdAsync(string UserId) 
        {
            var notifications = await _notificationService.GetByUserIdAsync(UserId);
            
            return Ok(notifications);
        }

        [HttpGet("UnRead/{UserId}")]
        public async Task<IActionResult> GetByUnReadUserIdAsync(string UserId)
        {
            var notifications = await _notificationService.GetUnreadByUserIdAsync(UserId);
            return Ok(notifications);
        }

        [HttpPut("MarkAsRead/{id:int}")]
        public async Task<IActionResult> MarkAsRead(int id) 
        {
            await _notificationService.MarkAsReadAsync(id);

            return Ok("Marked  notification as Read.");
        }

        [HttpPut("MarkAllAsRead/{UserId}")]
        public async Task<IActionResult> MarkAllAsRead(string UserId)
        {
            await _notificationService.MarkAllAsReadAsync(UserId);

            return Ok("Marked All notification as Read.");
        }

    }
}
