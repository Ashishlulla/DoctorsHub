using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.Web.Controllers
{
    public class BusinessInsightsController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }
    }
}
