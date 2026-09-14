using DoctorsHub.Application.DTOs.BusinessInsigts;
using DoctorsHub.Domain.Enums;
using DoctorsHub.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.Web.Controllers
{
    [Route("[controller]")]
    public class BusinessInsightsController : Controller
    {
        private readonly BusinessInsightsApiService _businessInsightsApiService;
        private readonly ILogger<BusinessInsightsController> _logger;

        public BusinessInsightsController(
            BusinessInsightsApiService businessInsightsApiService,
            ILogger<BusinessInsightsController> logger)
        {
            _businessInsightsApiService = businessInsightsApiService;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Index(
            AnalyticsTimeFilter filter = AnalyticsTimeFilter.Month)
        {
            _logger.LogInformation(
                "Business insights page requested with filter: {Filter}",
                filter);

            try
            {
                BusinessInsightsDto businessInsights =
                    await _businessInsightsApiService
                        .GetBusinessInsightsAsync(filter);

                ViewBag.Filter = filter;

                _logger.LogInformation(
                    "Business insights loaded successfully with filter: {Filter}",
                    filter);

                return View(businessInsights);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading business insights with filter: {Filter}",
                    filter);

                throw;
            }
        }
    }
}