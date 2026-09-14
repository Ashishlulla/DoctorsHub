using DoctorsHub.Application.DTOs.CRM;
using DoctorsHub.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.Web.Controllers
{
    [Route("[controller]")]
    public class DashBoardController : Controller
    {
        private readonly CRMApiService _crmApiService;
        private readonly ILogger<DashBoardController> _logger;

        public DashBoardController(
            CRMApiService crmApiService,
            ILogger<DashBoardController> logger)
        {
            _crmApiService = crmApiService;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation(
                "Dashboard page requested.");

            try
            {
                var dashBoardData =
                    _crmApiService.GetDashBoardDataAsync();

                var recentAppointments =
                    _crmApiService.GetRecentAppointmentsAsync();

                var upcomingAppointments =
                    _crmApiService.GetUpcomingAppointmentsAsync();

                var todaysAppointments =
                    _crmApiService.GetTodaysAppointmentsAsync();

                var scheduledAppointments =
                    _crmApiService.GetScheduledAppointmentsAsync();

                await Task.WhenAll(
                    dashBoardData,
                    recentAppointments,
                    upcomingAppointments,
                    todaysAppointments,
                    scheduledAppointments);

                DashBoardDto data = await dashBoardData;

                data.RecentAppointments =
                    await recentAppointments;

                data.UpcomingAppointments =
                    await upcomingAppointments;

                data.TodayAppointments =
                    await todaysAppointments;

                data.ScheduledAppointmentsList =
                    await scheduledAppointments;

                _logger.LogInformation(
                    "Dashboard data loaded successfully.");

                return View(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading dashboard data.");

                throw;
            }
        }
    }
}