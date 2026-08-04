using DoctorsHub.Application.DTOs.Reports.AppointmentsReport;
using DoctorsHub.Application.DTOs.Reports.BillingReport;
using DoctorsHub.Application.DTOs.Reports.DoctorsReport;
using DoctorsHub.Application.DTOs.Reports.PatientsReport;
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


        private async Task LoadDropdowns<T>(T filter)
        {
            // Implementation for loading dropdowns
            var doctors = await _reportsApiService.GetDoctorsAsync();
            var patients = await _reportsApiService.GetPatientsAsync();

            if(filter is AppointmentReportFilteredDto appointmentFilter)
            {
                ViewBag.Doctors = new SelectList(doctors, "Id", "FullName", appointmentFilter.DoctorId);
                ViewBag.Patients = new SelectList(patients, "Id", "FullName", appointmentFilter.PatientId);
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
                 appointmentFilter.Status.ToString()
             );
            }


            if(filter is BillingReportFilterDto billingFilter)
            {
                ViewBag.Doctors = new SelectList(doctors, "Id", "FullName", billingFilter.DoctorId);
                ViewBag.Patients = new SelectList(patients, "Id", "FullName", billingFilter.PatientId);
            }
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
        public async Task<IActionResult> AppointmentsReport()
        {

           await LoadDropdowns(new AppointmentReportFilteredDto());

            return View(new List<AppointmentReportDto>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> AppointmentsReport(AppointmentReportFilteredDto filter)
        {
            var reports = await _reportsApiService.GetAppointmentReportsAsync(filter);

            await LoadDropdowns(filter);




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
            

            return View(reports);
        }



        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> BillingReport()
        {
            await LoadDropdowns(new BillingReportFilterDto());
            

            return View(new List<BillingReportDto>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> BillingReport(BillingReportFilterDto filter)
        {
            var reports = await _reportsApiService.GetBillingReportsAsync(filter);

            await LoadDropdowns(filter);

            ViewBag.FromDate = filter.FromDate;
            ViewBag.ToDate = filter.ToDate;
            ViewBag.Status = filter.PaymentStatus.HasValue ? filter.PaymentStatus.Value.ToString() : null;

            return View(reports);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> DoctorsReport()
        {
            var specialization = await _reportsApiService.GetSpecializationAsync();
            ViewBag.Specializations = new SelectList(specialization, "Id", "Name");


            return View(new List<DoctorsReportDto>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> DoctorsReport(DoctorsReportFilteredDto filter)
        {
            var reports = await _reportsApiService.GetDoctorsReportsAsync(filter);

            var specialization = await _reportsApiService.GetSpecializationAsync();
            ViewBag.Specializations = new SelectList(specialization, "Id", "Name", filter.SpecializationId);

            ViewBag.Qualification = filter.Qualification;

            return View(reports);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> PatientsReport()
        {

            ViewBag.PatientName = string.Empty;
            ViewBag.Gender = string.Empty;
            ViewBag.BloodGroup = string.Empty;

            return View(new List<PatientsReportDto>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> PatientsReport(PatientsReportFilteredDto filter)
        {
            var reports = await _reportsApiService.GetPatientsReportsAsync(filter);

            ViewBag.PatientName = filter.PatientName;
            ViewBag.Gender = filter.Gender;
            ViewBag.BloodGroup = filter.BloodGroup;


            return View(reports);
        }
    }
}
