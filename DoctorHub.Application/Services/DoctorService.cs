using DoctorHub.Application.DTOs.Doctors;
using DoctorHub.Application.Interfaces;
using DoctorsHub.Application.DTOs.Doctors;
using DoctorsHub.Application.Interfaces.RepositoryContracts;
using DoctorsHub.Domain.Entities;
using DoctorsHub.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorHub.Application.Services
{
    public class DoctorService : IDoctorService
    {
        //Private Feilds
        private readonly IDoctorRepository _doctorRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        //Constructor
        public DoctorService(IDoctorRepository doctorRepository, UserManager<ApplicationUser> userManager) 
        {
            _doctorRepository  = doctorRepository;
            _userManager = userManager;
        }

        public async Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto createDoctorDto)
        {
            var existingEmail = await _userManager.FindByEmailAsync(createDoctorDto.Email);

            if (existingEmail != null)
            {
                throw new InvalidOperationException($"Doctor with Email: {createDoctorDto.Email}  already Exists.");
            }

            var user = new ApplicationUser 
            {
                UserName = createDoctorDto.Email,
                Email = createDoctorDto.Email,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, createDoctorDto.Password);

            if (!result.Succeeded) 
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException(errors);
            }
            var roleResult = await _userManager.AddToRoleAsync(user, "Doctor");

            if (!roleResult.Succeeded) 
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
            }
            var doctor = createDoctorDto.ToDoctor(user.Id);

            doctor = await _doctorRepository.AddAsync(doctor);

            return doctor.ToDoctorDto();
        }

        public async Task DeleteDoctorAsync(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null)
            {
                throw new KeyNotFoundException($"No doctor exists with Id ={id}");
            }

            await _doctorRepository.DeleteDoctorByIdAsync(id);
        }

        public async Task<List<DoctorDto>> GetAllDoctorsAsync()
        {
            var doctors = await _doctorRepository.GetDoctorsAsync();

            return doctors.Select(d => new DoctorDto
            {
                Id = d.Id,
                FullName = d.FullName,
                Qualification = d.Qualification,
                ConsultationFee = d.ConsultationFee,
                Email = d.User.Email?? string.Empty,
                ExperienceYears = d.ExperienceYears,
                SpecializationName  = d.Specialization.Name,
                AverageRating = d.AverageRating,

            }).ToList();
        }

        public async Task<(List<DoctorDto> Data, int TotalCount)> GetAllDoctorsAsync(string? searchBy, string? searchString, string? sortBy, string? sortOrder, int pageSize, int pageNumber)
        {
            var (doctors, TotalRecords) = await _doctorRepository.GetAllDoctorsAsync(searchBy, searchString, sortBy, sortOrder, pageSize, pageNumber);

            var doctlist = doctors.Select(d => new DoctorDto
            {
                Id = d.Id,
                FullName = d.FullName,
                Email = d.User?.Email ?? "N/A",
                Qualification = d.Qualification,
                ExperienceYears = d.ExperienceYears,
                ConsultationFee = d.ConsultationFee,
                SpecializationId = d.SpecializationId,

                SpecializationName = d.Specialization != null
        ? d.Specialization.Name
        : "N/A"
            }).ToList();

            return (doctlist, TotalRecords);
        }

        public async Task<DoctorDto?> GetByIdAsync(int id)
        {
            var doctor = await  _doctorRepository.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new KeyNotFoundException($"No doctor exists with Id ={id}");
            }

            return doctor.ToDoctorDto();
        }

        public async Task<UpdateDoctorDto?> GetDoctorForUpdateById(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new KeyNotFoundException($"No doctor exists with Id: {id}");
            }

            return doctor.ToUpdateDoctorDto();
        }

        public async Task UpdateDoctorAsync(int id, UpdateDoctorDto updateDoctorDto)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new KeyNotFoundException($"No doctor exists with Id: {id}");
            }
            doctor.UpdateDoctor(updateDoctorDto);

            await _doctorRepository.UpdateDoctorAsync(doctor);
        }
    }
}
