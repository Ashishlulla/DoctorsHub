using DoctorsHub.Application.DTOs.Doctors;
using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.DTOs.common.DoctorsHub.Application.DTOs.Common;
using DoctorsHub.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoctorsHub.Web.Controllers
{
    [Route("[controller]")]
    public class DoctorsController : Controller
    {
        private readonly DoctorApiService _doctorApiService;
        private readonly SpecializationApiService _specializationApiService;
        private readonly DepartmentApiService _departmentApiService;
        private readonly ILogger<DoctorsController> _logger;

        public DoctorsController(
            DoctorApiService doctorApiService,
            SpecializationApiService specializationApiService,
            DepartmentApiService departmentApiService,
            ILogger<DoctorsController> logger)
        {
            _doctorApiService = doctorApiService;
            _specializationApiService = specializationApiService;
            _departmentApiService = departmentApiService;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Index(
            DoctorQueryParameters doctorQueryParameters)
        {
            _logger.LogInformation(
                "Doctors index page requested. SearchBy: {SearchBy}, SearchString: {SearchString}, SortBy: {SortBy}, SortOrder: {SortOrder}",
                doctorQueryParameters.searchBy,
                doctorQueryParameters.searchString,
                doctorQueryParameters.sortBy,
                doctorQueryParameters.sortOrder);

            try
            {
                PagedResult<DoctorDto> doctors =
                    await _doctorApiService
                        .GetAllDoctorsAsync(doctorQueryParameters);

                ViewBag.searchBy = doctorQueryParameters.searchBy;
                ViewBag.searchString = doctorQueryParameters.searchString;
                ViewBag.sortBy = doctorQueryParameters.sortBy;
                ViewBag.sortOrder = doctorQueryParameters.sortOrder;

                _logger.LogInformation(
                    "Doctors loaded successfully.");

                return View(doctors);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading doctors.");

                throw;
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation(
                "Create doctor page requested.");

            try
            {
                var specializations =
                    await _specializationApiService
                        .GetAllSpecializationsAsync();

                var departments =
                    await _departmentApiService
                        .GetDepartmentsAsync();

                ViewBag.Specializations =
                    new SelectList(
                        specializations,
                        "Id",
                        "Name");

                ViewBag.Departments =
                    new SelectList(
                        departments,
                        "Id",
                        "Name");

                return View(new CreateDoctorDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading the create doctor page.");

                throw;
            }
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create(
            CreateDoctorDto createDoctorDto)
        {
            _logger.LogInformation(
                "Create doctor attempt started for PersonalEmail: {PersonalEmail}",
                createDoctorDto.PersonalEmail);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning(
                    "Create doctor validation failed for PersonalEmail: {PersonalEmail}",
                    createDoctorDto.PersonalEmail);

                var specializations =
                    await _specializationApiService
                        .GetAllSpecializationsAsync();

                var departments =
                    await _departmentApiService
                        .GetDepartmentsAsync();

                ViewBag.Specializations =
                    new SelectList(
                        specializations,
                        "Id",
                        "Name");

                ViewBag.Departments =
                    new SelectList(
                        departments,
                        "Id",
                        "Name");

                return View(createDoctorDto);
            }

            try
            {
                await _doctorApiService
                    .CreateDoctorAsync(createDoctorDto);

                _logger.LogInformation(
                    "Doctor created successfully for PersonalEmail: {PersonalEmail}",
                    createDoctorDto.PersonalEmail);

                TempData["Success"] =
                    "Doctor created successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while creating doctor for PersonalEmail: {PersonalEmail}",
                    createDoctorDto.PersonalEmail);

                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                var specializations =
                    await _specializationApiService
                        .GetAllSpecializationsAsync();

                var departments =
                    await _departmentApiService
                        .GetDepartmentsAsync();

                ViewBag.Specializations =
                    new SelectList(
                        specializations,
                        "Id",
                        "Name");

                ViewBag.Departments =
                    new SelectList(
                        departments,
                        "Id",
                        "Name");

                return View(createDoctorDto);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation(
                "Doctor details requested for DoctorId: {DoctorId}",
                id);

            try
            {
                DoctorDto doctor =
                    await _doctorApiService
                        .GetDoctorByIdAsync(id);

                if (doctor == null)
                {
                    _logger.LogWarning(
                        "Doctor not found. DoctorId: {DoctorId}",
                        id);

                    return NotFound();
                }

                return View(doctor);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading doctor details. DoctorId: {DoctorId}",
                    id);

                throw;
            }
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation(
                "Edit doctor page requested for DoctorId: {DoctorId}",
                id);

            try
            {
                var doctor =
                    await _doctorApiService
                        .GetDoctorByIdAsync(id);

                if (doctor == null)
                {
                    _logger.LogWarning(
                        "Doctor not found for editing. DoctorId: {DoctorId}",
                        id);

                    return NotFound();
                }

                var specializations =
                    await _specializationApiService
                        .GetAllSpecializationsAsync();

                var departments =
                    await _departmentApiService
                        .GetDepartmentsAsync();

                ViewBag.Specializations =
                    new SelectList(
                        specializations,
                        "Id",
                        "Name");

                ViewBag.Departments =
                    new SelectList(
                        departments,
                        "Id",
                        "Name");

                var model = new UpdateDoctorDto
                {
                    Id = doctor.Id,
                    FullName = doctor.FullName,
                    PersonalEmail = doctor.PersonalEmail,
                    VisitDays = doctor.VisitDays,
                    PhoneNumber = doctor.PhoneNumber,
                    Qualification = doctor.Qualification,
                    SpecializationId = doctor.SpecializationId,
                    ConsultationFee = doctor.ConsultationFee,
                    ExperienceYears = doctor.ExperienceYears,
                    DepartmentIds = doctor.DepartmentIds,
                    About = doctor.About
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading doctor for editing. DoctorId: {DoctorId}",
                    id);

                throw;
            }
        }

        [HttpPost]
        [Route("[action]/{id}")]
        public async Task<IActionResult> Edit(
            int id,
            UpdateDoctorDto updateDoctorDto)
        {
            _logger.LogInformation(
                "Update doctor attempt started for DoctorId: {DoctorId}, PersonalEmail: {PersonalEmail}",
                id,
                updateDoctorDto.PersonalEmail);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning(
                    "Update doctor validation failed for DoctorId: {DoctorId}, PersonalEmail: {PersonalEmail}",
                    id,
                    updateDoctorDto.PersonalEmail);

                var specializations =
                    await _specializationApiService
                        .GetAllSpecializationsAsync();

                var departments =
                    await _departmentApiService
                        .GetDepartmentsAsync();

                ViewBag.Specializations =
                    new SelectList(
                        specializations,
                        "Id",
                        "Name");

                ViewBag.Departments =
                    new SelectList(
                        departments,
                        "Id",
                        "Name");

                return View(updateDoctorDto);
            }

            try
            {
                await _doctorApiService
                    .UpdateDoctorAsync(updateDoctorDto);

                _logger.LogInformation(
                    "Doctor updated successfully. DoctorId: {DoctorId}, PersonalEmail: {PersonalEmail}",
                    id,
                    updateDoctorDto.PersonalEmail);

                TempData["Success"] =
                    "Doctor updated successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while updating doctor. DoctorId: {DoctorId}, PersonalEmail: {PersonalEmail}",
                    id,
                    updateDoctorDto.PersonalEmail);

                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return View(updateDoctorDto);
            }
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation(
                "Delete doctor page requested for DoctorId: {DoctorId}",
                id);

            try
            {
                var doctor =
                    await _doctorApiService
                        .GetDoctorByIdAsync(id);

                if (doctor == null)
                {
                    _logger.LogWarning(
                        "Doctor not found for deletion. DoctorId: {DoctorId}",
                        id);

                    return NotFound();
                }

                return View(doctor);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading doctor for deletion. DoctorId: {DoctorId}",
                    id);

                throw;
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation(
                "Delete doctor attempt started for DoctorId: {DoctorId}",
                id);

            try
            {
                await _doctorApiService
                    .DeleteDoctorAsync(id);

                _logger.LogInformation(
                    "Doctor deleted successfully. DoctorId: {DoctorId}",
                    id);

                TempData["Success"] =
                    "Doctor Deleted Successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while deleting doctor. DoctorId: {DoctorId}",
                    id);

                throw;
            }
        }
    }
}