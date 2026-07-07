using DoctorHub.Application.DTOs.Doctors;
using DoctorsHub.Application.DTOs.common;
using DoctorsHub.Application.DTOs.common.DoctorsHub.Application.DTOs.Common;
using DoctorsHub.Application.Interfaces.ServiceContracts;
using DoctorsHub.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Numerics;

namespace DoctorsHub.Web.Controllers
{
    [Route("[controller]")]
    public class DoctorController : Controller
    {
        //Private Feilds
        private readonly DoctorApiService _doctorApiService;
        private readonly SpecializationApiService _specializationApiService;

        //Constructor
        public DoctorController(DoctorApiService doctorApiService, SpecializationApiService specializationApiService) 
        {
            _doctorApiService = doctorApiService;
            _specializationApiService = specializationApiService;
        }


        [HttpGet]
        [Route("[action]")]
        [Route("/")]
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
            ViewBag.Specializations = new SelectList(
                specializations,
                "Id",
                "Name"
                );

            return View(new CreateDoctorDto());
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create(CreateDoctorDto createDoctorDto)
        {
            if (!ModelState.IsValid)
            {
                var specializations = await _specializationApiService.GetAllSpecializationsAsync();
                ViewBag.Specializations = new SelectList(
                    specializations,
                    "Id",
                    "Name"
                    );
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

            ViewBag.Specializations = new SelectList(
                await _specializationApiService.GetAllSpecializationsAsync(),
                "Id",
                "Name",
                doctor.SpecializationId
            );

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
                ViewBag.Specializations = new SelectList(
                    await _specializationApiService.GetAllSpecializationsAsync(),
                    "Id",
                    "Name",
                    updateDoctorDto.SpecializationId
                );

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
