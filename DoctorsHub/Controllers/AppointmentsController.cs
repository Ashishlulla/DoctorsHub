using DoctorsHub.Application.DTOs.Appoitments;
using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.DTOs.common.DoctorsHub.Application.DTOs.Common;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using DoctorsHub.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.Web.Controllers
{
    public class AppointmentsController : Controller
    {
        //private feilds
        private readonly AppointmentApiService _appointmentApiService;
        private readonly PatientApiService _patientApiService;
        private readonly DoctorApiService _doctorApiService;

        //constructor
        public AppointmentsController(AppointmentApiService appointmentApiService, PatientApiService patientApiService, DoctorApiService doctorApiService)
        {
            _appointmentApiService = appointmentApiService;
            _patientApiService = patientApiService;
            _doctorApiService = doctorApiService;
        }

        //GET: Index Action Method
        public async Task<IActionResult> Index([FromQuery] AppointmentQueryParameter appointmentQueryParameter)
        {
             PagedResult<AppointmentDto> appointments = await _appointmentApiService.GetAppointmentsAsync(appointmentQueryParameter);
       
             return View(appointments);
        }
        

        //GET: Create Action Method
        public async Task<IActionResult> Create()
        {
            
            ViewBag.Patients = await _patientApiService.GetAllPatientsAsync();
            ViewBag.Doctors = await _doctorApiService.GetAllDoctorsAsync();

            return View(new CreateAppointmentDto());
        }

        //POST: Create Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAppointmentDto createAppointmentDto) 
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Patients = await _patientApiService.GetAllPatientsAsync();
                ViewBag.Doctors = await _doctorApiService.GetAllDoctorsAsync();

                return View(createAppointmentDto);
            }
            try
            {
                await _appointmentApiService.CreateAppointmentAsync(createAppointmentDto);

                TempData["Success"] = "Appointment created successfully..";
                return RedirectToAction(nameof(AppointmentsController.Index));
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                ViewBag.Patients = await _patientApiService.GetAllPatientsAsync();
                ViewBag.Doctors = await _doctorApiService.GetAllDoctorsAsync();

                return View(createAppointmentDto);

            }
           
        }

        //GET: Edit Action Method
        public async Task<IActionResult> Edit(int id) 
        {
            var appointment = await _appointmentApiService.GetAppointmentForUpdateAsync(id);

            ViewBag.Patients = await _patientApiService.GetAllPatientsAsync();
            ViewBag.Doctors = await _doctorApiService.GetAllDoctorsAsync();
            return View(appointment);
        }

        //POST: Edit Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateAppointmentDto updateAppointmentDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Patients = await _patientApiService.GetAllPatientsAsync();
                ViewBag.Doctors = await _doctorApiService.GetAllDoctorsAsync();

                return View(updateAppointmentDto);
            }
            try { 
                await _appointmentApiService.UpdateAppointmentAsync(updateAppointmentDto);

                TempData["Success"] = "Appointment updated successfully..";

                return RedirectToAction(nameof(Index)); 
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                ViewBag.Patients = await _patientApiService.GetAllPatientsAsync();
                ViewBag.Doctors = await _doctorApiService.GetAllDoctorsAsync();

                return View(updateAppointmentDto);

            }
        }
        //GET: Details Action Method
        public async Task<IActionResult> Details(int id) 
        {
            AppointmentDetailsDto appointmentDetails = await _appointmentApiService.GetAppointmentForDetailsAsync(id);
            return View(appointmentDetails);
        }

        //GET: Delete Action Method
        public async Task<IActionResult> Delete(int id) 
        {
            AppointmentDto appointment = await _appointmentApiService.GetAppointmentByIdAsync(id);
            return View(appointment);
        }

        //POST: DeleteConfirmed Action Method
        [HttpPost]
        [ActionName(nameof(AppointmentsController.Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _appointmentApiService.DeleteAppointmentAsync(id);

            TempData["Success"] = "Appointment deleted successfully.";
            return RedirectToAction(nameof(Index));


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(int id) 
        {
            await _appointmentApiService.ConfirmAppointmentAsync(id);

            TempData["Success"] = "Appointment Confirmed Successfully.";

            return RedirectToAction(nameof(Details), new { id });
        }

        public async Task<IActionResult> Reschedule(int id) 
        {
            AppointmentDto appointment = await _appointmentApiService.GetAppointmentByIdAsync(id);

            RescheduleAppointmentDto rescheduleAppointmentDto = new RescheduleAppointmentDto
            {
                Id = appointment.Id,
                AppointmentDate = appointment.AppointmentDate,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime
            };

            return View(rescheduleAppointmentDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reschedule([FromBody]RescheduleAppointmentDto rescheduleAppointmentDto) 
        {
            if (!ModelState.IsValid)
            {
                return View(rescheduleAppointmentDto);
            }

            await _appointmentApiService.RescheduleAppointmentAsync(rescheduleAppointmentDto);

            TempData["Success"] = "Appointment rescheduled successfully";

            return RedirectToAction(nameof(Details), new { rescheduleAppointmentDto.Id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id) 
        {
            try
            {
                await _appointmentApiService.CancelAppointmentAsync(id);
                TempData["Success"] = "Appointment cancelled successfully";
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (Exception ex) 
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int id) 
        {
            try
            {
                await _appointmentApiService.CompleteAppointmentAsync(id);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
