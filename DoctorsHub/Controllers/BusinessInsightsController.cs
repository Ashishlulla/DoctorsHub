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
            List<AppointmentStatusChartDto> appointmentStatuses = await _businessInsightsApiService.GetAppointmentStatusesAsync();

            BusinessInsightsDto businessInsights = new BusinessInsightsDto();


            businessInsights.GetAppointmentStatuses = appointmentStatuses;

            return View(businessInsights);
        }
    }
}
