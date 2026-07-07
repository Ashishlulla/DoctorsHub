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
        //Private Feilds
        private readonly PatientApiService _patientApiService;
        private readonly IMapper _mapper;

        //Constructor
        public PatientsController(PatientApiService patientApiService, IMapper mapper)
        {
            _patientApiService = patientApiService;
            _mapper = mapper;
        }


        [HttpGet]
        [Route("[action]")]
        
        public async Task<IActionResult> Index([FromQuery]PatientQueryParameters patientQueryParameters)
        {
            PagedResult<PatientDto> patients = await _patientApiService.GetAllPatientsAsync(patientQueryParameters);

            
            ViewBag.searchBy = patientQueryParameters.searchBy;
            ViewBag.searchString = patientQueryParameters.searchString;
            ViewBag.sortBy = patientQueryParameters.sortBy;
            ViewBag.sortOrder = patientQueryParameters.sortOrder;

            return View(patients);
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Create()
        {
            return View(new CreatePatientDto());
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create(CreatePatientDto createPatientDto)
        {
            if (!ModelState.IsValid)
            {
                
                return View(createPatientDto);
            }

            await _patientApiService.CreatePatientAsync(createPatientDto);

            TempData["Success"] = "Patient created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Details(int id)
        {
            PatientDto patient= await _patientApiService.GetPatientByIdAsync(id);

            return View(patient);
        }

        [HttpGet]
        [Route("[action]/{id}")]

        public async Task<IActionResult> Edit(int id)
        {
            var patient = await _patientApiService.GetPatientByIdAsync(id);

            if (patient == null)
                return NotFound();

           

            return View(_mapper.Map<UpdatePatientDto>(patient));
        }

        [HttpPost]
        [Route("[action]/{id}")]
        public async Task<IActionResult> Edit(int id, UpdatePatientDto updatePatientDto)
        {
            if (!ModelState.IsValid)
            {
                return View(updatePatientDto);
            }

            await _patientApiService.UpdatePatientAsync(updatePatientDto);
            
            TempData["Success"] = "Patient updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _patientApiService.GetPatientByIdAsync(id);


            return View(patient);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _patientApiService.DeletePatientAsync(id);

            TempData["Success"] = "Patient Deleted Successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
