using DoctorsHub.Application.DTOs.Appoitments;
using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.DTOs.common.DoctorsHub.Application.DTOs.Common;
using DoctorsHub.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.Web.Controllers
{
    [Route("[controller]")]
    public class AppointmentsController : Controller
    {
        // Private fields
        private readonly AppointmentApiService _appointmentApiService;
        private readonly PatientApiService _patientApiService;
        private readonly DoctorApiService _doctorApiService;

        // Logger
        private readonly ILogger<AppointmentsController> _logger;

        // Constructor
        public AppointmentsController(
            AppointmentApiService appointmentApiService,
            PatientApiService patientApiService,
            DoctorApiService doctorApiService,
            ILogger<AppointmentsController> logger)
        {
            _appointmentApiService = appointmentApiService;
            _patientApiService = patientApiService;
            _doctorApiService = doctorApiService;
            _logger = logger;
        }

        
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Index(
            [FromQuery] AppointmentQueryParameter appointmentQueryParameter)
        {
            _logger.LogInformation(
                "Appointments index page requested.");

            try
            {
                PagedResult<AppointmentDto> appointments =
                    await _appointmentApiService.GetAppointmentsAsync(
                        appointmentQueryParameter);

                _logger.LogInformation(
                    "Appointments loaded successfully.");

                return View(appointments);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading appointments.");

                throw;
            }
        }

        
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation(
                "Create appointment page requested.");

            try
            {
                ViewBag.Patients =
                    await _patientApiService.GetAllPatientsAsync();

                ViewBag.Doctors =
                    await _doctorApiService.GetAllDoctorsAsync();

                _logger.LogInformation(
                    "Create appointment page loaded successfully.");

                return View(new CreateAppointmentDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading the create appointment page.");

                throw;
            }
        }

        
        [HttpPost]
        [Route("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateAppointmentDto createAppointmentDto)
        {
            _logger.LogInformation(
                "Create appointment attempt started.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning(
                    "Create appointment validation failed.");

                ViewBag.Patients =
                    await _patientApiService.GetAllPatientsAsync();

                ViewBag.Doctors =
                    await _doctorApiService.GetAllDoctorsAsync();

                return View(createAppointmentDto);
            }

            try
            {
                await _appointmentApiService.CreateAppointmentAsync(
                    createAppointmentDto);

                _logger.LogInformation(
                    "Appointment created successfully.");

                TempData["Success"] =
                    "Appointment created successfully..";

                return RedirectToAction(
                    nameof(AppointmentsController.Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while creating an appointment.");

                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                ViewBag.Patients =
                    await _patientApiService.GetAllPatientsAsync();

                ViewBag.Doctors =
                    await _doctorApiService.GetAllDoctorsAsync();

                return View(createAppointmentDto);
            }
        }

        
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation(
                "Edit appointment page requested for AppointmentId: {AppointmentId}",
                id);

            try
            {
                var appointment =
                    await _appointmentApiService
                        .GetAppointmentForUpdateAsync(id);

                ViewBag.Patients =
                    await _patientApiService.GetAllPatientsAsync();

                ViewBag.Doctors =
                    await _doctorApiService.GetAllDoctorsAsync();

                _logger.LogInformation(
                    "Edit appointment page loaded successfully for AppointmentId: {AppointmentId}",
                    id);

                return View(appointment);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading appointment for editing. AppointmentId: {AppointmentId}",
                    id);

                throw;
            }
        }

        
        [HttpPost]
        [Route("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            UpdateAppointmentDto updateAppointmentDto)
        {
            _logger.LogInformation(
                "Update appointment attempt started for AppointmentId: {AppointmentId}",
                updateAppointmentDto.Id);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning(
                    "Update appointment validation failed for AppointmentId: {AppointmentId}",
                    updateAppointmentDto.Id);

                ViewBag.Patients =
                    await _patientApiService.GetAllPatientsAsync();

                ViewBag.Doctors =
                    await _doctorApiService.GetAllDoctorsAsync();

                return View(updateAppointmentDto);
            }

            try
            {
                await _appointmentApiService
                    .UpdateAppointmentAsync(updateAppointmentDto);

                _logger.LogInformation(
                    "Appointment updated successfully. AppointmentId: {AppointmentId}",
                    updateAppointmentDto.Id);

                TempData["Success"] =
                    "Appointment updated successfully..";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while updating appointment. AppointmentId: {AppointmentId}",
                    updateAppointmentDto.Id);

                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                ViewBag.Patients =
                    await _patientApiService.GetAllPatientsAsync();

                ViewBag.Doctors =
                    await _doctorApiService.GetAllDoctorsAsync();

                return View(updateAppointmentDto);
            }
        }

        
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation(
                "Appointment details requested for AppointmentId: {AppointmentId}",
                id);

            try
            {
                AppointmentDetailsDto appointmentDetails =
                    await _appointmentApiService
                        .GetAppointmentForDetailsAsync(id);

                _logger.LogInformation(
                    "Appointment details loaded successfully for AppointmentId: {AppointmentId}",
                    id);

                return View(appointmentDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading appointment details. AppointmentId: {AppointmentId}",
                    id);

                throw;
            }
        }

        
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation(
                "Delete appointment confirmation page requested for AppointmentId: {AppointmentId}",
                id);

            try
            {
                AppointmentDto appointment =
                    await _appointmentApiService
                        .GetAppointmentByIdAsync(id);

                return View(appointment);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading appointment for deletion. AppointmentId: {AppointmentId}",
                    id);

                throw;
            }
        }

        
        [HttpPost]
        [ActionName(nameof(AppointmentsController.Delete))]
        [Route("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation(
                "Delete appointment attempt started for AppointmentId: {AppointmentId}",
                id);

            try
            {
                await _appointmentApiService.DeleteAppointmentAsync(id);

                _logger.LogInformation(
                    "Appointment deleted successfully. AppointmentId: {AppointmentId}",
                    id);

                TempData["Success"] =
                    "Appointment deleted successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while deleting appointment. AppointmentId: {AppointmentId}",
                    id);

                throw;
            }
        }

        
        [HttpPost]
        [Route("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(int id)
        {
            _logger.LogInformation(
                "Appointment confirmation attempt started for AppointmentId: {AppointmentId}",
                id);

            try
            {
                await _appointmentApiService
                    .ConfirmAppointmentAsync(id);

                _logger.LogInformation(
                    "Appointment confirmed successfully. AppointmentId: {AppointmentId}",
                    id);

                TempData["Success"] =
                    "Appointment Confirmed Successfully.";

                return RedirectToAction(
                    nameof(Details),
                    new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while confirming appointment. AppointmentId: {AppointmentId}",
                    id);

                throw;
            }
        }

        
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Reschedule(int id)
        {
            _logger.LogInformation(
                "Reschedule appointment page requested for AppointmentId: {AppointmentId}",
                id);

            try
            {
                AppointmentDto appointment =
                    await _appointmentApiService
                        .GetAppointmentByIdAsync(id);

                RescheduleAppointmentDto rescheduleAppointmentDto =
                    new RescheduleAppointmentDto
                    {
                        Id = appointment.Id,
                        AppointmentDate = appointment.AppointmentDate,
                        StartTime = appointment.StartTime,
                        EndTime = appointment.EndTime
                    };

                return View(rescheduleAppointmentDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading appointment for rescheduling. AppointmentId: {AppointmentId}",
                    id);

                throw;
            }
        }

        
        [HttpPost]
        [Route("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reschedule(
            RescheduleAppointmentDto rescheduleAppointmentDto)
        {
            _logger.LogInformation(
                "Reschedule appointment attempt started for AppointmentId: {AppointmentId}",
                rescheduleAppointmentDto.Id);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning(
                    "Reschedule appointment validation failed for AppointmentId: {AppointmentId}",
                    rescheduleAppointmentDto.Id);

                return View(rescheduleAppointmentDto);
            }

            try
            {
                await _appointmentApiService
                    .RescheduleAppointmentAsync(
                        rescheduleAppointmentDto);

                _logger.LogInformation(
                    "Appointment rescheduled successfully. AppointmentId: {AppointmentId}",
                    rescheduleAppointmentDto.Id);

                TempData["Success"] =
                    "Appointment rescheduled successfully";

                return RedirectToAction(
                    nameof(Details),
                    new
                    {
                        rescheduleAppointmentDto.Id
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while rescheduling appointment. AppointmentId: {AppointmentId}",
                    rescheduleAppointmentDto.Id);

                throw;
            }
        }

        
        [HttpPost]
        [Route("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            _logger.LogInformation(
                "Cancel appointment attempt started for AppointmentId: {AppointmentId}",
                id);

            try
            {
                await _appointmentApiService
                    .CancelAppointmentAsync(id);

                _logger.LogInformation(
                    "Appointment cancelled successfully. AppointmentId: {AppointmentId}",
                    id);

                TempData["Success"] =
                    "Appointment cancelled successfully";

                return RedirectToAction(
                    nameof(Details),
                    new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while cancelling appointment. AppointmentId: {AppointmentId}",
                    id);

                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(
                nameof(Details),
                new { id });
        }

        
        [HttpPost]
        [Route("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int id)
        {
            _logger.LogInformation(
                "Complete appointment attempt started for AppointmentId: {AppointmentId}",
                id);

            try
            {
                await _appointmentApiService
                    .CompleteAppointmentAsync(id);

                _logger.LogInformation(
                    "Appointment completed successfully. AppointmentId: {AppointmentId}",
                    id);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while completing appointment. AppointmentId: {AppointmentId}",
                    id);

                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(
                nameof(Details),
                new { id });
        }
    }
}