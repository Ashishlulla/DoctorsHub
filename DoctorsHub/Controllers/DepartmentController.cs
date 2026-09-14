using DoctorsHub.Application.DTOs.Departments;
using DoctorsHub.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.Web.Controllers
{
    [Route("[controller]")]
    public class DepartmentController : Controller
    {
        private readonly DepartmentApiService _departmentApiService;
        private readonly ILogger<DepartmentController> _logger;

        public DepartmentController(
            DepartmentApiService departmentApiService,
            ILogger<DepartmentController> logger)
        {
            _departmentApiService = departmentApiService;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation(
                "Department index page requested.");

            try
            {
                var departments =
                    await _departmentApiService.GetDepartmentsAsync();

                _logger.LogInformation(
                    "Departments loaded successfully.");

                return View(departments);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading departments.");

                throw;
            }
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Create()
        {
            _logger.LogInformation(
                "Create department page requested.");

            return View(new CreateDepartmentDto());
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create(
            CreateDepartmentDto createDepartmentDto)
        {
            _logger.LogInformation(
                "Create department attempt started.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning(
                    "Create department validation failed.");

                return View(createDepartmentDto);
            }

            try
            {
                await _departmentApiService
                    .CreateDepartmentAsync(createDepartmentDto);

                _logger.LogInformation(
                    "Department created successfully.");

                TempData["SuccessMessage"] =
                    "Department created successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while creating department.");

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
            _logger.LogInformation(
                "Edit department page requested for DepartmentId: {DepartmentId}",
                id);

            try
            {
                var department =
                    await _departmentApiService.GetDepartmentByIdAsync(id);

                if (department == null)
                {
                    _logger.LogWarning(
                        "Department not found for editing. DepartmentId: {DepartmentId}",
                        id);

                    return NotFound();
                }

                var updateDepartmentDto = new UpdateDepartmentDto
                {
                    Id = department.Id,
                    Name = department.Name,
                    Description = department.Description
                };

                _logger.LogInformation(
                    "Edit department page loaded successfully for DepartmentId: {DepartmentId}",
                    id);

                return View(updateDepartmentDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading department for editing. DepartmentId: {DepartmentId}",
                    id);

                throw;
            }
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Edit(
            UpdateDepartmentDto updateDepartmentDto)
        {
            _logger.LogInformation(
                "Update department attempt started for DepartmentId: {DepartmentId}",
                updateDepartmentDto.Id);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning(
                    "Update department validation failed for DepartmentId: {DepartmentId}",
                    updateDepartmentDto.Id);

                return View(updateDepartmentDto);
            }

            try
            {
                await _departmentApiService
                    .UpdateDepartmentAsync(updateDepartmentDto);

                _logger.LogInformation(
                    "Department updated successfully for DepartmentId: {DepartmentId}",
                    updateDepartmentDto.Id);

                TempData["SuccessMessage"] =
                    "Department updated successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while updating department. DepartmentId: {DepartmentId}",
                    updateDepartmentDto.Id);

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
            _logger.LogInformation(
                "Department details requested for DepartmentId: {DepartmentId}",
                id);

            try
            {
                var department =
                    await _departmentApiService
                        .GetDepartmentDetailsAsync(id);

                if (department == null)
                {
                    _logger.LogWarning(
                        "Department not found for details. DepartmentId: {DepartmentId}",
                        id);

                    return NotFound();
                }

                _logger.LogInformation(
                    "Department details loaded successfully for DepartmentId: {DepartmentId}",
                    id);

                return View(department);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading department details. DepartmentId: {DepartmentId}",
                    id);

                throw;
            }
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation(
                "Delete department page requested for DepartmentId: {DepartmentId}",
                id);

            try
            {
                var department =
                    await _departmentApiService.GetDepartmentByIdAsync(id);

                if (department == null)
                {
                    _logger.LogWarning(
                        "Department not found for deletion. DepartmentId: {DepartmentId}",
                        id);

                    return NotFound();
                }

                return View(department);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading department for deletion. DepartmentId: {DepartmentId}",
                    id);

                throw;
            }
        }

        [HttpPost]
        [Route("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation(
                "Delete department attempt started for DepartmentId: {DepartmentId}",
                id);

            try
            {
                await _departmentApiService.DeleteDepartmentAsync(id);

                _logger.LogInformation(
                    "Department deleted successfully for DepartmentId: {DepartmentId}",
                    id);

                TempData["SuccessMessage"] =
                    "Department deleted successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while deleting department. DepartmentId: {DepartmentId}",
                    id);

                TempData["ErrorMessage"] =
                    "Unable to delete the department.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}