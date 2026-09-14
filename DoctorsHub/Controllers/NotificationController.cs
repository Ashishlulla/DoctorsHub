using DoctorsHub.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DoctorsHub.Web.Controllers
{
    [Route("[controller]")]
    public class NotificationController : Controller
    {
        public readonly NotificationApiService _notificationApiService;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(
            NotificationApiService notificationApiService,
            ILogger<NotificationController> logger)
        {
            _notificationApiService = notificationApiService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation(
                "Notification index requested by user with roles: {Roles}",
                string.Join(", ", User.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value)));

            try
            {
                if (User.IsInRole("Admin") ||
                    User.IsInRole("Receptionist"))
                {
                    var notifications =
                        await _notificationApiService
                            .GetAllUnreadNotifications();

                    _logger.LogInformation(
                        "Unread notifications loaded successfully for Admin/Receptionist.");

                    return View(notifications);
                }

                if (User.IsInRole("Doctor"))
                {
                    var userId =
                        User.FindFirst(
                            ClaimTypes.NameIdentifier)?.Value;

                    if (string.IsNullOrEmpty(userId))
                    {
                        _logger.LogWarning(
                            "Notification index access denied because Doctor user ID is missing.");

                        return Unauthorized();
                    }

                    var notifications =
                        await _notificationApiService
                            .GetByUserIdAsync(userId);

                    _logger.LogInformation(
                        "Notifications loaded successfully for Doctor UserId: {UserId}",
                        userId);

                    return View(notifications);
                }

                _logger.LogWarning(
                    "Notification index access forbidden for user.");

                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading notifications.");

                throw;
            }
        }

        [HttpGet("Unread")]
        public async Task<IActionResult> Unread()
        {
            _logger.LogInformation(
                "Unread notification request received.");

            try
            {
                if (User.IsInRole("Admin") ||
                    User.IsInRole("Receptionist"))
                {
                    var notifications =
                        await _notificationApiService
                            .GetAllUnreadNotifications();

                    _logger.LogInformation(
                        "Unread notifications loaded successfully for Admin/Receptionist.");

                    return Json(notifications);
                }

                if (User.IsInRole("Doctor"))
                {
                    var userId =
                        User.FindFirst(
                            ClaimTypes.NameIdentifier)?.Value;

                    if (string.IsNullOrEmpty(userId))
                    {
                        _logger.LogWarning(
                            "Unread notification request failed because Doctor user ID is missing.");

                        return Unauthorized();
                    }

                    var notifications =
                        await _notificationApiService
                            .GetUnreadByUserIdAsync(userId);

                    _logger.LogInformation(
                        "Unread notifications loaded successfully for Doctor UserId: {UserId}",
                        userId);

                    return Json(notifications);
                }

                _logger.LogWarning(
                    "Unread notification request forbidden for user.");

                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading unread notifications.");

                throw;
            }
        }

        [HttpPost("MarkAsRead")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            _logger.LogInformation(
                "Mark notification as read requested for NotificationId: {NotificationId}",
                id);

            try
            {
                await _notificationApiService.MarkAsReadAsync(id);

                _logger.LogInformation(
                    "Notification marked as read successfully. NotificationId: {NotificationId}",
                    id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while marking notification as read. NotificationId: {NotificationId}",
                    id);

                throw;
            }
        }

        [HttpPost("MarkAllAsRead")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId =
                User.FindFirst(
                    ClaimTypes.NameIdentifier)?.Value;

            _logger.LogInformation(
                "Mark all notifications as read requested for UserId: {UserId}",
                userId);

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning(
                    "Mark all notifications as read failed because UserId is missing.");

                return Unauthorized();
            }

            try
            {
                await _notificationApiService
                    .MarkAllAsReadAsync(userId);

                _logger.LogInformation(
                    "All notifications marked as read successfully for UserId: {UserId}",
                    userId);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while marking all notifications as read for UserId: {UserId}",
                    userId);

                throw;
            }
        }
    }
}