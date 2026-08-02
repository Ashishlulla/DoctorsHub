using DoctorsHub.Application.DTOs.Reports.AppointmentsReport;
using DoctorsHub.Domain.Enums;
using DoctorsHub.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoctorsHub.Web.Controllers
{
    [Route("[controller]")]
    public class ReportsController : Controller
    {
        //Private Feilds
        private readonly ReportsApiService _reportsApiService;

        //Constructor
        public ReportsController(ReportsApiService reportsApiService)
        {
            _reportsApiService = reportsApiService;
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Index()
        {
            return View();
        }

        // Displays report after filters are submitted
        [HttpGet]
        [Route("[action]")]
        public IActionResult AppointmentsReport()
        {
            var doctors = _reportsApiService.GetDoctorsAsync();
            var patients = _reportsApiService.GetPatientsAsync();

           ViewBag.Doctors = new SelectList(doctors.Result, "Id", "FullName");
           ViewBag.Patients = new SelectList(patients.Result, "Id", "FullName");

            return View(new List<AppointmentReportDto>());
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AppointmentsReport(AppointmentReportFilteredDto filter)
        {
            var reports = await _reportsApiService.GetAppointmentReportsAsync(filter);

            var doctors = await _reportsApiService.GetDoctorsAsync();
            var patients = await _reportsApiService.GetPatientsAsync();

            ViewBag.Doctors = new SelectList(doctors, "Id", "FullName", filter.DoctorId);
            ViewBag.Patients = new SelectList(patients, "Id", "FullName", filter.PatientId);
            ViewBag.Statuses = new SelectList(
                 new[]
                 {
                    new { Value = "Scheduled", Text = "Scheduled" },
                    new { Value = "Confirmed", Text = "Confirmed" },
                    new { Value = "Completed", Text = "Completed" },
                    new { Value = "Cancelled", Text = "Cancelled" }
                 },
                 "Value",
                 "Text",
                 filter.Status.ToString()
             );



            // Pass the filter back to the view so that the selected values can be retained

            ViewBag.FromDate = filter.FromDate;
            ViewBag.ToDate = filter.ToDate;
            ViewBag.DoctorId = filter.DoctorId;
            ViewBag.PatientId = filter.PatientId;
            ViewBag.Status = filter.Status;

            return View(reports);
        }



        [HttpGet]
        [Route("[action]")]
        public IActionResult DoctorsReport()
        {
            return View();
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult PatientsReport()
        {
            return View();
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult BillingReport()
        {
            return View();
        }
    }
}
