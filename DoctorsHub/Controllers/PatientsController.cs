using DoctorsHub.Application.DTOs.Patients;
using DoctorsHub.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.Web.Controllers
{
    [Route("[controller]")]
    public class PatientsController : Controller
    {
        //private feilds
        private readonly IPatientService _patientService;

        //constructor
        public PatientsController(IPatientService patientService) 
        {
            _patientService = patientService;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Index()
        {
            var patients = await _patientService.GetAllPatientAsync();

            return View(patients);
        }
        [HttpGet]
        [Route("[action]")]
        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        [Route("[action]")]
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
    }
}
