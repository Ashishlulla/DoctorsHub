using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.Web.Controllers
{
    public class NotificationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
