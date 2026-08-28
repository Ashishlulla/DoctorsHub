using DoctorsHub.Application.DTOs.Departments;
using DoctorsHub.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.Web.Controllers
{
    [Route("[controller]")]
    public class DepartmentController : Controller
    {
        private readonly DepartmentApiService _departmentApiService;

        public DepartmentController(
            DepartmentApiService departmentApiService)
        {
            _departmentApiService = departmentApiService;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Index()
        {
            var departments =
                await _departmentApiService.GetDepartmentsAsync();

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
        public async Task<IActionResult> Create(
            CreateDepartmentDto createDepartmentDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createDepartmentDto);
            }

            try
            {
                await _departmentApiService
                    .CreateDepartmentAsync(createDepartmentDto);

                TempData["SuccessMessage"] =
                    "Department created successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to create the department.");

                return View(createDepartmentDto);
            }
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var department =
                await _departmentApiService.GetDepartmentByIdAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            var updateDepartmentDto = new UpdateDepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description
            };

            return View(updateDepartmentDto);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Edit(
            UpdateDepartmentDto updateDepartmentDto)
        {
            if (!ModelState.IsValid)
            {
                return View(updateDepartmentDto);
            }

            try
            {
                await _departmentApiService
                    .UpdateDepartmentAsync(updateDepartmentDto);

                TempData["SuccessMessage"] =
                    "Department updated successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to update the department.");

                return View(updateDepartmentDto);
            }
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var department =
                await _departmentApiService.GetDepartmentDetailsAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var department =
                await _departmentApiService.GetDepartmentByIdAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        [HttpPost]
        [Route("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _departmentApiService.DeleteDepartmentAsync(id);

                TempData["SuccessMessage"] =
                    "Department deleted successfully.";
            }
            catch
            {
                TempData["ErrorMessage"] =
                    "Unable to delete the department.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}