using DoctorsHub.Application.DTOs.Appoitments;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        //private feilds
        private readonly IAppointmentService _appointmentService;

        //Constructor
        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointments()
        {
            var appointments = await _appointmentService.GetAppointmentsAsync();

            return Ok(appointments);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointments(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);

            return Ok(appointment);

        }

        [HttpPost]

        public async Task<IActionResult> CreateAppointmentAsync([FromBody]CreateAppointmentDto createAppointmentDto)
        {
            await _appointmentService.CreateAppointmentAsync(createAppointmentDto);

            return Ok(new
            {
                Message = "Appointment created successfully",
                AppointmentDetails = createAppointmentDto
            });
        }

        [HttpPut]

        public async Task<IActionResult> UpdateAppointmentAsync([FromBody]UpdateAppointmentDto updateAppointmentDto)
        {
            await _appointmentService.UpdateAppointmentAsync(updateAppointmentDto);

            return Ok(new
            {
                Message = "Appointment updated successfully",
                UpdatdAppointmentDetails = updateAppointmentDto
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointmentAsync(int id)
        {
            await _appointmentService.DeleteAppointmentAsync(id);

            return Ok(new
            {
                Message = "Appointment deleted successfully.",
                AppointmentId = id
            });
        }
    }
}
