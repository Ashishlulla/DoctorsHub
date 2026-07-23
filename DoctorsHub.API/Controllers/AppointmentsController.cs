using DoctorsHub.Application.DTOs.Appoitments;
using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.DTOs.common.DoctorsHub.Application.DTOs.Common;
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
        public async Task<IActionResult> GetAppointmentsAsync([FromQuery]AppointmentQueryParameter appointmentQueryParameter) 
        {
           var(appointmens, TotalRecords) = await _appointmentService.GetAllAppointmentsAsync(appointmentQueryParameter);

            return Ok(new PagedResult<AppointmentDto>
            {
                Items = appointmens,
                PageSize = appointmentQueryParameter.PageSize,
                PageNumber = appointmentQueryParameter.PageNumber,
                TotalCount = TotalRecords,
                
            });
        }



        [HttpGet("all")]
        public async Task<IActionResult> GetAllAppointmentsAsync()
        {
            var appointments = await _appointmentService.GetAppointmentsAsync();

            return Ok(appointments);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentById(int id)
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

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Confirm(int id) 
        {
            await _appointmentService.ConfirmedAppointmentAsync(id);

            return Ok(new
            {
                Message = "Congratulation your appointment got confirmed",
                AppointmentId = id
            });
        }

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Cancel(int id) 
        {
            await _appointmentService.CancelAppointmentAsync(id);

            return Ok(new
            {
                Message = "Appointment cancelled successfully.",
                AppointmentId = id
            });
        }

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Complete(int id)
        {
            await _appointmentService.CompletedAppointmentAsync(id);

            return Ok(new
            {
                Message = "Appointment completed successfully.",
                AppointmentId = id
            });
        }

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Reschedule(RescheduleAppointmentDto rescheduleAppointmentDto)
        {
            await _appointmentService.RescheduleAppointmentAsync(rescheduleAppointmentDto); ;

            return Ok(new
            {
                Message = "Appointment reschedule successfully.",
                AppointmentId = rescheduleAppointmentDto.Id
            });
        }
    }
}
