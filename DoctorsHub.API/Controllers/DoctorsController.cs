using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.DTOs.common.DoctorsHub.Application.DTOs.Common;
using DoctorsHub.Application.DTOs.Doctors;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Receptionist,Doctor")]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetDoctorsAsync(
            [FromQuery] DoctorQueryParameters doctorQueryParameters)
        {
            var (doctors, totalRecords) =
                await _doctorService.GetAllDoctorsAsync(doctorQueryParameters);

            return Ok(new PagedResult<DoctorDto>
            {
                Items = doctors,
                PageNumber = doctorQueryParameters.PageNumber,
                PageSize = doctorQueryParameters.PageSize,
                TotalCount = totalRecords
            });
        }

       
        [HttpGet("all")]
        public async Task<IActionResult> GetDoctorsAsync()
        {
            List<DoctorDto> doctors =
                await _doctorService.GetAllDoctorsAsync();

            return Ok(doctors);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorByIdAsync(int id)
        {
            DoctorDto? doctor =
                await _doctorService.GetByIdAsync(id);

            if (doctor == null)
            {
                return NotFound(new
                {
                    Message = "Doctor not found"
                });
            }

            return Ok(doctor);
        }

       
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateDoctorAsync(
            [FromBody] CreateDoctorDto createDoctorDto)
        {
            await _doctorService.CreateDoctorAsync(createDoctorDto);

            return Ok(new
            {
                Message = "Doctor created successfully",
                DoctorName = createDoctorDto.FullName
            });
        }

       
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDoctorAsync(
            [FromBody] UpdateDoctorDto updateDoctorDto)
        {
            await _doctorService.UpdateDoctorAsync(
                updateDoctorDto.Id,
                updateDoctorDto);

            return Ok(new
            {
                Message = "Doctor updated successfully",
                DoctorName = updateDoctorDto.FullName
            });
        }

       
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDoctorAsync(int id)
        {
            await _doctorService.DeleteDoctorAsync(id);

            return Ok(new
            {
                Message = "Doctor deleted successfully"
            });
        }
    }
}