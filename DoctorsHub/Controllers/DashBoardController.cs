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
            var dashBoardData = _crmApiService.GetDashBoardDataAsync();
            var recentAppointments = _crmApiService.GetRecentAppointmentsAsync();
            var upcomingAppointments = _crmApiService.GetUpcomingAppointmentsAsync();

            await Task.WhenAll(dashBoardData, recentAppointments, upcomingAppointments);

            DashBoardDto data = await dashBoardData;
            data.RecentAppointments = await recentAppointments;
            data.UpcomingAppointments= await upcomingAppointments;

            return View(data);



            
        }
    }
}
