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
            var revenueTrendTask = _businessInsightsApiService.GetRevenueTrendAsync();
            var revenueByDoctorTask = _businessInsightsApiService.GetRevenueByDoctorsAsync();
            var TopRevenueGeneratingDoctorTask = _businessInsightsApiService.GetTopRevenueGeneratingsAsync();
            var AverageRevenuGeneratedByDoctorTask = _businessInsightsApiService.GetAverageRevenueGeneratedByDoctorsAsync();



            BusinessInsightsDto businessInsights = new BusinessInsightsDto();

            await Task.WhenAll(appointmentStatusTask, appointmentTrendTask,appointmentsByDoctorsTask, peakAppointmentHoursTask, revenueTrendTask, revenueByDoctorTask, TopRevenueGeneratingDoctorTask, AverageRevenuGeneratedByDoctorTask);
          

            businessInsights.GetAppointmentStatuses = await appointmentStatusTask;
            businessInsights.GetAppointmentsTrend = await appointmentTrendTask;
            businessInsights.GetAppointmentsByDoctors = await appointmentsByDoctorsTask;
            businessInsights.GetPeakAppointmentsHours = await peakAppointmentHoursTask;
            businessInsights.GetRevenueTrends = await revenueTrendTask;
            businessInsights.GetRevenueByDoctors = await revenueByDoctorTask;
            businessInsights.GetTopRevenueGeneratingDoctors = await TopRevenueGeneratingDoctorTask;
            businessInsights.GetAverageRevenueGeneratedByDoctors = await AverageRevenuGeneratedByDoctorTask;

            return View(businessInsights);
        }
    }
}
