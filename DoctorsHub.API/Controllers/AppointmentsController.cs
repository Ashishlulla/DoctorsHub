using DoctorsHub.Application.DTOs.Appoitments;
using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.DTOs.common.DoctorsHub.Application.DTOs.Common;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        // Admin + Doctor + Receptionist
        [HttpGet]
        [Authorize(Roles = "Admin,Doctor,Receptionist")]
        public async Task<IActionResult> GetAppointmentsAsync(
            [FromQuery] AppointmentQueryParameter appointmentQueryParameter)
        {
            var (appointments, totalRecords) =
                await _appointmentService.GetAllAppointmentsAsync(
                    appointmentQueryParameter);

            return Ok(new PagedResult<AppointmentDto>
            {
                Items = appointments,
                PageSize = appointmentQueryParameter.PageSize,
                PageNumber = appointmentQueryParameter.PageNumber,
                TotalCount = totalRecords
            });
        }

        // Admin + Doctor + Receptionist
        [HttpGet("all")]
        [Authorize(Roles = "Admin,Doctor,Receptionist")]
        public async Task<IActionResult> GetAllAppointmentsAsync()
        {
            var appointments =
                await _appointmentService.GetAppointmentsAsync();

            return Ok(appointments);
        }

        // Admin + Doctor + Receptionist
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Doctor,Receptionist")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            var appointment =
                await _appointmentService.GetAppointmentByIdAsync(id);

            return Ok(appointment);
        }

        // Admin + Doctor + Receptionist
        [HttpPost]
        [Authorize(Roles = "Admin,Doctor,Receptionist")]
        public async Task<IActionResult> CreateAppointmentAsync(
            [FromBody] CreateAppointmentDto createAppointmentDto)
        {
            await _appointmentService.CreateAppointmentAsync(
                createAppointmentDto);

            return Ok(new
            {
                Message = "Appointment created successfully",
                AppointmentDetails = createAppointmentDto
            });
        }

        // Admin + Doctor + Receptionist
        [HttpPut]
        [Authorize(Roles = "Admin,Doctor,Receptionist")]
        public async Task<IActionResult> UpdateAppointmentAsync(
            [FromBody] UpdateAppointmentDto updateAppointmentDto)
        {
            await _appointmentService.UpdateAppointmentAsync(
                updateAppointmentDto);

            return Ok(new
            {
                Message = "Appointment updated successfully",
                UpdatedAppointmentDetails = updateAppointmentDto
            });
        }

        // Admin ONLY
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAppointmentAsync(int id)
        {
            await _appointmentService.DeleteAppointmentAsync(id);

            return Ok(new
            {
                Message = "Appointment deleted successfully.",
                AppointmentId = id
            });
        }

        // Admin + Doctor + Receptionist
        [HttpPut("[action]/{id}")]
        [Authorize(Roles = "Admin,Doctor,Receptionist")]
        public async Task<IActionResult> Confirm(int id)
        {
            await _appointmentService.ConfirmedAppointmentAsync(id);

            return Ok(new
            {
                Message = "Congratulation your appointment got confirmed",
                AppointmentId = id
            });
        }

        // Admin + Doctor + Receptionist
        [HttpPut("[action]/{id}")]
        [Authorize(Roles = "Admin,Doctor,Receptionist")]
        public async Task<IActionResult> Cancel(int id)
        {
            await _appointmentService.CancelAppointmentAsync(id);

            return Ok(new
            {
                Message = "Appointment cancelled successfully.",
                AppointmentId = id
            });
        }

        // Admin + Doctor + Receptionist
        [HttpPut("[action]/{id}")]
        [Authorize(Roles = "Admin,Doctor,Receptionist")]
        public async Task<IActionResult> Complete(int id)
        {
            await _appointmentService.CompletedAppointmentAsync(id);

            return Ok(new
            {
                Message = "Appointment completed successfully.",
                AppointmentId = id
            });
        }

        // Admin + Doctor + Receptionist
        [HttpPut("[action]")]
        [Authorize(Roles = "Admin,Doctor,Receptionist")]
        public async Task<IActionResult> Reschedule(
            RescheduleAppointmentDto rescheduleAppointmentDto)
        {
            await _appointmentService.RescheduleAppointmentAsync(
                rescheduleAppointmentDto);

            return Ok(new
            {
                Message = "Appointment reschedule successfully.",
                AppointmentId = rescheduleAppointmentDto.Id
            });
        }
    }
}