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
        private readonly PdfExportService _pdfExportService;

        //Private Feilds API Services
        private readonly DoctorApiService _doctorApiService;
        private readonly PatientApiService _patientApiService;


        //Constructor
        public ReportsController(ReportsApiService reportsApiService, ExcelExportService excelExportService, PdfExportService pdfExportService, DoctorApiService doctorApiService, PatientApiService patientApiService)
        {
            _reportsApiService = reportsApiService;
            _excelExportService = excelExportService;
            _pdfExportService = pdfExportService;
           
            _doctorApiService = doctorApiService;
            _patientApiService = patientApiService;
            
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

            // Pass the filter back to the view so that the selected values can be retained

            ViewBag.FromDate = filter.FromDate;
            ViewBag.ToDate = filter.ToDate;
            ViewBag.Status = filter.Status;
            

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

        #region Export to PDF

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> ExportAppointmentsPdf(AppointmentReportFilteredDto filter)
        {
            var appointments = await _reportsApiService.GetAppointmentReportsAsync(filter);

            if (filter.DoctorId!.Value > 0 && filter.PatientId!.Value > 0)
            {
                var doctor = await _doctorApiService.GetDoctorByIdAsync((int)filter.DoctorId!);

                var patient = await _patientApiService.GetPatientByIdAsync((int)filter.PatientId!);

                if (doctor != null)
                    filter.DoctorName = doctor.FullName;

                if (patient != null)
                    filter.PatientName = patient.FullName;
            }


            var pdf = _pdfExportService.ExportAppointmentsPdf(appointments, filter);

            var reportName =
            $"AppointmentsReport_" +
            $"{(filter.FromDate==default ? filter.FromDate.ToString("yyyyMMdd") : "All")}-" +
            $"{(filter.ToDate== default ? filter.ToDate.ToString("yyyyMMdd") : "All")}-" +
            $"{filter.DoctorName ?? "AllDoctors"}-" +
            $"{filter.Status?.ToString() ?? "All"}+{DateTime.UtcNow.ToString("yyyy-MM-dd")}`";

            return File(pdf, "application/pdf", reportName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> ExportBillingPdf(BillingReportFilterDto filter)
        {
            var bills = await _reportsApiService.GetBillingReportsAsync(filter);

            if (filter.DoctorId!.Value>0 && filter.PatientId!.Value>0)
            {
                var doctor = await _doctorApiService.GetDoctorByIdAsync((int)filter.DoctorId!);

                var patient = await _patientApiService.GetPatientByIdAsync((int)filter.PatientId!);

                if (doctor != null)
                    filter.DoctorName = doctor.FullName;

                if (patient != null)
                    filter.PatientName = patient.FullName;
            }


            var pdf = _pdfExportService.ExportBillingPdf(bills, filter);

            var reportName =
            $"BillingReport_" +
            $"{(filter.FromDate == default ? filter.FromDate.ToString("yyyyMMdd") : "All")}-" +
            $"{(filter.ToDate == default ? filter.ToDate.ToString("yyyyMMdd") : "All")}-" +
            $"{filter.DoctorName ?? "AllDoctors"}-" +
            $"{filter.PaymentStatus?.ToString() ?? "All"}+{DateTime.UtcNow.ToString("yyyy-MM-dd")}";

            return File(pdf, "application/pdf", reportName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> ExportDoctorPdf(DoctorsReportFilteredDto filter)
        {
            var doctors = await _reportsApiService.GetDoctorsReportsAsync(filter);

            if (filter.SpecializationId!.Value>0)
            {
                var specialization = await _reportsApiService.GetSpecializationAsync();
                var name = specialization.FirstOrDefault(s => s.Id == filter.SpecializationId)!.Name;

                filter.SpecializationName = (string)name!;
            }

            var pdf = _pdfExportService.ExportDoctorsPdf(doctors, filter);

            var reportName =
                $"DoctorsReport_" +
                $"{filter.SpecializationName ?? "AllSpecializations"}-" +
                $"{filter.Qualification ?? "AllQualifications"}+{DateTime.UtcNow.ToString("yyyy-MM-dd")}";

            return File(pdf, "application/pdf", reportName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> ExportPatientsPdf(PatientsReportFilteredDto filter) 
        {
            //Extracting Patients report data
            var patientsReport = await _reportsApiService.GetPatientsReportsAsync(filter);

            //Passing patient reportfor pdf generation
            var pdf =  _pdfExportService.ExportPatientsPdf(patientsReport, filter);

            var reportName =
                $"PatientsReport_" +
                $"{filter.PatientName ?? "AllPatients"}-" +
                $"{filter.Gender ?? "All"}-" +
                $"{filter.BloodGroup ?? "All"}+{DateTime.UtcNow.ToString("yyyy-MM-dd")}";

            return File(pdf, "application/pdf", reportName);

        }
        #endregion
    }
}
