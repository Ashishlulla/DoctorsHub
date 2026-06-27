using DoctorHub.Application.DTOs.Doctors;
using DoctorHub.Application.Interfaces;
using DoctorsHub.Application.DTOs.Doctors;
using DoctorsHub.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DoctorsHub.Web.Controllers
{
    [Route("[controller]")]
    public class DoctorController : Controller
    {
        //Private Feilds
        private readonly IDoctorService _doctorService;
        private readonly ISpecializationService _specializationService;

        //Constructor
        public DoctorController(IDoctorService doctorService, ISpecializationService specializationService) 
        {
            _doctorService = doctorService;
            _specializationService = specializationService;
        }


        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
        {
            var doctors = await _doctorService.GetAllDoctorsAsync();

            var pagedData = doctors
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new PagedResult<DoctorDto>
            {
                Items = pagedData,
                PageNumber = page,
                PageSize = pageSize,
                TotalRecords = doctors.Count
            };

            return View(result);
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Specializations = await _specializationService.GetAllSpecialization();
            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create(CreateDoctorDto createDoctorDto) 
        {
            if (!ModelState.IsValid)
            {
                return View(createDoctorDto);
            }

            await _doctorService.CreateDoctorAsync(createDoctorDto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Details(int id) 
        {
            var doctor = await _doctorService.GetByIdAsync(id);

            if (doctor == null)
            {
                return NotFound();                
            }

            return View(doctor);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<IActionResult> Edit(int id) 
        {
            var doctor = await _doctorService.GetDoctorForUpdateById(id);

            ViewBag.Specializations = await _specializationService.GetAllSpecialization();

            
            return View(doctor);
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var doctor =  await _doctorService.GetByIdAsync(id);
        

            return View(doctor);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _doctorService.DeleteDoctorAsync(id);

            TempData["Success"] = "Doctor Deleted Successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
