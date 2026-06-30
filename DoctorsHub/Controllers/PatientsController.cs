using AutoMapper;
using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.DTOs.Patients;
using DoctorsHub.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.Web.Controllers
{
    //[Route("[controller]")]
    public class PatientsController : Controller
    {
        //private feilds
        private readonly IPatientService _patientService;
        private readonly IMapper _mapper;

        //constructor
        public PatientsController(IPatientService patientService, IMapper mapper) 
        {
            _patientService = patientService;
            _mapper = mapper;
        }

        [HttpGet]
        //[Route("[action]")]
        public async Task<IActionResult> Index(PatientQueryParameters patientQueryParameters)
        {
            var patients = await _patientService.GetAllPatientAsync(patientQueryParameters);

            return View(patients);
        }
        [HttpGet]
        //[Route("[action]")]
        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        //[Route("[action]")]
        public async Task<IActionResult> Create(CreatePatientDto createPatientDto) 
        {
            if (!ModelState.IsValid)
            {
                return View(createPatientDto);
            }

            await _patientService.CreatePatientAsync(createPatientDto);

            TempData["Success"] = "Patient created Successfully..";
            return RedirectToAction(nameof(PatientsController.Index));
        }

        [HttpGet]
        
        public async Task<IActionResult> Details(int id) 
        {
            var patient = await _patientService.GetPatientByIdAsync(id);

            return View(patient);
        }

        [HttpGet]
        
        public async Task<IActionResult> Edit(int id)
        {
            var patient = await _patientService.GetPatientForUpdateByIdAsync(id);

            //return View(_mapper.Map<UpdatePatientDto>(patient));
            return View(patient);
        }

        [HttpPost]
        
        public async Task<IActionResult> Edit(UpdatePatientDto updatePatientDto)
        {
            if (!ModelState.IsValid)
            {
                return View(updatePatientDto);
            }

            await _patientService.UpdatePatientAsync(updatePatientDto);

            TempData["Success"] = "Patient created Successfully..";
            return RedirectToAction(nameof(PatientsController.Index));
        }

        
        public async Task<IActionResult> Delete(int id)
        {
            var doctor = await _patientService.GetPatientByIdAsync(id);


            return View(doctor);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _patientService.DeletePatientAsync(id);

            TempData["Success"] = "Patient Deleted Successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
