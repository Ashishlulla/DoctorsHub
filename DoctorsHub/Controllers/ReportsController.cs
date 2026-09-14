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
        private readonly ReportsApiService _reportsApiService;
        private readonly ExcelExportService _excelExportService;
        private readonly PdfExportService _pdfExportService;
        private readonly DoctorApiService _doctorApiService;
        private readonly PatientApiService _patientApiService;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(
            ReportsApiService reportsApiService,
            ExcelExportService excelExportService,
            PdfExportService pdfExportService,
            DoctorApiService doctorApiService,
            PatientApiService patientApiService,
            ILogger<ReportsController> logger)
        {
            _reportsApiService = reportsApiService;
            _excelExportService = excelExportService;
            _pdfExportService = pdfExportService;
            _doctorApiService = doctorApiService;
            _patientApiService = patientApiService;
            _logger = logger;
        }

        private async Task LoadDropdowns<T>(T filter)
        {
            _logger.LogInformation(
                "Loading report dropdown data.");

            try
            {
                var doctors =
                    await _reportsApiService.GetDoctorsAsync();

                var patients =
                    await _reportsApiService.GetPatientsAsync();

                if (filter is AppointmentReportFilteredDto appointmentFilter)
                {
                    ViewBag.Doctors =
                        new SelectList(
                            doctors,
                            "Id",
                            "FullName",
                            appointmentFilter.DoctorId);

                    ViewBag.Patients =
                        new SelectList(
                            patients,
                            "Id",
                            "FullName",
                            appointmentFilter.PatientId);

                    ViewBag.Statuses =
                        new SelectList(
                            new[]
                            {
                                new { Value = "Scheduled", Text = "Scheduled" },
                                new { Value = "Confirmed", Text = "Confirmed" },
                                new { Value = "Completed", Text = "Completed" },
                                new { Value = "Cancelled", Text = "Cancelled" }
                            },
                            "Value",
                            "Text",
                            appointmentFilter.Status.ToString());
                }

                if (filter is BillingReportFilterDto billingFilter)
                {
                    ViewBag.Doctors =
                        new SelectList(
                            doctors,
                            "Id",
                            "FullName",
                            billingFilter.DoctorId);

                    ViewBag.Patients =
                        new SelectList(
                            patients,
                            "Id",
                            "FullName",
                            billingFilter.PatientId);
                }

                _logger.LogInformation(
                    "Report dropdown data loaded successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading report dropdown data.");

                throw;
            }
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Index()
        {
            _logger.LogInformation(
                "Reports index page requested.");

            return View();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> AppointmentsReport()
        {
            _logger.LogInformation(
                "Appointments report page requested.");

            try
            {
                await LoadDropdowns(
                    new AppointmentReportFilteredDto());

                _logger.LogInformation(
                    "Appointments report page loaded successfully.");

                return View(new List<AppointmentReportDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading appointments report page.");

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> AppointmentsReport(
            AppointmentReportFilteredDto filter)
        {
            _logger.LogInformation(
                "Appointments report generation requested.");

            try
            {
                var reports =
                    await _reportsApiService
                        .GetAppointmentReportsAsync(filter);

                await LoadDropdowns(filter);

                ViewBag.FromDate = filter.FromDate;
                ViewBag.ToDate = filter.ToDate;
                ViewBag.Status = filter.Status;

                _logger.LogInformation(
                    "Appointments report generated successfully.");

                return View(reports);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while generating appointments report.");

                throw;
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> BillingReport()
        {
            _logger.LogInformation(
                "Billing report page requested.");

            try
            {
                await LoadDropdowns(
                    new BillingReportFilterDto());

                _logger.LogInformation(
                    "Billing report page loaded successfully.");

                return View(new List<BillingReportDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading billing report page.");

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> BillingReport(
            BillingReportFilterDto filter)
        {
            _logger.LogInformation(
                "Billing report generation requested.");

            try
            {
                var reports =
                    await _reportsApiService
                        .GetBillingReportsAsync(filter);

                await LoadDropdowns(filter);

                ViewBag.FromDate = filter.FromDate;
                ViewBag.ToDate = filter.ToDate;
                ViewBag.Status =
                    filter.PaymentStatus.HasValue
                        ? filter.PaymentStatus.Value.ToString()
                        : null;

                _logger.LogInformation(
                    "Billing report generated successfully.");

                return View(reports);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while generating billing report.");

                throw;
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> DoctorsReport()
        {
            _logger.LogInformation(
                "Doctors report page requested.");

            try
            {
                var specialization =
                    await _reportsApiService
                        .GetSpecializationAsync();

                ViewBag.Specializations =
                    new SelectList(
                        specialization,
                        "Id",
                        "Name");

                _logger.LogInformation(
                    "Doctors report page loaded successfully.");

                return View(new List<DoctorsReportDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading doctors report page.");

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> DoctorsReport(
            DoctorsReportFilteredDto filter)
        {
            _logger.LogInformation(
                "Doctors report generation requested.");

            try
            {
                var reports =
                    await _reportsApiService
                        .GetDoctorsReportsAsync(filter);

                var specialization =
                    await _reportsApiService
                        .GetSpecializationAsync();

                ViewBag.Specializations =
                    new SelectList(
                        specialization,
                        "Id",
                        "Name",
                        filter.SpecializationId);

                ViewBag.Qualification =
                    filter.Qualification;

                _logger.LogInformation(
                    "Doctors report generated successfully.");

                return View(reports);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while generating doctors report.");

                throw;
            }
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult PatientsReport()
        {
            _logger.LogInformation(
                "Patients report page requested.");

            ViewBag.PatientName = string.Empty;
            ViewBag.Gender = string.Empty;
            ViewBag.BloodGroup = string.Empty;

            return View(new List<PatientsReportDto>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> PatientsReport(
            PatientsReportFilteredDto filter)
        {
            _logger.LogInformation(
                "Patients report generation requested.");

            try
            {
                var reports =
                    await _reportsApiService
                        .GetPatientsReportsAsync(filter);

                ViewBag.PatientName =
                    filter.PatientName;

                ViewBag.Gender =
                    filter.Gender;

                ViewBag.BloodGroup =
                    filter.BloodGroup;

                _logger.LogInformation(
                    "Patients report generated successfully.");

                return View(reports);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while generating patients report.");

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> ExportAppointmentToExcel(
            AppointmentReportFilteredDto appointmentReportFiltered)
        {
            _logger.LogInformation(
                "Appointment report Excel export requested.");

            try
            {
                var report =
                    await _reportsApiService
                        .GetAppointmentReportsAsync(
                            appointmentReportFiltered);

                var stream =
                    _excelExportService
                        .ExportAppointmentExcelFile(report);

                var fileName =
                    $"AppointmentReports-{DateTime.Now:yyyyMMdd-HHmmss}.xlsx";

                _logger.LogInformation(
                    "Appointment report Excel file generated successfully.");

                return File(
                    stream,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while exporting appointment report to Excel.");

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> ExportBillingExcel(
            BillingReportFilterDto billingReportFiltered)
        {
            _logger.LogInformation(
                "Billing report Excel export requested.");

            try
            {
                var report =
                    await _reportsApiService
                        .GetBillingReportsAsync(
                            billingReportFiltered);

                var stream =
                    _excelExportService
                        .ExportBillingExcelFile(report);

                var fileName =
                    $"BillingReports-{DateTime.Now:yyyyMMdd-HHmmss}.xlsx";

                _logger.LogInformation(
                    "Billing report Excel file generated successfully.");

                return File(
                    stream,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while exporting billing report to Excel.");

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> ExportDoctorsExcel(
            DoctorsReportFilteredDto doctorsReportFiltered)
        {
            _logger.LogInformation(
                "Doctors report Excel export requested.");

            try
            {
                var report =
                    await _reportsApiService
                        .GetDoctorsReportsAsync(
                            doctorsReportFiltered);

                var stream =
                    _excelExportService
                        .ExportDoctorsExcelfile(report);

                var fileName =
                    $"DoctorsReports-{DateTime.Now:yyyyMMdd-HHmmss}.xlsx";

                _logger.LogInformation(
                    "Doctors report Excel file generated successfully.");

                return File(
                    stream,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while exporting doctors report to Excel.");

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> ExportPatientsExcel(
            PatientsReportFilteredDto patientsReportFiltered)
        {
            _logger.LogInformation(
                "Patients report Excel export requested.");

            try
            {
                var report =
                    await _reportsApiService
                        .GetPatientsReportsAsync(
                            patientsReportFiltered);

                var stream =
                    _excelExportService
                        .ExportPatientsExcelfile(report);

                var fileName =
                    $"PatientsReports-{DateTime.Now:yyyyMMdd-HHmmss}.xlsx";

                _logger.LogInformation(
                    "Patients report Excel file generated successfully.");

                return File(
                    stream,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while exporting patients report to Excel.");

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> ExportAppointmentsPdf(
            AppointmentReportFilteredDto filter)
        {
            _logger.LogInformation(
                "Appointments report PDF export requested.");

            try
            {
                var appointments =
                    await _reportsApiService
                        .GetAppointmentReportsAsync(filter);

                if (filter.DoctorId.HasValue &&
                    filter.PatientId.HasValue &&
                    filter.DoctorId.Value > 0 &&
                    filter.PatientId.Value > 0)
                {
                    var doctor =
                        await _doctorApiService
                            .GetDoctorByIdAsync(
                                filter.DoctorId.Value);

                    var patient =
                        await _patientApiService
                            .GetPatientByIdAsync(
                                filter.PatientId.Value);

                    if (doctor != null)
                    {
                        filter.DoctorName =
                            doctor.FullName;
                    }

                    if (patient != null)
                    {
                        filter.PatientName =
                            patient.FullName;
                    }
                }

                var pdf =
                    _pdfExportService
                        .ExportAppointmentsPdf(
                            appointments,
                            filter);

                var reportName =
                    $"AppointmentsReport_" +
                    $"{(filter.FromDate == default ? filter.FromDate.ToString("yyyyMMdd") : "All")}-" +
                    $"{(filter.ToDate == default ? filter.ToDate.ToString("yyyyMMdd") : "All")}-" +
                    $"{filter.DoctorName ?? "AllDoctors"}-" +
                    $"{filter.Status?.ToString() ?? "All"}+" +
                    $"{DateTime.UtcNow:yyyy-MM-dd}.pdf";

                _logger.LogInformation(
                    "Appointments report PDF generated successfully.");

                return File(
                    pdf,
                    "application/pdf",
                    reportName);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while exporting appointments report to PDF.");

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> ExportBillingPdf(
            BillingReportFilterDto filter)
        {
            _logger.LogInformation(
                "Billing report PDF export requested.");

            try
            {
                var bills =
                    await _reportsApiService
                        .GetBillingReportsAsync(filter);

                if (filter.DoctorId.HasValue &&
                    filter.PatientId.HasValue &&
                    filter.DoctorId.Value > 0 &&
                    filter.PatientId.Value > 0)
                {
                    var doctor =
                        await _doctorApiService
                            .GetDoctorByIdAsync(
                                filter.DoctorId.Value);

                    var patient =
                        await _patientApiService
                            .GetPatientByIdAsync(
                                filter.PatientId.Value);

                    if (doctor != null)
                    {
                        filter.DoctorName =
                            doctor.FullName;
                    }

                    if (patient != null)
                    {
                        filter.PatientName =
                            patient.FullName;
                    }
                }

                var pdf =
                    _pdfExportService
                        .ExportBillingPdf(
                            bills,
                            filter);

                var reportName =
                    $"BillingReport_" +
                    $"{(filter.FromDate == default ? filter.FromDate.ToString("yyyyMMdd") : "All")}-" +
                    $"{(filter.ToDate == default ? filter.ToDate.ToString("yyyyMMdd") : "All")}-" +
                    $"{filter.DoctorName ?? "AllDoctors"}-" +
                    $"{filter.PaymentStatus?.ToString() ?? "All"}+" +
                    $"{DateTime.UtcNow:yyyy-MM-dd}.pdf";

                _logger.LogInformation(
                    "Billing report PDF generated successfully.");

                return File(
                    pdf,
                    "application/pdf",
                    reportName);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while exporting billing report to PDF.");

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> ExportDoctorPdf(
            DoctorsReportFilteredDto filter)
        {
            _logger.LogInformation(
                "Doctors report PDF export requested.");

            try
            {
                var doctors =
                    await _reportsApiService
                        .GetDoctorsReportsAsync(filter);

                if (filter.SpecializationId.HasValue &&
                    filter.SpecializationId.Value > 0)
                {
                    var specialization =
                        await _reportsApiService
                            .GetSpecializationAsync();

                    var selectedSpecialization =
                        specialization.FirstOrDefault(
                            s => s.Id == filter.SpecializationId.Value);

                    if (selectedSpecialization != null)
                    {
                        filter.SpecializationName =
                            selectedSpecialization.Name;
                    }
                }

                var pdf =
                    _pdfExportService
                        .ExportDoctorsPdf(
                            doctors,
                            filter);

                var reportName =
                    $"DoctorsReport_" +
                    $"{filter.SpecializationName ?? "AllSpecializations"}-" +
                    $"{filter.Qualification ?? "AllQualifications"}+" +
                    $"{DateTime.UtcNow:yyyy-MM-dd}.pdf";

                _logger.LogInformation(
                    "Doctors report PDF generated successfully.");

                return File(
                    pdf,
                    "application/pdf",
                    reportName);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while exporting doctors report to PDF.");

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> ExportPatientsPdf(
            PatientsReportFilteredDto filter)
        {
            _logger.LogInformation(
                "Patients report PDF export requested.");

            try
            {
                var patientsReport =
                    await _reportsApiService
                        .GetPatientsReportsAsync(filter);

                var pdf =
                    _pdfExportService
                        .ExportPatientsPdf(
                            patientsReport,
                            filter);

                var reportName =
                    $"PatientsReport_" +
                    $"{filter.PatientName ?? "AllPatients"}-" +
                    $"{filter.Gender ?? "All"}-" +
                    $"{filter.BloodGroup ?? "All"}+" +
                    $"{DateTime.UtcNow:yyyy-MM-dd}.pdf";

                _logger.LogInformation(
                    "Patients report PDF generated successfully.");

                return File(
                    pdf,
                    "application/pdf",
                    reportName);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while exporting patients report to PDF.");

                throw;
            }
        }
    }
}