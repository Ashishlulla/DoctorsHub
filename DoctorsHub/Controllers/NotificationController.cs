using DoctorsHub.Domain.Entities;
using DoctorsHub.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.Web.Controllers
{
    [Route("[controller]")]
    public class NotificationController : Controller
    {
        //Private Feilds
        public readonly NotificationApiService _notificationApiService;

        //Constructor
        public NotificationController(NotificationApiService notificationApiService) 
        {
            _notificationApiService = notificationApiService;
        }


        [HttpGet]
        
        public async Task<IActionResult> Index()
        {
            
            if (User.IsInRole("Admin") || User.IsInRole("Receptionist"))
            {

                var notifications = await _notificationApiService.GetAllUnreadNotifications();
                ViewBag.CountOfUnReadNotifications = notifications.Count();
                return View(notifications);
            }

            if (User.IsInRole("Doctor")) 
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                var notifications = await _notificationApiService.GetByUserIdAsync(userId);
                ViewBag.CountOfUnReadNotifications = notifications.Count();


                return View(notifications);
            }
            return Forbid();
            
        }

        [HttpGet("Unread")]
        public async Task<IActionResult> Unread()
        {
            if (User.IsInRole("Admin") || User.IsInRole("Receptionist"))
            {
                var notifications = await _notificationApiService.GetAllUnreadNotifications();
                return Json(notifications);
            }

            if (User.IsInRole("Doctor"))
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                var notifications = await _notificationApiService.GetUnreadByUserIdAsync(userId);

                return Json(notifications);
            }

            return Forbid();
        }

        [HttpPost]
        
        public async Task<IActionResult> MarkAsRead(int id) 
        {
            await _notificationApiService.MarkAsReadAsync(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]

        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId =  User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            await _notificationApiService.MarkAllAsReadAsync(userId);

            return RedirectToAction(nameof(Index));
        }
    }
}
