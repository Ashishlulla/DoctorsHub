using DoctorsHub.Application.DTOs.Doctors;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationsController : ControllerBase
    {
        //private feilds
        private readonly ISpecializationService _specializationService;

        //Constructor
        public SpecializationsController(ISpecializationService specializationService) 
        {
            _specializationService = specializationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSpecializationAsync(int id)
        {
            List<SpecializationDTO> specializations = await _specializationService.GetAllSpecialization();

            return Ok(specializations);
        }
    }
}
