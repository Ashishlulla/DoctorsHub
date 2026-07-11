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
            List<AppointmentStatusChartDto> appointmentStatuses = await _businessInsightsService.GetAppointmentStatusChartAsync();

            return Ok(appointmentStatuses);
        }

        [HttpGet("appointment-trend")]
        public async Task<IActionResult> GetAppointmentTrendAsync()
        {
            List<AppointmentTrendDto> appointmentStatuses = await _businessInsightsService.GetAppointmentTrendsAsync();

            return Ok(appointmentStatuses);
        }

        [HttpGet("appointment-by-doctors")]
        public async Task<IActionResult> GetAppointmentByDoctorsAsync()
        {
            List<AppointmentsByDoctorDto> appointmentsByDoctors = await _businessInsightsService.GetAppointmentsByDoctorsAsync();

            return Ok(appointmentsByDoctors);
        }

        [HttpGet("appointment-peakhours")]
        public async Task<IActionResult> GetPeakAppointmentHoursAsync()
        {
            List<PeakAppointmentHoursDto> peakAppointmentHours= await _businessInsightsService.GetPeakAppointmentHoursAsync();

            return Ok(peakAppointmentHours);
        }
    }
}
