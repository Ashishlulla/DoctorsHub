using AutoMapper;
using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.DTOs.common.DoctorsHub.Application.DTOs.Common;
using DoctorsHub.Application.DTOs.Patients;
using DoctorsHub.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.Web.Controllers
{
    [Route("[controller]")]
    public class PatientsController : Controller
    {
        private readonly PatientApiService _patientApiService;
        private readonly IMapper _mapper;
        private readonly ILogger<PatientsController> _logger;

        public PatientsController(
            PatientApiService patientApiService,
            IMapper mapper,
            ILogger<PatientsController> logger)
        {
            _patientApiService = patientApiService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Index(
            [FromQuery] PatientQueryParameters patientQueryParameters)
        {
            _logger.LogInformation(
                "Patients index page requested. SearchBy: {SearchBy}, SearchString: {SearchString}, SortBy: {SortBy}, SortOrder: {SortOrder}",
                patientQueryParameters.searchBy,
                patientQueryParameters.searchString,
                patientQueryParameters.sortBy,
                patientQueryParameters.sortOrder);

            try
            {
                PagedResult<PatientDto> patients =
                    await _patientApiService
                        .GetAllPatientsAsync(patientQueryParameters);

                ViewBag.searchBy = patientQueryParameters.searchBy;
                ViewBag.searchString = patientQueryParameters.searchString;
                ViewBag.sortBy = patientQueryParameters.sortBy;
                ViewBag.sortOrder = patientQueryParameters.sortOrder;

                _logger.LogInformation(
                    "Patients loaded successfully.");

                return View(patients);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading patients.");

                throw;
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation(
                "Create patient page requested.");

            return View(new CreatePatientDto());
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create(
            CreatePatientDto createPatientDto)
        {
            _logger.LogInformation(
                "Create patient attempt started.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning(
                    "Create patient validation failed.");

                return View(createPatientDto);
            }

            try
            {
                await _patientApiService
                    .CreatePatientAsync(createPatientDto);

                _logger.LogInformation(
                    "Patient created successfully.");

                TempData["Success"] =
                    "Patient created successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while creating patient.");

                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return View(createPatientDto);
            }
        }

        [HttpGet]
        [Route("[action]/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation(
                "Patient details requested for PatientId: {PatientId}",
                id);

            try
            {
                PatientDto patient =
                    await _patientApiService
                        .GetPatientByIdAsync(id);

                if (patient == null)
                {
                    _logger.LogWarning(
                        "Patient not found. PatientId: {PatientId}",
                        id);

                    return NotFound();
                }

                _logger.LogInformation(
                    "Patient details loaded successfully for PatientId: {PatientId}",
                    id);

                return View(patient);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading patient details. PatientId: {PatientId}",
                    id);

                throw;
            }
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation(
                "Edit patient page requested for PatientId: {PatientId}",
                id);

            try
            {
                var patient =
                    await _patientApiService
                        .GetPatientByIdAsync(id);

                if (patient == null)
                {
                    _logger.LogWarning(
                        "Patient not found for editing. PatientId: {PatientId}",
                        id);

                    return NotFound();
                }

                var updatePatientDto =
                    _mapper.Map<UpdatePatientDto>(patient);

                _logger.LogInformation(
                    "Edit patient page loaded successfully for PatientId: {PatientId}",
                    id);

                return View(updatePatientDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading patient for editing. PatientId: {PatientId}",
                    id);

                throw;
            }
        }

        [HttpPost]
        [Route("[action]/{id}")]
        public async Task<IActionResult> Edit(
            int id,
            UpdatePatientDto updatePatientDto)
        {
            _logger.LogInformation(
                "Update patient attempt started for PatientId: {PatientId}",
                id);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning(
                    "Update patient validation failed for PatientId: {PatientId}",
                    id);

                return View(updatePatientDto);
            }

            try
            {
                await _patientApiService
                    .UpdatePatientAsync(updatePatientDto);

                _logger.LogInformation(
                    "Patient updated successfully for PatientId: {PatientId}",
                    id);

                TempData["Success"] =
                    "Patient updated successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while updating patient. PatientId: {PatientId}",
                    id);

                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return View(updatePatientDto);
            }
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation(
                "Delete patient page requested for PatientId: {PatientId}",
                id);

            try
            {
                var patient =
                    await _patientApiService
                        .GetPatientByIdAsync(id);

                if (patient == null)
                {
                    _logger.LogWarning(
                        "Patient not found for deletion. PatientId: {PatientId}",
                        id);

                    return NotFound();
                }

                return View(patient);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading patient for deletion. PatientId: {PatientId}",
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
                "Delete patient attempt started for PatientId: {PatientId}",
                id);

            try
            {
                await _patientApiService
                    .DeletePatientAsync(id);

                _logger.LogInformation(
                    "Patient deleted successfully for PatientId: {PatientId}",
                    id);

                TempData["Success"] =
                    "Patient Deleted Successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while deleting patient. PatientId: {PatientId}",
                    id);

                throw;
            }
        }
    }
}