using DoctorsHub.Application.DTOs.CRM;
using DoctorsHub.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.Web.Controllers
{
    public class DashBoardController : Controller
    {
        //Private Feild
        private readonly CRMApiService _crmApiService;

        //Constructor
        public DashBoardController(CRMApiService crmApiService) 
        {
            _crmApiService = crmApiService;
        }
        public async Task<IActionResult> Index()
        {
            DashBoardDto data = await _crmApiService.GetDashBoardDataAsync();
            
            return View(data);
        }
    }
}
