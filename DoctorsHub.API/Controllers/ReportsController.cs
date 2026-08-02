using DoctorsHub.Application.DTOs.Reports.AppointmentsReport;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.API.Controllers
{
    [Authorize(Roles ="Admin, Doctor, Receptionist")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        //Private Feilds
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost("appointments")]
        public async Task<IActionResult> GetAppointmentReportAsync([FromBody] AppointmentReportFilteredDto appointmentReportFiltered) 
        {
            List<AppointmentReportDto> reports = await _reportService.GetAppointmentReportsAsync(appointmentReportFiltered);
            
            return Ok(reports);
        }
    }
}
