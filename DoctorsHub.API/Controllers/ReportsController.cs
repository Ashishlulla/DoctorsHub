using DoctorsHub.Application.DTOs.Reports.AppointmentsReport;
using DoctorsHub.Application.DTOs.Reports.BillingReport;
using DoctorsHub.Application.DTOs.Reports.DoctorsReport;
using DoctorsHub.Application.DTOs.Reports.PatientsReport;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.API.Controllers
{
    //[Authorize(Roles ="Admin, Doctor, Receptionist")]
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

        [HttpPost("Billing")]
        public async Task<IActionResult> GetBillingReportAsync([FromBody] BillingReportFilterDto billingReportFilter)
        {
            List<BillingReportDto> reports = await _reportService.GetBillingReportsAsync(billingReportFilter);

            return Ok(reports);
        }

        [HttpPost("Doctors")]
        public async Task<IActionResult> GetDoctorsReportAsync([FromBody] DoctorsReportFilteredDto doctorsReportFiltered)
        {
            List<DoctorsReportDto> reports = await _reportService.GetDoctorsReportsAsync(doctorsReportFiltered);

            return Ok(reports);
        }

        [HttpPost("Patients")]
        public async Task<IActionResult> GetPatientsReportAsync([FromBody] PatientsReportFilteredDto patientsReportFiltered)
        {
            List<PatientsReportDto> reports = await _reportService.GetPatientsReportsAsync(patientsReportFiltered);

            return Ok(reports);
        }
    }
}
