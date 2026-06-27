using DoctorHub.Application.DTOs.Doctors;
using DoctorHub.Application.Interfaces;
using DoctorsHub.Application.DTOs.Doctors;
using DoctorsHub.Application.Interfaces;
using DoctorsHub.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Numerics;

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
        [Route("[action]")]
        [Route("/")]
        public async Task<IActionResult> Index(
            string? searchBy =nameof(Doctor.FullName),
            string? searchString = "",
            string? sortBy = nameof(Doctor.FullName),
            string? sortOrder= "asc",
            int pageSize = 5,
            int pageNumber = 1)
        {
            var (data, totalCount) = await _doctorService.GetAllDoctorsAsync(
                searchBy,
                searchString,
                sortBy,
                sortOrder,
                pageSize,
                pageNumber
            );

            var result = new PagedResult<DoctorDto>
            {
                Items = data,
                TotalRecords = totalCount,
                PageSize = pageSize,
                PageNumber = pageNumber
            };

            ViewBag.SearchBy = searchBy;
            ViewBag.SearchString = searchString;
            ViewBag.SortBy = sortBy;
            ViewBag.SortOrder = sortOrder;

            return View(result);
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Create() 
        {
            
            var specializations = await _specializationService.GetAllSpecialization();

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
                return View(createDoctorDto);
            }

            await _doctorService.CreateDoctorAsync(createDoctorDto);

            TempData["Success"] = "Doctor created successfully.";
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

            if (doctor == null)
                return NotFound();

            ViewBag.Specializations = new SelectList(
                await _specializationService.GetAllSpecialization(),
                "Id",
                "Name",
                doctor.SpecializationId
            );

            return View(doctor);
        }

        [HttpPost]
        [Route("[action]/{id}")]
        public async Task<IActionResult> Edit(int id, UpdateDoctorDto updateDoctorDto) 
        {
            if (!ModelState.IsValid)
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Specializations = new SelectList(
                        await _specializationService.GetAllSpecialization(),
                        "Id",
                        "Name",
                        updateDoctorDto.SpecializationId
                    );

                    return View(updateDoctorDto);
                }
            }
            await _doctorService.UpdateDoctorAsync(id, updateDoctorDto);

            TempData["Success"] = "Doctor updated successfully.";
            return RedirectToAction(nameof(Index));
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
