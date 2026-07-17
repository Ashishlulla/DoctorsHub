using DoctorsHub.Application.DTOs.BusinessInsigts;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using DoctorsHub.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessInsightsController : ControllerBase
    {
        //Private Feilds
        private readonly IBusinessInsightsService _businessInsightsService;

        //Condstructor
        public BusinessInsightsController(IBusinessInsightsService businessInsightsService) 
        {
            _businessInsightsService = businessInsightsService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetBusinessInsightsAsync([FromQuery]AnalyticsTimeFilter timeFilter =AnalyticsTimeFilter.Month) 
        {
            BusinessInsightsDto businessInsights = await _businessInsightsService.GetBusinessInsightsAsync(timeFilter);

            return Ok(businessInsights);

        }
    }
}
