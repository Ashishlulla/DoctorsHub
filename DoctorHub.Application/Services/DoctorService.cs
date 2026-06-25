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
    public class DoctorService :IDoctorService
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

            return doctor.ToDoctorDto(user.Email);
        }
    }
}
