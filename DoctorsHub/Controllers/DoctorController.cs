using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.Web.Controllers
{
    public class DoctorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
