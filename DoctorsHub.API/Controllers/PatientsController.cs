using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.DTOs.common.DoctorsHub.Application.DTOs.Common;
using DoctorsHub.Application.DTOs.Patients;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        //private feilds 
        private readonly IPatientService _patientService;

        //constructor
        public PatientsController(IPatientService patientService) 
        {
            _patientService = patientService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllPatientsAsync() 
        {
            List<PatientDto> patients = await _patientService.GetAllPatientsAsync();

            return Ok(patients);
        }

        [HttpGet]
        public async Task<PagedResult<PatientDto>> GetAllPatientsAsync([FromQuery] PatientQueryParameters 
        patientQueryParameters) 
        {
            var data = await _patientService.GetAllPatientsAsync(patientQueryParameters);

            return data;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientByIdAsync(int id)
        {
            PatientDto patient = await _patientService.GetPatientByIdAsync(id);

            return Ok(patient);
        }

        [HttpPost]
        
        public async Task<IActionResult> CreatePatientAsync([FromBody]CreatePatientDto createPatientDto)
        {
            await _patientService.CreatePatientAsync(createPatientDto);

            return Ok(new
            {
                Message = "Patient created successfully.",
                PatientName = createPatientDto.FullName
            });
        }
        [HttpPut]
        public async Task<IActionResult> UpdatePatientAsync([FromBody]UpdatePatientDto updatePatientDto) 
        {
            await _patientService.UpdatePatientAsync(updatePatientDto);

            return Ok(new 
            {
                Message = "Patient Updated successfull",
                PatientName =updatePatientDto.FullName
            });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatientAsync(int id) 
        {
            await _patientService.DeletePatientAsync(id);

            return Ok($"Patient with  id = {id} deleted successfully");
        }

    }
}
