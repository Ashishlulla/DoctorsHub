using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.Web.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
