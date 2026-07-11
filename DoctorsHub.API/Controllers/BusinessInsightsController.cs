using DoctorsHub.Application.DTOs.BusinessInsigts;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using Microsoft.AspNetCore.Http;
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

        [HttpGet("appointment-status")]
        public async Task<IActionResult> GetAppointmentStatusAsync() 
        {
            List<AppointmentStatusChartDto> appointmentStatuses = await _businessInsightsService.GetAppointmentStatusChart();

            return Ok(appointmentStatuses);
        }
    }
}
