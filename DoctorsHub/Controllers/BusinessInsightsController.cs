using DoctorsHub.Application.DTOs.BusinessInsigts;
using DoctorsHub.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.Web.Controllers
{
    public class BusinessInsightsController : Controller
    {
        //Private Feilds
        private readonly BusinessInsightsApiService _businessInsightsApiService;

        //Constructor
        public BusinessInsightsController(BusinessInsightsApiService businessInsightsApiService) 
        {
            _businessInsightsApiService = businessInsightsApiService;
        }
       
        public async Task<IActionResult> Index()
        {

            var appointmentStatusTask = _businessInsightsApiService.GetAppointmentStatusesAsync();
            var appointmentTrendTask = _businessInsightsApiService.GetAppointmentTrendsAsync();
            var appointmentsByDoctorsTask = _businessInsightsApiService.GetAppointmentByDoctorsAsync();
            var peakAppointmentHoursTask = _businessInsightsApiService.GetPeakAppointmentHoursAsync();

            BusinessInsightsDto businessInsights = new BusinessInsightsDto();

            await Task.WhenAll(appointmentStatusTask, appointmentTrendTask,appointmentsByDoctorsTask, peakAppointmentHoursTask);
          

            businessInsights.GetAppointmentStatuses = await appointmentStatusTask;
            businessInsights.GetAppointmentsTrend = await appointmentTrendTask;
            businessInsights.GetAppointmentsByDoctors = await appointmentsByDoctorsTask;
            businessInsights.GetPeakAppointmentsHours = await peakAppointmentHoursTask;

            return View(businessInsights);
        }
    }
}
