using DoctorsHub.Application.DTOs.BusinessInsigts;
using DoctorsHub.Domain.Enums;
using DoctorsHub.Web.Services;
using Microsoft.AspNetCore.Authorization;
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
       
        public async Task<IActionResult> Index(AnalyticsTimeFilter filter = AnalyticsTimeFilter.Month)
        {

            BusinessInsightsDto businessInsights = await _businessInsightsApiService.GetBusinessInsightsAsync(filter);

            ViewBag.Filter = filter;
            return View(businessInsights);
        }
    }
}
