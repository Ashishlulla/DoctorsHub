using DoctorHub.Application.DTOs.Doctors;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        //Private feilds
        private readonly IDoctorService _doctorService;

        //Constructor
        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDoctorsAsync()
        {
            List<DoctorDto> doctors = await _doctorService.GetAllDoctorsAsync();

            return Ok(doctors);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorByIdAsync(int id) 
        {
            DoctorDto? doctor = await _doctorService.GetByIdAsync(id);

            return Ok(doctor);
        }
        [HttpPost]
        public async Task<IActionResult> CreateDoctorAsync([FromBody]CreateDoctorDto createDoctorDto) 
        {
            await _doctorService.CreateDoctorAsync(createDoctorDto);

            return Ok(new
            {
                Message = "Doctor created successfully",
                DoctorName = createDoctorDto.FullName
            });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDoctorAsync([FromBody]UpdateDoctorDto updateDoctorDto )
        {
            await _doctorService.UpdateDoctorAsync(updateDoctorDto.Id, updateDoctorDto);

            return Ok(new
            {
                Message = "Doctor updated successfully",
                DoctorName = updateDoctorDto.FullName
            });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctorAsync(int id) 
        {
            await _doctorService.DeleteDoctorAsync(id);

            return Ok("Doctor deleted successfully...");
        }
    }
}
