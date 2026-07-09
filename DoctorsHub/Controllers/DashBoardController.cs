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
            var todaysAppointments = _crmApiService.GetTodaysAppointmentsAsync();
            var scheduledAppointments = _crmApiService.GetScheduledAppointmentsAsync();

            await Task.WhenAll(dashBoardData, recentAppointments, upcomingAppointments, todaysAppointments, scheduledAppointments);

            DashBoardDto data = await dashBoardData;
            data.RecentAppointments = await recentAppointments;
            data.UpcomingAppointments= await upcomingAppointments;
            data.TodayAppointments = await todaysAppointments;
            data.ScheduledAppointmentsList = await scheduledAppointments;

            return View(data);



            
        }
    }
}
