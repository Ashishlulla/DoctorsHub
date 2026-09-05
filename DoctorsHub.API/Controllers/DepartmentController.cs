using DoctorsHub.Application.DTOs.Departments;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.API.Controllers
{
    [Authorize(Roles = "Admin, Receptionist,Doctor")]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        //Private Feilds 
        private readonly IDepartmentService _departmentService;

        //Constructor
        public DepartmentController(IDepartmentService departmentService) 
        {
            _departmentService = departmentService;
        }


        [HttpGet("All")]
        [Authorize(Roles ="Admin, Receptionist,Doctor")]
        public async Task<IActionResult> GetDepartments() 
        {
            var departments = await _departmentService.GetDepartmentsAsync();

            return Ok(departments);
        }

        [HttpGet("{id:int}")]
        
        public async Task<IActionResult> GetDepartmentById(int id) 
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);

            return Ok(department);
        }

        [HttpGet("{id:int}/Details")]
        public async Task<IActionResult> GetDepartmentDetails(int id)
        {
            var department =
                await _departmentService.GetDepartmentDetailsAsync(id);

            return Ok(department);
        }

        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateDepartment([FromBody]CreateDepartmentDto createDepartmentDto) 
        {
            var department = await _departmentService.AddDepartmentAsync(createDepartmentDto);

            return Ok(department);
        }

        [HttpPut("Update")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> UpdateDepartment([FromBody] UpdateDepartmentDto updateDepartmentDto) 
        {
            var updatedDepartment = await _departmentService.UpdateDepartmentAsync(updateDepartmentDto);
            return Ok(updatedDepartment);
        }



        [HttpDelete("Delete/{id:int}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteDepartmentById(int id) 
        {
            await _departmentService.DeleteDepartmentAsync(id);

            return Ok("Department deleted successfully.");
        }
    }
}
