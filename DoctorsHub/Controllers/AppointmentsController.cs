using DoctorsHub.Application.DTOs.Appoitments;
using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.DTOs.common.DoctorsHub.Application.DTOs.Common;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.Web.Controllers
{
    public class AppointmentsController : Controller
    {
        //private feilds
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;

        //constructor
        public AppointmentsController(IAppointmentService appointmentService, IPatientService patientService, IDoctorService doctorService)
        {
            _appointmentService = appointmentService;
            _patientService = patientService;
            _doctorService = doctorService;
        }

        //GET: Index Action Method
        public async Task<IActionResult> Index(AppointmentQueryParameter appointmentqueryParameters)
        {
            var (appointments, TotalRecords) = await _appointmentService.GetAllAppointmentsAsync(appointmentqueryParameters);
            PagedResult<AppointmentDto> result = new PagedResult<AppointmentDto> 
            {
                Items = appointments,
                PageSize = appointmentqueryParameters.PageSize,
                PageNumber = appointmentqueryParameters.PageNumber,
                TotalCount = TotalRecords,
                TotalPages = (int)Math.Ceiling((double) TotalRecords/ appointmentqueryParameters.PageSize)
            };
            ViewBag.QuerParameters = appointmentqueryParameters;

            ViewBag.Count = appointments.Count;
            ViewBag.Total = TotalRecords;
            return View(result);
        }
        

        //GET: Create Action Method
        public async Task<IActionResult> Create()
        {
            
            ViewBag.Patients = await _patientService.GetAllPatientsAsync();
            ViewBag.Doctors = await _doctorService.GetAllDoctorsAsync();

            return View(new CreateAppointmentDto());
        }

        //POST: Create Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAppointmentDto createAppointmentDto) 
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Patients = await _patientService.GetAllPatientsAsync();
                ViewBag.Doctors = await _doctorService.GetAllDoctorsAsync();
                return View(createAppointmentDto);
            }
            try
            {
                await _appointmentService.CreateAppointmentAsync(createAppointmentDto);

                TempData["Success"] = "Appointment created successfully..";
                return RedirectToAction(nameof(AppointmentsController.Index));
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                ViewBag.Patients = await _patientService.GetAllPatientsAsync();
                ViewBag.Doctors = await _doctorService.GetAllDoctorsAsync();
                return View(createAppointmentDto);

            }
           
        }

        //GET: Edit Action Method
        public async Task<IActionResult> Edit(int id) 
        {
            var appointment = await _appointmentService.GetAppointmentForUpdateByIdAsync(id);
            ViewBag.Patients = await _patientService.GetAllPatientsAsync();
            ViewBag.Doctors = await _doctorService.GetAllDoctorsAsync();
            return View(appointment);
        }

        //POST: Edit Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateAppointmentDto updateAppointmentDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Patients = await _patientService.GetAllPatientsAsync();
                ViewBag.Doctors = await _doctorService.GetAllDoctorsAsync();

                return View(updateAppointmentDto);
            }
            try { 
                await _appointmentService.UpdateAppointmentAsync(updateAppointmentDto);

                TempData["Success"] = "Appointment updated successfully..";

                return RedirectToAction(nameof(Index)); 
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                ViewBag.Patients = await _patientService.GetAllPatientsAsync();
                ViewBag.Doctors = await _doctorService.GetAllDoctorsAsync();
                return View(updateAppointmentDto);

            }
        }
        //GET: Details Action Method
        public async Task<IActionResult> Details(int id) 
        {
            AppointmentDetailsDto appointmentDetails = await _appointmentService.GetAppointmentForDetailsByIdAsync(id);
            return View(appointmentDetails);
        }

        //GET: Delete Action Method
        public async Task<IActionResult> Delete(int id) 
        {
            AppointmentDto appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            return View(appointment);
        }

        //POST: DeleteConfirmed Action Method
        [HttpPost]
        [ActionName(nameof(AppointmentsController.Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _appointmentService.DeleteAppointmentAsync(id);

            TempData["Success"] = "Appointment deleted successfully.";
            return RedirectToAction(nameof(Index));


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(int id) 
        {
            await _appointmentService.ConfirmedAppointmentAsync(id);

            TempData["Success"] = "Appointment Confirmed Successfully.";

            return RedirectToAction(nameof(Details), new { id });
        }

        public async Task<IActionResult> Reschedule(int id) 
        {
            AppointmentDto appointment = await _appointmentService.GetAppointmentByIdAsync(id);

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
        public async Task<IActionResult> Reschedule(RescheduleAppointmentDto rescheduleAppointmentDto) 
        {
            if (!ModelState.IsValid)
            {
                return View(rescheduleAppointmentDto);
            }

            await _appointmentService.RescheduleAppointmentAsync(rescheduleAppointmentDto);

            TempData["Success"] = "Appointment rescheduled successfully";

            return RedirectToAction(nameof(Details), new { rescheduleAppointmentDto.Id});
        }
    }
}
