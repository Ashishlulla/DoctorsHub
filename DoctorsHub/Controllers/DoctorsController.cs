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
        //Private Feilds
        private readonly DoctorApiService _doctorApiService;
        private readonly SpecializationApiService _specializationApiService;

        private readonly DepartmentApiService _departmentApiService;
        //Constructor
        public DoctorsController(DoctorApiService doctorApiService, SpecializationApiService specializationApiService, DepartmentApiService departmentApiService) 
        {
            _doctorApiService = doctorApiService;
            _specializationApiService = specializationApiService;
            _departmentApiService = departmentApiService;
        }


        [HttpGet]
        [Route("[action]")]
        //[Route("/")]
        
        public async Task<IActionResult> Index(DoctorQueryParameters doctorQueryParameters)
        {
            PagedResult<DoctorDto> doctors = await _doctorApiService.GetAllDoctorsAsync(doctorQueryParameters);

            

            ViewBag.searchBy = doctorQueryParameters.searchBy;
            ViewBag.searchString = doctorQueryParameters.searchString;
            ViewBag.sortBy = doctorQueryParameters.sortBy;
            ViewBag.sortOrder = doctorQueryParameters.sortOrder;

            return View(doctors);
        }
        
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Create()
        {

            var specializations = await _specializationApiService.GetAllSpecializationsAsync();
            var departments = await _departmentApiService.GetDepartmentsAsync();

            //ViewBag for Dropdowns
            ViewBag.Specializations = new SelectList(
                specializations, "Id", "Name"
                );

            ViewBag.Departments = new SelectList(departments, "Id", "Name");


            return View(new CreateDoctorDto());
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create(CreateDoctorDto createDoctorDto)
        {
            if (!ModelState.IsValid)
            {
                var specializations = await _specializationApiService.GetAllSpecializationsAsync();
                var departments = await _departmentApiService.GetDepartmentsAsync();

                //ViewBag for Dropdowns
                ViewBag.Specializations = new SelectList(specializations,"Id","Name");

                ViewBag.Departments = new SelectList(departments, "Id", "Name");
                return View(createDoctorDto);
            }

            await _doctorApiService.CreateDoctorAsync(createDoctorDto);

            TempData["Success"] = "Doctor created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Details(int id)
        {
            DoctorDto doctor = await _doctorApiService.GetDoctorByIdAsync(id);

            return View(doctor);
        }

        [HttpGet]
        [Route("[action]/{id}")]

        public async Task<IActionResult> Edit(int id)
        {
            var doctor = await _doctorApiService.GetDoctorByIdAsync(id);

            if (doctor == null)
                return NotFound();

            var specializations = await _specializationApiService.GetAllSpecializationsAsync();
            var departments = await _departmentApiService.GetDepartmentsAsync();

            //ViewBag for Dropdowns
            ViewBag.Specializations = new SelectList(specializations, "Id", "Name");

            ViewBag.Departments = new SelectList(departments, "Id", "Name");

            var model = new UpdateDoctorDto
            {
                Id = doctor.Id,
                FullName = doctor.FullName,
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

        [HttpPost]
        [Route("[action]/{id}")]
        public async Task<IActionResult> Edit(int id, UpdateDoctorDto updateDoctorDto)
        {
            if (!ModelState.IsValid)
            {
                var specializations = await _specializationApiService.GetAllSpecializationsAsync();
                var departments = await _departmentApiService.GetDepartmentsAsync();

                //ViewBag for Dropdowns
                ViewBag.Specializations = new SelectList(specializations, "Id", "Name");

                ViewBag.Departments = new SelectList(departments, "Id", "Name");

                return View(updateDoctorDto);   
            }
            await _doctorApiService.UpdateDoctorAsync(updateDoctorDto);

            TempData["Success"] = "Doctor updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var doctor = await _doctorApiService.GetDoctorByIdAsync(id);


            return View(doctor);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _doctorApiService.DeleteDoctorAsync(id);

            TempData["Success"] = "Doctor Deleted Successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
