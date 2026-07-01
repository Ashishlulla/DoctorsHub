using DoctorsHub.Application.DTOs.Appoitments;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.Web.Controllers
{
    public class AppointmentsController : Controller
    {
        //private feilds
        private readonly IAppointmentService _appointmentService;

        //constructor
        public AppointmentsController(IAppointmentService appointmentService) 
        {
            _appointmentService = appointmentService;
        }

        //GET: Index Action Method
        public async Task<IActionResult> Index()
        {
            List <AppointmentDto> appointments = await _appointmentService.GetAllAppointmentsAsync();

           return View(appointments);
        }

        //GET: Create Action Method
        public IActionResult Create() 
        {
            CreateAppointmentDto model = new CreateAppointmentDto();

            return View(model);
        }

        //POST: Create Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAppointmentDto createAppointmentDto) 
        {
            if (!ModelState.IsValid)
            {
                return View(createAppointmentDto);
            }

            await _appointmentService.CreateAppointmentAsync(createAppointmentDto);

            TempData["Success"] = "Appointment created successfully..";
            return RedirectToAction(nameof(AppointmentsController.Index));
        }

        //GET: Edit Action Method
        public async Task<IActionResult> Edit(int id) 
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            return View(appointment);
        }

        //POST: Edit Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateAppointmentDto updateAppointmentDto) 
        {
            if (!ModelState.IsValid)
            {
                return View(updateAppointmentDto);   
            }

            await _appointmentService.UpdateAppointmentAsync(updateAppointmentDto);

            TempData["Success"] = "Appointment updated successfully..";

            return RedirectToAction(nameof(Index));
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
    }
}
