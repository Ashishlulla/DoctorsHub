using DoctorsHub.Application.DTOs.Departments;

using DoctorsHub.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.Web.Controllers
{
    [Route("[controller]")]
    public class DepartmentController : Controller
    {

        //Private Feilds
        private readonly DepartmentApiService _departmentApiService;

        //Constructor
        public DepartmentController(DepartmentApiService departmentApiService) 
        {
            _departmentApiService = departmentApiService;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Index()
        {
            var departments = await  _departmentApiService.GetDepartmentsAsync();
            return View(departments);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Create() 
        {
            return View(new CreateDepartmentDto());
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create(CreateDepartmentDto createDepartmentDto) 
        {
            if (!ModelState.IsValid)
            {
                return View(createDepartmentDto);
            }

            try
            {
                await _departmentApiService.CreateDepartmentAsync(createDepartmentDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) 
            {
                return View(createDepartmentDto);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Edit(int id) 
        {
            var department = await _departmentApiService.GetDepartmentByIdAsync(id);

            UpdateDepartmentDto updateDepartmentDto = new UpdateDepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description
            };
            return View(updateDepartmentDto);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Edit(UpdateDepartmentDto updateDepartmentDto) 
        {
            if (!ModelState.IsValid)
            {
                return View(updateDepartmentDto);
            }

            try
            {
                await _departmentApiService.UpdateDepartmentAsync(updateDepartmentDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) 
            {
                return View(updateDepartmentDto);
            }
        }
    }
}
