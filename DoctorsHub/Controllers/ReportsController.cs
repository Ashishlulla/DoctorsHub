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
        private readonly ExcelExportService _excelExportService;

        //Constructor
        public ReportsController(ReportsApiService reportsApiService, ExcelExportService excelExportService)
        {
            _reportsApiService = reportsApiService;
            _excelExportService = excelExportService;
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


        #region Reports
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

        #endregion

        #region Export to Excel

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> ExportAppointmentToExcel(AppointmentReportFilteredDto appointmentReportFiltered) 
        {
            // Get the filtered appointment reports

            var report = await _reportsApiService.GetAppointmentReportsAsync(appointmentReportFiltered);

            //Generate Excel file
            var stream =  _excelExportService.ExportAppointmentExcelFile(report);

            var fileName = $"AppointmentReports-{DateTime.Now:yyyyMMdd-HHmmss}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> ExportBillingExcel(BillingReportFilterDto billingReportFiltered)
        {
            // Get the filtered billing reports

            var report = await _reportsApiService.GetBillingReportsAsync(billingReportFiltered);

            //Generate Excel file
            var stream = _excelExportService.ExportBillingExcelFile(report);

            var fileName = $"BillingReports-{DateTime.Now:yyyyMMdd-HHmmss}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> ExportDoctorsExcel(DoctorsReportFilteredDto doctorsReportFiltered)
        {
            // Get the filtered doctors reports

            var report = await _reportsApiService.GetDoctorsReportsAsync(doctorsReportFiltered);

            //Generate Excel file
            var stream = _excelExportService.ExportDoctorsExcelfile(report);

            var fileName = $"DoctorsReports-{DateTime.Now:yyyyMMdd-HHmmss}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> ExportPatientsExcel(PatientsReportFilteredDto patientsReportFiltered)
        {
            // Get the filtered patients reports

            var report = await _reportsApiService.GetPatientsReportsAsync(patientsReportFiltered);

            //Generate Excel file
            var stream = _excelExportService.ExportPatientsExcelfile(report);

            var fileName = $"PatientsReports-{DateTime.Now:yyyyMMdd-HHmmss}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        #endregion
    }
}
