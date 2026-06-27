using DoctorHub.Application.DTOs.Doctors;
using DoctorsHub.Application.DTOs.Doctors;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorHub.Application.Interfaces
{
    public interface IDoctorService
    {
        Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto? createDoctorDto);
        
        Task<List<DoctorDto>> GetAllDoctorsAsync();
        Task<DoctorDto?> GetByIdAsync(int id);
        Task<UpdateDoctorDto?> GetDoctorForUpdateById(int id);
        Task UpdateDoctorAsync(int id, UpdateDoctorDto updateDoctorDto);
        Task DeleteDoctorAsync(int id);

    }
}
