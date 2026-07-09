using DoctorsHub.Application.DTOs.CRM;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        //Private Feilds 
        private readonly ICRMService _crmService;

        //Constructor
        public DashBoardController(ICRMService crmService) 
        {
            _crmService = crmService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashBoardAsync() 
        {
            DashBoardDto dashBoardData = await _crmService.GetDashBoardAsync();

            return Ok(dashBoardData);
        }

        [HttpGet("recent")]
        public async Task<IActionResult> GetRecentAppoitment() 
        {
            List<RecentAppointmentsDto> recentAppointments = await _crmService.GetRecentAppointmentsAsync();

            return Ok(recentAppointments);
        }

        [HttpGet("upcoming")]
        public async Task<IActionResult> GetUpcomingAppointsmentAsync() 
        {
            List<UpcomingAppointmentsDto> upcomingAppointments = await _crmService.GetUpcomingAppointmentsAsync();

            return Ok(upcomingAppointments);
        }
    }
}
